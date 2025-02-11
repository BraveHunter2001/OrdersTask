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
import { h, inject, onMounted, ref } from "vue";
import { deleteAsync, getAsync, postAsync } from "../axios";
import {
  FilterOrderStatus,
  GET_ACCEPT_ORDER_URL,
  GET_CLOSE_ORDER_URL,
  GET_ORDER_ID_URL,
  ORDER_STATUS,
  PAGIANTED_ORDER_LIST_URL,
  ROLES,
} from "../constants";

const buildActionButtons = (row, accept, close, del) => {
  const isCustomer = userInfoRef.value.role === ROLES.Customer;
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

  const delButton = h(
    NButton,
    {
      strong: true,
      tertiary: true,
      size: "small",
      onClick: () => del(row),
    },
    "Delete"
  );

  switch (row.status) {
    case ORDER_STATUS.New:
      return isCustomer ? [delButton] : [acceptButton, closeButton];

    case ORDER_STATUS.InProgress:
      return isCustomer ? [] : [closeButton];

    default:
      return [];
  }
};

const buildColumns = ({ accept, close, del }) => {
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
          buildActionButtons(row, accept, close, del)
        );
      },
    },
  ];
};

const columns = buildColumns({
  accept: (row) => handleAcceptOrder(row.orderId),
  close: (row) => handleCloseOrder(row.orderId),
  del: (row) => handleDeleteOrder(row.orderId),
});

const message = useMessage();
const handleFilter = async (filterModel) => await getOrders(filterModel);
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

const handleDeleteOrder = async (orderId) => {
  await deleteAsync(GET_ORDER_ID_URL(orderId));
  getOrders();
};

const PAGE_SIZE = 20;
const page = ref(1);
const pageCount = ref(0);
const orders = ref([]);
const userInfoRef = inject("userInfoRef", null);

onMounted(async () => await getOrders());

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
      ><OrderFilter :onFilterHandler="handleFilter"
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
          :on-update:page="
            (num) => {
              page = num;
              getOrders();
            }
          "
          simple /></n-space
    ></n-grid-item>
  </n-grid>
</template>

<style scoped>
</style>