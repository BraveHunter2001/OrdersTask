import { createRouter, createWebHistory } from "vue-router";

import Home from "./components/Home.vue";

const routes = [
  {
    path: "/",
    name: "home",
    component: Home,
  },
];

const router = createRouter({
  history: createWebHistory(import.meta.env.VITE_BASE_URL),
  routes,
});

export default router;
