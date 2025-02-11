<script setup>
import { NGrid, NGridItem, NButton, NSpace } from "naive-ui";
import UserAuth from "./UserAuth.vue";
import { useRouter } from "vue-router";
import { computed, inject } from "vue";
import { ROLES } from "../constants";

const router = useRouter();
const userInfoRef = inject("userInfoRef", null);

const hasCustomersLink = computed(
  () => userInfoRef.value?.role === ROLES.Customer
);
const hasManagerLink = computed(
  () => userInfoRef.value?.role === ROLES.Manager
);
</script>

<template>
  <n-grid x-gap="12" :cols="12">
    <n-grid-item :span="11">
      <n-space>
        <n-button type="primary" @click="() => router.push('/')">Home</n-button>
        <n-button type="primary" @click="() => router.push('/orders')"
          >Orders</n-button
        >
        <n-button type="primary" @click="() => router.push('/items')"
          >Items</n-button
        >
        <n-button
          v-if="hasManagerLink"
          type="primary"
          @click="() => router.push('/users')"
          >Users</n-button
        >
      </n-space>
    </n-grid-item>
    <n-grid-item :span="1"><UserAuth /></n-grid-item>
  </n-grid>
</template>

<style scoped>
</style>
