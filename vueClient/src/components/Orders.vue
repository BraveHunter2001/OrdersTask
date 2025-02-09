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
import OrderFilter from "./OrderFilter.vue";
import { h, onMounted, ref } from "vue";
import { getAsync, postAsync } from "../axios";
import {
  FilterOrderStatus,
  GET_ACCEPT_ORDER_URL,
  GET_CLOSE_ORDER_URL,
  ORDER_STATUS,
  PAGIANTED_ORDER_LIST_URL,
} from "../constants";

const buildActionButtons = (row, accept, close) => {
  const acceptButton = h(
    NButton,
    {
      strong: true,
      tertiary: true,
      size: "small",
      onClick: () => accept(row),
    },
    "Accept"
  );

  const closeButton = h(
    NButton,
    {
      strong: true,
      tertiary: true,
      size: "small",
      onClick: () => close(row),
    },
    "Close"
  );

  switch (row.status) {
    case ORDER_STATUS.New:
      return [acceptButton, closeButton];

    case ORDER_STATUS.InProgress:
      return [closeButton];

    default:
      return [];
  }
};

const buildColumns = ({ accept, close }) => {
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
        return h(NButtonGroup, { size: "small" }, () =>
          buildActionButtons(row, accept, close)
        );
      },
    },
  ];
};

const message = useMessage();

const handleAcceptOrder = async (orderId) => {
  const { isOk } = await postAsync(GET_ACCEPT_ORDER_URL(orderId));
  if (isOk) {
    message.success("Order was accepted");
    await getOrders();
  }
};

const handleCloseOrder = async (orderId) => {
  const { isOk } = await postAsync(GET_CLOSE_ORDER_URL(orderId));
  if (isOk) {
    message.success("Order was canceled");
    await getOrders();
  }
};

const columns = buildColumns({
  accept: (row) => handleAcceptOrder(row.orderId),
  close: (row) => handleCloseOrder(row.orderId),
});

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