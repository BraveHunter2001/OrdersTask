<script setup>
import { AUTH_URL } from '../constants';
import Modal from './Modal.vue';
import { ref } from 'vue';

const props = defineProps({
  show: Boolean
})

const loginModel = ref({Login:"john_manager", Password:"securepassword123"})

const login = async () =>{
    console.log("login", loginModel.value);
    const response =  await fetch(AUTH_URL, {
    method: "POST",
    headers: {
    "Content-Type": "application/json",
    },
    credentials: 'include',
    mode: 'cors',
    body:JSON.stringify(loginModel.value)
    });

    console.log(response);
}

</script>

<template>
<Modal :show="show" @close="$emit('close')">
    <template #header>
        <h3>Login</h3>
    </template>
    <template #body>
        <input v-model="loginModel.Login" placeholder="input login">
        <input v-model="loginModel.Password" type="password" placeholder="input password">
    </template>
    <template #footer>
        <button type="submit" @click="login">Login</button>
    </template>
</Modal>
</template>
