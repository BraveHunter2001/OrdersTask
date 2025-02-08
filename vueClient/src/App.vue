<script setup>
import AuthModal from './components/AuthModal.vue';
import { ref } from 'vue';


const showModal = ref(false)
</script>

<template>
  <button @click="()=>showModal = !showModal">Auth</button>
  <AuthModal :show="showModal" @close="showModal = false"/>
</template>

<style scoped>

</style>
