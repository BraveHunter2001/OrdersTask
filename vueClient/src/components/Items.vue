
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

import { h, onMounted, ref, watch } from "vue";
import { deleteAsync, getAsync, patchAsync, postAsync } from "../axios";
import {
  GET_ITEM_ID_URL,
  ITEMS_URL,
  PAGIANTED_ITEMS_LIST_URL,
} from "../constants";
import ItemFilter from "./ItemFilter.vue";
import ItemCard from "./ItemCard.vue";

const buildActionButtons = (row, change, del) => {
  const changeButton = h(
    NButton,
    {
      strong: true,
      tertiary: true,
      size: "small",
      onClick: () => change(row),
    },
    "Change"
  );

  const deleteButton = h(
    NButton,
    {
      strong: true,
      tertiary: true,
      size: "small",
      onClick: () => del(row),
    },
    "Delete"
  );

  return [changeButton, deleteButton];
};

const buildColumns = ({ change, del }) => {
  return [
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
    {
      title: "Action",
      key: "actions",
      render(row) {
        return h(NButtonGroup, { size: "small" }, () =>
          buildActionButtons(row, change, del)
        );
      },
    },
  ];
};

const columns = buildColumns({
  change: (row) => handleChangeItem(row.itemId),
  del: (row) => handleDeleteItem(row.itemId),
});

const message = useMessage();

const handleChangeItem = async (itemId) => {
  isChangeItemModel.value = true;
  const { isOk, data } = await getAsync(GET_ITEM_ID_URL(itemId));
  if (!isOk) {
    message.error("Couldn't get item to change");
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
  } else message.error("failed to receive items");
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
    message.info("Nothing change");
    return;
  }

  const { isOk, data } = await patchAsync(
    GET_ITEM_ID_URL(selectedItem.value.id),
    changes
  );

  if (isOk) {
    message.success("Item was changed");
    getItems();
  } else {
    let mes = "";
    for (const message of data) mes += message;
    message.error(mes);
  }
};

const AddItem = async (model) => {
  const { isOk, data } = await postAsync(ITEMS_URL, model);

  if (isOk) {
    message.success("Item successful added");
    getItems();
  } else {
    let mes = "";

    for (const message of data) mes += message;
    message.error(mes);
  }
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
