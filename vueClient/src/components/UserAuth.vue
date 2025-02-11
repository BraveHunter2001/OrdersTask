<script setup>
import { postAsync } from "../axios";
import { AUTH_URL, LOGOUT_URL } from "../constants";
import {
  NButton,
  NH6,
  NModal,
  NCard,
  NInput,
  NForm,
  NFormItem,
  useMessage,
} from "naive-ui";
import { ref, inject } from "vue";

const props = defineProps({
  show: Boolean,
});

const loginModel = ref({
  Login: null,
  Password: null,
});
const showModal = ref(false);
const userInfoRef = inject("userInfoRef", null);

const message = useMessage();

const login = async () => {
  const { isOk, data } = await postAsync(AUTH_URL, loginModel.value);

  if (isOk) {
    showModal.value = false;

    //todo: сделать лучше переход с обновлением информ пользователя
    window.location.href = "/";
    message.success("Succsesfull login");
  }
};

const logout = async () => {
  const { isOk } = await postAsync(LOGOUT_URL);

  if (isOk) {
    window.location.href = "/";
    message.success("Succsesfull logout");
  }
};
</script>

<template>
  <NH6 v-if="userInfoRef"
    >{{ userInfoRef.login }} | {{ userInfoRef.roleName }}</NH6
  >
  <NButton v-if="userInfoRef" secondary @click="logout">Logout</NButton>
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
      <n-form :model="loginModel">
        <n-form-item label="Login">
          <n-input
            v-model:value="loginModel.Login"
            type="text"
            placeholder="Login"
          />
        </n-form-item>
        <n-form-item label="Password">
          <n-input
            v-model:value="loginModel.Password"
            type="password"
            show-password-on="mousedown"
            placeholder="Password"
          />
        </n-form-item>
        <n-form-item><NButton @click="login">Login</NButton></n-form-item>
      </n-form>
    </n-card>
  </n-modal>
</template>
