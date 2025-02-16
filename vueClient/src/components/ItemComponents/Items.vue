
<script setup>
import {
  NGrid,
  NGridItem,
  NPagination,
  NButton,
  NButtonGroup,
  NSpace,
  useMessage,
  NDataTable,
  NModal,
  NCard,
} from "naive-ui";

import { computed, h, inject, onMounted, ref, watch } from "vue";
import { deleteAsync, getAsync, patchAsync, postAsync } from "../../axios";
import {
  CART_ACTION_TYPE,
  GET_ITEM_ID_URL,
  ITEMS_URL,
  MY_CART_URL,
  ORDER_URL,
  PAGIANTED_ITEMS_LIST_URL,
  ROLES,
} from "../../constants";
import ItemFilter from "./ItemFilter.vue";
import ItemCard from "./ItemCard.vue";
import { showErrorMessages } from "../../utils";
import {
  BaseItemColumn,
  buildActionListButton,
  getSelectedItems,
} from "../componentUtils";
import Cart from "./Cart.vue";

const buildActionButtons = (row, change, del) => {
  const changeButton = buildActionListButton("Change", () => change(row));
  const deleteButton = buildActionListButton("Delete", () => del(row));
  return [changeButton, deleteButton];
};

const buildColumns = ({ change, del }) => {
  let mainCols = [];
  if (!isManager.value)
    mainCols.push({
      type: "selection",
    });

  mainCols = [...mainCols, ...BaseItemColumn];

  if (isManager.value)
    mainCols.push({
      title: "Action",
      key: "actions",
      render(row) {
        return h(NButtonGroup, { size: "small" }, () =>
          buildActionButtons(row, change, del)
        );
      },
    });

  return mainCols;
};

const messager = useMessage();

const handleChangeItem = async (itemId) => {
  isChangeItemModel.value = true;
  const { isOk, data } = await getAsync(GET_ITEM_ID_URL(itemId));
  if (!isOk) {
    messager.error("Couldn't get item to change");
    return;
  }

  showItemModal.value = true;
  selectedItem.value = data;
};

const handleDeleteItem = async (itemId) => {
  await deleteAsync(GET_ITEM_ID_URL(itemId));
  getItems();
};

const handleFilter = async (filterModel) => await getItems(filterModel);

const PAGE_SIZE = 10;
const page = ref(1);
const pageCount = ref(0);
const items = ref([]);
const showItemModal = ref(false);
const showCreateOrderModal = ref(false);
const selectedItem = ref(null);
const isChangeItemModel = ref(false);
const userInfoRef = inject("userInfoRef", null);
const isManager = computed(() => userInfoRef.value?.role === ROLES.Manager);
const selectedItemRows = ref([]);

const columns = buildColumns({
  change: (row) => handleChangeItem(row.itemId),
  del: (row) => handleDeleteItem(row.itemId),
});

onMounted(async () => await getItems());

const getItems = async (filter) => {
  let url =
    PAGIANTED_ITEMS_LIST_URL +
    `?pageSize=${PAGE_SIZE}&pageIndex=${page.value - 1}`;

  if (filter?.code) {
    url += `&code=${filter?.code}`;
  }

  if (filter?.categories) {
    for (const cat of filter?.categories) {
      url += `&categories=${cat}`;
    }
  }

  const { isOk, data } = await getAsync(url);
  if (isOk) {
    items.value = data.value;
    pageCount.value = data.totalPages;
  } else messager.error("failed to receive items");
};

const handleSubmitItem = async (model) => {
  if (isChangeItemModel.value) {
    await ChangeItem(model);
  } else await AddItem(model);
};

const ChangeItem = async (model) => {
  const changes = {};

  for (const key in selectedItem.value) {
    if (selectedItem.value[key] != model[key]) changes[key] = model[key];
  }

  if (Object.keys(changes).length == 0) {
    messager.info("Nothing change");
    return;
  }

  const { isOk, data } = await patchAsync(
    GET_ITEM_ID_URL(selectedItem.value.id),
    changes
  );

  if (isOk) {
    messager.success("Item was changed");
    getItems();
  } else showErrorMessages(messager, data);
};

const AddItem = async (model) => {
  const { isOk, data } = await postAsync(ITEMS_URL, model);

  if (isOk) {
    messager.success("Item successful added");
    getItems();
  } else showErrorMessages(messager, data);
};

watch(showItemModal, (newValue) => {
  if (newValue) return;
  selectedItem.value = null;
});

const HandleAddItemsToCart = async () => {
  const selitems = getSelectedItems(
    selectedItemRows.value,
    items.value,
    (i, ids) => ids.has(i.itemId)
  );
  const request = {
    cartItems: selitems.map((si) => ({
      itemId: si.itemId,
      count: 1,
    })),
    actionType: CART_ACTION_TYPE.AddItems,
  };
  const { isOk, data } = await patchAsync(MY_CART_URL, request);

  if (isOk) {
    messager.success("Items  succseccsul added into cart");
  } else showErrorMessages(messager, data);
};
</script>

<template>
  <n-grid :cols="12">
    <n-grid-item :span="4"
      ><ItemFilter :onFilterHandler="handleFilter"
    /></n-grid-item>
    <n-grid-item :span="8">
      <n-button
        v-if="isManager"
        @click="
          () => {
            showItemModal = true;
            isChangeItemModel = false;
          }
        "
        >Add</n-button
      >
      <n-button v-if="!isManager" @click="HandleAddItemsToCart"
        >Add to Cart</n-button
      >
      <n-button
        v-if="!isManager"
        @click="
          () => {
            showCreateOrderModal = true;
          }
        "
        >Open Cart</n-button
      >
      <n-p v-if="selectedItemRows?.length > 0">
        You have selected {{ selectedItemRows.length }} item{{
          selectedItemRows.length < 2 ? "" : "s"
        }}.
      </n-p>
      <n-data-table
        :columns="columns"
        :data="items"
        :pagination="false"
        :bordered="false"
        :row-key="(row) => row.itemId"
        @update:checked-row-keys="
          (keys) => {
            selectedItemRows = keys;
          }
        "
    /></n-grid-item>
    <n-grid-item :span="12"
      ><n-space justify="center"
        ><n-pagination
          v-model:page="page"
          :page-count="pageCount"
          :on-update:page="
            (num) => {
              page = num;
              getItems();
            }
          "
          simple /></n-space
    ></n-grid-item>
  </n-grid>
  <n-modal v-model:show="showItemModal"
    ><n-card
      style="width: 600px"
      title="Item"
      size="small"
      role="dialog"
      aria-modal="true"
      ><ItemCard
        :item="selectedItem"
        :onSubmitHandler="handleSubmitItem" /></n-card
  ></n-modal>
  <n-modal v-model:show="showCreateOrderModal"
    ><n-card
      style="width: 600px"
      title="Cart"
      size="small"
      role="dialog"
      aria-modal="true"
      ><Cart :openCart="showCreateOrderModal" /></n-card
  ></n-modal>
</template>
