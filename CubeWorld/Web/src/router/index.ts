import { createRouter, createWebHistory, RouteRecordRaw } from "vue-router";
import Home from "../views/Home.vue";
import License from "../views/License.vue";
import Game from "../views/Game.vue";

const routes: Array<RouteRecordRaw> = [
  {
    path: "/",
    name: "Cube World",
    component: Home,
  },
  {
    path: "/license",
    name: "License",
    component: License,
  },
  {
    path: "/game",
    name: "Game",
    component: Game,
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    //component: () =>
    //  import(/* webpackChunkName: "about" */ "../views/About.vue")
  }
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
});

export default router;
