<script setup>
import {
  NForm,
  NFormItem,
  NInput,
  NButton,
  NSpace,
  useMessage,
  NDataTable,
  NModal,
  NSelect,
  NCard,
  NRadioGroup,
  NRadioButton,
} from "naive-ui";
import { ROLES } from "../../constants";
import { ref, toRaw } from "vue";

const props = defineProps({
  onSubmitHandler: {
    type: Function,
    required: true,
  },

  user: Object,
});
const RolesPair = Object.entries(ROLES);

const userModalRef = ref(
  (props.user && structuredClone(toRaw(props.user))) ?? {
    login: null,
    password: null,
    role: 0,
    address: null,
    name: null,
    discount: null,
  }
);
const onSubmit = () => props.onSubmitHandler(userModalRef.value);
</script>

<template>
  <n-form inline :label-width="80" :model="userModalRef">
    <n-space vertical>
      <n-form-item label="Login"
        ><n-input
          v-model:value="userModalRef.login"
          type="text"
          placeholder="Login"
      /></n-form-item>
      <n-form-item label="Password"
        ><n-input
          v-model:value="userModalRef.password"
          type="text"
          placeholder="Password"
      /></n-form-item>
      <n-form-item label="Role">
        <n-radio-group v-model:value="userModalRef.role" name="Role">
          <n-radio-button
            v-for="[key, value] in RolesPair"
            :key="key"
            :value="value"
            :label="key"
          />
        </n-radio-group>
      </n-form-item>
      <n-form-item
        v-if="userModalRef?.code && userModalRef.role === ROLES.Customer"
        label="Code"
        ><n-input
          v-model:value="userModalRef.code"
          type="text"
          placeholder="Code"
      /></n-form-item>
      <n-form-item v-if="userModalRef.role === ROLES.Customer" label="Name"
        ><n-input
          v-model:value="userModalRef.name"
          type="text"
          placeholder="Name"
      /></n-form-item>
      <n-form-item v-if="userModalRef.role === ROLES.Customer" label="Address"
        ><n-input
          v-model:value="userModalRef.address"
          type="text"
          placeholder="Address"
      /></n-form-item>
      <n-form-item v-if="userModalRef.role === ROLES.Customer" label="Discount"
        ><n-input
          v-model:value="userModalRef.discount"
          type="text"
          placeholder="Discount"
      /></n-form-item>
      <n-form-item>
        <n-button type="primary" @click="onSubmit">Submit</n-button>
      </n-form-item>
    </n-space>
  </n-form>
</template>

<style scoped>
</style>
