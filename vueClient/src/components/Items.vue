
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
} from "naive-ui";

import { h, onMounted, ref } from "vue";
import { getAsync, postAsync } from "../axios";
import { PAGIANTED_ITEMS_LIST_URL } from "../constants";
import ItemFilter from "./ItemFilter.vue";

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

const buildColumns = ({ accept, close }) => {
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
          buildActionButtons(row, accept, close)
        );
      },
    },
  ];
};

const columns = buildColumns({
  change: (row) => console.log(row),
  del: (row) => console.log(row),
});

const message = useMessage();

const handleFilter = async (filterModel) => await getItems(filterModel);

const PAGE_SIZE = 10;
const page = ref(1);
const pageCount = ref(0);
const items = ref([]);

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
</script>

<template>
  <n-grid :cols="12">
    <n-grid-item :span="4"
      ><ItemFilter :onFilterHandler="handleFilter"
    /></n-grid-item>
    <n-grid-item :span="8">
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
</template>
