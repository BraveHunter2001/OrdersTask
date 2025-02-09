<script setup>
import { RouterView } from "vue-router";
import UserInfoProvider from "./components/UserInfoProvider.vue";
import Navigation from "./components/Navigation.vue";
import { NMessageProvider } from "naive-ui";
</script>

<template>
  <UserInfoProvider>
    <n-message-provider>
      <Navigation />
      <RouterView />
    </n-message-provider>
  </UserInfoProvider>
</template>

<style scoped>
</style>
