import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router';
import PersonsPage from '../views/PersonsPage.vue';
import PersonDetailsPage from '../views/PersonDetailsPage.vue';

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        name: 'PersonsPage',
        component: PersonsPage,
    },
    {
        path: '/persons/:id',
        name: 'PersonDetailsPage',
        component: PersonDetailsPage,
    }
];

const router = createRouter({
    history: createWebHistory(),
    routes,
});

export default router;
