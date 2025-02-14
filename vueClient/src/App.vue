<script setup>
import { RouterView } from "vue-router";
import UserInfoProvider from "./components/UserComponents/UserInfoProvider.vue";
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
