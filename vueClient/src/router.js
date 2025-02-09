import { createRouter, createWebHistory } from "vue-router";

import Home from "./components/Home.vue";
import Orders from "./components/Orders.vue";
import Items from "./components/Items.vue";

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
];

const router = createRouter({
  history: createWebHistory(import.meta.env.VITE_BASE_URL),
  routes,
});

export default router;
