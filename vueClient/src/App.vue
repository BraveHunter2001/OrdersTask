<script setup>
import { RouterView } from "vue-router";
import UserInfoProvider from "./components/UserInfoProvider.vue";
import Navigation from "./components/Navigation.vue";
</script>

<template>
  <UserInfoProvider>
    <Navigation />
    <RouterView />
  </UserInfoProvider>
</template>

<style scoped>
</style>
