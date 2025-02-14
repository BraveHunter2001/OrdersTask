<script setup>
import { ref, provide, onMounted } from "vue";
import { USER_INFO_URL } from "../../constants";
import { getAsync } from "../../axios";

const userInfo = ref(null);
provide("userInfoRef", userInfo);

onMounted(async () => {
  const { isOk, data } = await getAsync(USER_INFO_URL);
  if (isOk) userInfo.value = data;
});
</script>

<template>
  <slot></slot>
</template>

<style scoped>
</style>
