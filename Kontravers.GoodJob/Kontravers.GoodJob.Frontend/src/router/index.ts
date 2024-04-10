import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import PersonDetailsPage from '../views/PersonDetailsPage.vue';

const routes: Array<RouteRecordRaw> = [

    {
        path: '/',
        name: 'PersonDetailsPage',
        component: PersonDetailsPage,
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;
