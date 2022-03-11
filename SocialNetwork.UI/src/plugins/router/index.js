import Vue from 'vue'
import VueRouter from 'vue-router';
Vue.use(VueRouter);

import Login from '@/pages/Login.vue';
import Register from '@/pages/Register.vue';
import Home from '@/pages/Home.vue';

const routes = [
    {
        path: '/login',
        name: 'login',
        component: Login,
    },
    {
        path: '/register',
        name: 'register',
        component: Register,
    },
    {
        path: '/home',
        name: 'home',
        component: Home,
    },
]
const router = new VueRouter({
    mode: 'history',
    routes
});
export default router