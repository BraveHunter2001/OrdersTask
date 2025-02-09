<script setup>
import { postAsync } from "../axios";
import { AUTH_URL } from "../constants";
import { NButton, NH6, NModal, NCard, NInput } from "naive-ui";
import { ref, inject, watch } from "vue";
import { useRouter } from "vue-router";

const props = defineProps({
  show: Boolean,
});

const loginModel = ref({
  Login: "john_manager",
  Password: "securepassword123",
});
const showModal = ref(false);
const userInfoRef = inject("userInfoRef", null);

const router = useRouter();

const login = async () => {
  const { isOk, data } = await postAsync(AUTH_URL, loginModel.value);

  if (isOk) {
    showModal.value = false;
    window.location.href = "/";
  }
};
</script>

<template>
  <NH6 v-if="userInfoRef"
    >{{ userInfoRef.login }} | {{ userInfoRef.roleName }}</NH6
  >
  <NButton v-else secondary @click="showModal = true">Auth</NButton>
  <n-modal v-model:show="showModal">
    <n-card
      style="width: 600px"
      title="Login"
      :bordered="false"
      size="huge"
      role="dialog"
      aria-modal="true"
    >
      <n-input
        v-model:value="loginModel.Login"
        type="text"
        placeholder="Login"
      />
      <n-input
        v-model:value="loginModel.Password"
        type="password"
        show-password-on="mousedown"
        placeholder="Password"
      />
      <template #footer><NButton @click="login">Login</NButton></template>
    </n-card>
  </n-modal>
</template>
