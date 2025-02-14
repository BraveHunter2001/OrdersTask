
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
  GET_ITEM_ID_URL,
  ITEMS_URL,
  PAGIANTED_ITEMS_LIST_URL,
  ROLES,
} from "../../constants";
import ItemFilter from "./ItemFilter.vue";
import ItemCard from "./ItemCard.vue";
import { showErrorMessages } from "../../utils";
import { buildActionListButton } from "../componentUtils";

const buildActionButtons = (row, change, del) => {
  const changeButton = buildActionListButton("Change", () => change(row));
  const deleteButton = buildActionListButton("Delete", () => del(row));
  return [changeButton, deleteButton];
};

const buildColumns = ({ change, del }) => {
  const mainCols = [
    {
      title: "Code",
      key: "code",
    },
    {
      title: "Name",
      key: "name",
    },
    {
      title: "Price",
      key: "price",
    },
    {
      title: "Category",
      key: "category",
    },
  ];

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
const selectedItem = ref(null);
const isChangeItemModel = ref(false);
const userInfoRef = inject("userInfoRef", null);
const isManager = computed(() => userInfoRef.value?.role === ROLES.Manager);

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
      <n-data-table
        :columns="columns"
        :data="items"
        :pagination="false"
        :bordered="false"
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
</template>
