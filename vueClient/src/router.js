import { createRouter, createWebHistory } from "vue-router";
import Orders from "./components/OrderComponents/Orders.vue";
import Home from "./components/Home.vue";
import Items from "./components/ItemComponents/Items.vue";
import Users from "./components/UserComponents/Users.vue";

const routes = [
  {
    path: "/",
    name: "home",
    component: Home,
  },
  {
    path: "/orders",
    name: "orders",
    component: Orders,
  },
  {
    path: "/items",
    name: "items",
    component: Items,
  },
  {
    path: "/users",
    name: "users",
    component: Users,
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.VITE_BASE_URL),
  routes,
});

export default router;
