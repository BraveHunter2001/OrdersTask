<script setup>
import {
  NGrid,
  NGridItem,
  NPagination,
  NButton,
  NSpace,
  useMessage,
  NDataTable,
} from "naive-ui";
import OrderFilter from "./OrderFilter.vue";
import { h, onMounted, ref } from "vue";
import { getAsync } from "../axios";
import { FilterOrderStatus, PAGIANTED_ORDER_LIST_URL } from "../constants";

function createColumns({ play }) {
  return [
    {
      title: "Number",
      key: "number",
    },
    {
      title: "Status",
      key: "statusName",
    },
    {
      title: "CreateDate",
      key: "createDate",
    },
    {
      title: "ShippingDate",
      key: "shippingDate",
    },
    {
      title: "Action",
      key: "actions",
      render(row) {
        return h(
          NButton,
          {
            strong: true,
            tertiary: true,
            size: "small",
            onClick: () => play(row),
          },
          { default: () => "Play" }
        );
      },
    },
  ];
}

const columns = createColumns(() => console.log("Play"));
const message = useMessage();
const PAGE_SIZE = 20;

const handlerFilter = async (filterModel) => {
  await getOrders(filterModel);
};
const page = ref(1);
const pageCount = ref(0);
const orders = ref([]);

onMounted(async () => {
  await getOrders();
});

const getOrders = async (filter) => {
  let url =
    PAGIANTED_ORDER_LIST_URL +
    `?pageSize=${PAGE_SIZE}&pageIndex=${page.value - 1}`;

  if (filter && filter.status !== FilterOrderStatus.All) {
    url += `&status=${filter.status - 1}`;
  }

  const { isOk, data } = await getAsync(url);
  if (isOk) {
    orders.value = data.value;
    pageCount.value = data.totalPages;
  } else message.error("failed to receive orders");
};
</script>

<template>
  <n-grid :cols="12">
    <n-grid-item :span="4"
      ><OrderFilter :onFilterHandler="handlerFilter"
    /></n-grid-item>
    <n-grid-item :span="8">
      <n-data-table
        :columns="columns"
        :data="orders"
        :pagination="false"
        :bordered="false"
    /></n-grid-item>
    <n-grid-item :span="12"
      ><n-space justify="center"
        ><n-pagination
          v-model:page="page"
          :page-count="pageCount"
          simple /></n-space
    ></n-grid-item>
  </n-grid>
</template>

<style scoped>
</style>