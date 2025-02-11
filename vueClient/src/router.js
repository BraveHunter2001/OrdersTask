import { createRouter, createWebHistory } from "vue-router";

import Home from "./components/Home.vue";
import Orders from "./components/Orders.vue";
import Items from "./components/Items.vue";
import Users from "./components/Users.vue";

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
