<script setup>
import {
  NForm,
  NFormItem,
  NInput,
  NButton,
  NSpace,
  NSelect,
  NRadioGroup,
  NRadioButton,
} from "naive-ui";
import { ref } from "vue";
import { FILTER_ROLES } from "../../constants";

const FilterItemsRolesPair = Object.entries(FILTER_ROLES);
const props = defineProps({
  onFilterHandler: {
    type: Function,
    required: true,
  },
});

const filterModel = ref({
  login: null,
  role: FILTER_ROLES.All,
  code: null,
  address: null,
});

const onFilter = () => props.onFilterHandler(filterModel.value);
</script>

<template>
  <n-form inline :label-width="80" :model="filterModel">
    <n-space vertical>
      <n-form-item label="Login"
        ><n-input
          v-model:value="filterModel.login"
          type="text"
          placeholder="Login"
      /></n-form-item>
      <n-form-item label="Status">
        <n-radio-group v-model:value="filterModel.role" name="Role">
          <n-radio-button
            v-for="[key, value] in FilterItemsRolesPair"
            :key="key"
            :value="value"
            :label="key"
          />
        </n-radio-group>
      </n-form-item>
      <n-form-item label="Code"
        ><n-input
          v-model:value="filterModel.code"
          type="text"
          placeholder="Code"
      /></n-form-item>
      <n-form-item label="Address"
        ><n-input
          v-model:value="filterModel.address"
          type="text"
          placeholder="Address"
      /></n-form-item>
      <n-form-item>
        <n-button type="primary" @click="onFilter">Filter</n-button>
      </n-form-item>
    </n-space>
  </n-form>
</template>
