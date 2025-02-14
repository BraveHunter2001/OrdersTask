<script setup>
import {
  NGrid,
  NGridItem,
  NPagination,
  NButton,
  NButtonGroup,
  NSpace,
  useMessage,
  NDataTable,
  NModal,
  NCard,
} from "naive-ui";
import { h, onMounted, ref, watch } from "vue";
import { deleteAsync, getAsync, patchAsync, postAsync } from "../../axios";
import {
  FILTER_ROLES,
  GET_USER_ID_URL,
  PAGINATED_USERS_LIST_URL,
  USERS_URL,
} from "../../constants";
import { buildActionListButton } from "../componentUtils";
import UserFilter from "./UserFilter.vue";
import UserCard from "./UserCard.vue";
import { showErrorMessages } from "../../utils";

const buildActionButtons = (row, change, del) => {
  const delButton = buildActionListButton("Delete", () => del(row));
  const changeButton = buildActionListButton("Change", () => change(row));

  return [changeButton, delButton];
};

const buildColumns = ({ change, del }) => {
  return [
    {
      title: "Login",
      key: "login",
    },
    {
      title: "Role",
      key: "roleName",
    },
    {
      title: "Code",
      key: "code",
    },
    {
      title: "Name",
      key: "name",
    },
    {
      title: "Address",
      key: "address",
    },
    {
      title: "Discount",
      key: "discount",
    },
    {
      title: "Action",
      key: "actions",
      render(row) {
        return h(NButtonGroup, { size: "small" }, () =>
          buildActionButtons(row, change, del)
        );
      },
    },
  ];
};

const columns = buildColumns({
  change: (row) => handleChangeUser(row.userId),
  del: (row) => handleDeleteUser(row.userId),
});

const messager = useMessage();
const handleFilter = async (filterModel) => await getUsers(filterModel);

const PAGE_SIZE = 20;
const page = ref(1);
const pageCount = ref(0);
const users = ref([]);
const showUserModal = ref(false);
const selectedUser = ref(null);
const isChangeUserModel = ref(false);

onMounted(async () => await getUsers());

const handleDeleteUser = async (userId) => {
  await deleteAsync(GET_USER_ID_URL(userId));
  getUsers();
};

const handleChangeUser = async (userId) => {
  isChangeUserModel.value = true;
  const { isOk, data } = await getAsync(GET_USER_ID_URL(userId));

  if (!isOk) {
    messager.error("Couldn't get item to change");
    return;
  }

  showUserModal.value = true;
  selectedUser.value = data;
};

const getUsers = async (filter) => {
  let url =
    PAGINATED_USERS_LIST_URL +
    `?pageSize=${PAGE_SIZE}&pageIndex=${page.value - 1}`;

  if (filter?.login) url += `&login=${filter.login}`;
  if (filter?.role && filter.role != FILTER_ROLES.All)
    url += `&role=${filter.role - 1}`;
  if (filter?.code) url += `&code=${filter.code}`;
  if (filter?.address) url += `&address=${filter.address}`;

  const { isOk, data } = await getAsync(url);
  if (isOk) {
    users.value = data.value;
    pageCount.value = data.totalPages;
  } else messager.error("failed to receive users");
};

const handleSubmitUser = async (model) => {
  if (isChangeUserModel.value) {
    await ChangeUser(model);
  } else await AddUser(model);
};

const ChangeUser = async (model) => {
  const changes = {};

  for (const key in selectedUser.value) {
    if (selectedUser.value[key] != model[key]) changes[key] = model[key];
  }

  if (Object.keys(changes).length == 0) {
    messager.info("Nothing change");
    return;
  }

  const { isOk, data } = await patchAsync(
    GET_USER_ID_URL(selectedUser.value.userId),
    changes
  );

  if (isOk) {
    messager.success("User was changed");
    getUsers();
  } else showErrorMessages(messager, data);
};

const AddUser = async (model) => {
  const { isOk, data } = await postAsync(USERS_URL, model);

  if (isOk) {
    messager.success("User successful added");
    getUsers();
  } else showErrorMessages(messager, data);
};

watch(showUserModal, (newValue) => {
  if (newValue) return;
  selectedUser.value = null;
});
</script>

<template>
  <n-grid :cols="12">
    <n-grid-item :span="4"
      ><UserFilter :onFilterHandler="handleFilter"
    /></n-grid-item>
    <n-grid-item :span="8">
      <n-button
        @click="
          () => {
            showUserModal = true;
            isChangeUserModel = false;
          }
        "
        >Add</n-button
      >
      <n-data-table
        :columns="columns"
        :data="users"
        :pagination="false"
        :bordered="false"
    /></n-grid-item>
    <n-grid-item :span="12"
      ><n-space justify="center"
        ><n-pagination
          v-model:page="page"
          :page-count="pageCount"
          :on-update:page="
            (num) => {
              page = num;
              getUsers();
            }
          "
          simple /></n-space
    ></n-grid-item>
  </n-grid>
  <n-modal v-model:show="showUserModal"
    ><n-card
      style="width: 600px"
      title="Item"
      size="small"
      role="dialog"
      aria-modal="true"
      ><UserCard
        :user="selectedUser"
        :onSubmitHandler="handleSubmitUser" /></n-card
  ></n-modal>
</template>

<style scoped>
</style>