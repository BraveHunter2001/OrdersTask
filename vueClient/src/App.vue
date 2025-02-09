<script setup>
import { ref } from "vue";
import UserAuth from "./components/UserAuth.vue";
import UserInfoProvider from "./components/UserInfoProvider.vue";
</script>

<template>
  <UserInfoProvider><UserAuth /></UserInfoProvider>
</template>

<style scoped>
</style>
