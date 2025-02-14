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
} from "naive-ui";
import { getAsync } from "../../axios";
import { GET_SUGGESTS_URL } from "../../constants";
import { ref, toRaw } from "vue";

const props = defineProps({
  onSubmitHandler: {
    type: Function,
    required: true,
  },

  item: Object,
});

const itemModalRef = ref(
  (props.item && structuredClone(toRaw(props.item))) ?? {
    name: null,
    category: null,
    price: 0,
  }
);
const loadingRef = ref(false);
const optionsRef = ref([]);

const onSubmit = () => {
  props.onSubmitHandler(itemModalRef.value);
};

const handleSearch = async (query) => {
  if (query?.length <= 3) {
    optionsRef.value = [{ label: `${query}`, value: query }];
    return;
  }

  loadingRef.value = true;
  const { isOk, data } = await getAsync(GET_SUGGESTS_URL(query));
  if (isOk && data?.length > 0) optionsRef.value = data;
  else optionsRef.value = [{ label: query, value: query }];
  loadingRef.value = false;
};
</script>

<template>
  <n-form inline :label-width="80" :model="itemModalRef">
    <n-space vertical>
      <n-form-item v-if="itemModalRef.code" label="Code"
        ><n-input
          v-model:value="itemModalRef.code"
          type="text"
          placeholder="Code"
      /></n-form-item>
      <n-form-item label="Name"
        ><n-input
          v-model:value="itemModalRef.name"
          type="text"
          placeholder="Name"
      /></n-form-item>
      <n-form-item label="Categories">
        <n-select
          v-model:value="itemModalRef.category"
          tag
          filterable
          placeholder="Input categories"
          :options="optionsRef"
          :loading="loadingRef"
          clearable
          remote
          :clear-filter-after-select="false"
          @search="handleSearch"
        />
      </n-form-item>
      <n-form-item label="Price"
        ><n-input
          v-model:value="itemModalRef.price"
          type="number"
          placeholder="Price"
      /></n-form-item>
      <n-form-item>
        <n-button type="primary" @click="onSubmit">Submit</n-button>
      </n-form-item>
    </n-space>
  </n-form>
</template>

<style scoped>
</style>
