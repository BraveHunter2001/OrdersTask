<script setup>
import {
  NForm,
  NFormItem,
  NInput,
  NButton,
  NSpace,
  useMessage,
  NDataTable,
  NModal,
  NSelect,
  NCard,
  NInputNumber,
} from "naive-ui";

import { h, ref, toRaw, watch } from "vue";
import { BaseItemColumn, getSelectedItems } from "../componentUtils";
import { deleteAsync, getAsync, postAsync } from "../../axios";
import { MY_CART_URL, ORDER_URL } from "../../constants";
import { onMounted } from "vue";

const props = defineProps({
  openCart: {
    type: Boolean,
  },
});
const messager = useMessage();
const columns = [
  {
    type: "selection",
  },
  ...BaseItemColumn,
  {
    title: "Count",
    key: "count",
    render(row, index) {
      return h(NInputNumber, {
        size: "tiny",
        min: "1",
        value: row.count,
        async onUpdateValue(v) {
          cartItemsRef.value[index].count = v;

          if (!v) return;
          var bodyFormData = new FormData();
          bodyFormData.append("count", v);
          const { isOk } = await postAsync(
            MY_CART_URL + `/${row.cartItemId}/count`,
            bodyFormData
          );

          if (!isOk) messager.error("Failed to update cart item count");
        },
      });
    },
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
          onClick: async () => {
            const { isOk } = await deleteAsync(
              MY_CART_URL + `/${row.cartItemId}`
            );

            cartItemsRef.value = cartItemsRef.value.filter(
              (i) => i.cartItemId != row.cartItemId
            );
            if (!isOk) messager.error("Failed to delete cart item count");
          },
        },
        { default: () => "Delete" }
      );
    },
  },
];

const cartItemsRef = ref([]);
const selectedItemRows = ref([]);

onMounted(async () => {
  const { isOk, data } = await getAsync(MY_CART_URL);
  cartItemsRef.value = data.items;

  const res = data.items.map((v) => v.cartItemId);
  selectedItemRows.value = res;

  if (!isOk) messager.error("Failed to load cart");
});

const onSubmit = async () => {
  const items = getSelectedItems(
    selectedItemRows.value,
    cartItemsRef.value,
    (i, ids) => ids.has(i.cartItemId)
  );

  const request = {
    items: items.map((si) => ({
      itemId: si.itemId,
      count: si.count,
    })),
  };

  const { isOk, data } = await postAsync(ORDER_URL, request);

  if (isOk) {
    messager.success("Order  succseccsul created");

    cartItemsRef.value = cartItemsRef.value.filter(
      (i) => !items.some((item) => item.cartItemId == i.cartItemId)
    );
  } else showErrorMessages(messager, data);
};
</script>

<template>
  <n-data-table
    :columns="columns"
    :data="cartItemsRef"
    :pagination="false"
    :bordered="false"
    :row-key="(row) => row.cartItemId"
    :checked-row-keys="selectedItemRows"
    @update:checked-row-keys="(keys) => (selectedItemRows = keys)"
  />
  <NButton v-if="cartItemsRef.length > 0" @click="onSubmit"
    >Create order</NButton
  >
</template>

<style scoped>
</style>
