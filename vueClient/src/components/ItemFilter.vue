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
import { handleError, ref } from "vue";
import { getAsync } from "../axios";
import { GET_SUGGESTS_URL } from "../constants";

const props = defineProps({
  onFilterHandler: {
    type: Function,
    required: true,
  },
});

const filterModel = ref({ code: null, categories: null });
const loadingRef = ref(false);
const optionsRef = ref([]);

const onFilter = () => {
  props.onFilterHandler(filterModel.value);
};

const handleSearch = async (query) => {
  if (query?.length <= 3) {
    optionsRef.value = [];
    return;
  }

  loadingRef.value = true;
  const { isOk, data } = await getAsync(GET_SUGGESTS_URL(query));
  if (isOk) optionsRef.value = data;
  loadingRef.value = false;
};
</script>

<template>
  <n-form inline :label-width="80" :model="filterModel">
    <n-space vertical>
      <n-form-item label="Code"
        ><n-input
          v-model:value="filterModel.code"
          type="text"
          placeholder="Code"
      /></n-form-item>
      <n-form-item label="Categories">
        <n-select
          v-model:value="filterModel.categories"
          multiple
          filterable
          placeholder="Search Songs"
          :options="optionsRef"
          :loading="loadingRef"
          clearable
          remote
          :clear-filter-after-select="false"
          @search="handleSearch"
        />
      </n-form-item>
      <n-form-item>
        <n-button type="primary" @click="onFilter">Filter</n-button>
      </n-form-item>
    </n-space>
  </n-form>
</template>
