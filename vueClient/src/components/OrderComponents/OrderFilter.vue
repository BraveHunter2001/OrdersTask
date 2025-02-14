<script setup>
import {
  NForm,
  NFormItem,
  NButton,
  NSpace,
  NRadioGroup,
  NRadioButton,
} from "naive-ui";
import { ref } from "vue";
import { FilterOrderStatus } from "../../constants";

const props = defineProps({
  onFilterHandler: {
    type: Function,
    required: true,
  },
});

const FilterOrderStatusPair = Object.entries(FilterOrderStatus);

const filterModel = ref({ status: FilterOrderStatus.All });

const onFilter = () => {
  props.onFilterHandler(filterModel.value);
};
</script>

<template>
  <n-form inline :label-width="80" :model="filterModel">
    <n-space vertical>
      <n-form-item label="Status">
        <n-radio-group v-model:value="filterModel.status" name="status">
          <n-radio-button
            v-for="[key, value] in FilterOrderStatusPair"
            :key="key"
            :value="value"
            :label="key"
          />
        </n-radio-group>
      </n-form-item>
      <n-form-item>
        <n-button type="primary" @click="onFilter">Filter</n-button>
      </n-form-item>
    </n-space>
  </n-form>
</template>

<style scoped>
</style>