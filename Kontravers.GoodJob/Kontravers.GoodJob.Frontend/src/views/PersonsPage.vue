<template>
    <div>
        <RouterView />
        <h1>{{ pageTitle }}</h1>
        <p v-if="isLoading">Loading...</p>
        <DataTable selectionMode="single" v-else-if="!error" :value="persons"
            tableStyle="min-width: 50rem" @rowClick="onRowClick">
            <Column v-for="col of columns" :key="col.field" :field="col.field"
                :header="col.header"></Column>
            <Column key="actions" field="actions" header="Actions">
                <template #body="slotProps">
                    <Button icon="pi pi-pencil" outlined rounded class="mr-2"
                        @click="viewPerson(slotProps.data)" />
                    <Button icon="pi pi-trash" outlined rounded
                        severity="danger" @click="viewPerson(slotProps.data)" />
                </template>
            </Column>
        </DataTable>
        <h2 v-if="error">neradubageri</h2>
        <Button icon="pi pi-plus" rounded @click="goTologin()" label="LOGIN" />
        <Button rounded @click="getKentoTebrex()"
            label="GetTokentooooooooken" />
        <p>{{ kento }}</p>
    </div>
</template>

<script lang="ts" setup>
import { ref, onMounted } from 'vue';
import PersonsService, { Person } from '../services/PersonsService';
import AuthService from '../services/AuthService';
import { useRouter } from 'vue-router';

const router = useRouter();
const navigate = (personId: string) => {
    router.push({ name: 'PersonDetailsPage', params: { id: personId } });
};

const columns = [
    { field: 'id', header: 'id' },
    { field: 'name', header: 'Name' },
    { field: 'email', header: 'Email' },
    { field: 'organisationId', header: 'orgId' },
];

const persons = ref<Person[]>([]);
const error = ref<string | null>(null);
const isLoading = ref<boolean>(false);
const kento = ref<any>('');
const personsData = PersonsService.getPerson();
console.log(personsData);
const pageTitle = ref('Persons');

const viewPerson = (person: any) => {
    console.log(person.id);
};
// const deleteProduct = (person: PersonModel) => {
//     console.log('Delete this person' + person.id);
// };

const onRowClick = (event: any) => {
    console.log(event.data.id);
    navigate(event.data.id);

};

const goTologin = () => {
    window.location.href = 'https://goodjob-api.kontrave.rs/Account/SignIn?returnUrl=https://goodjob.kontrave.rs';
}


const getKentoTebrex = async () => {
    const response = await AuthService.getKento();
    kento.value = response;
}


onMounted(async () => {
    isLoading.value = true;
    try {
        const response = await PersonsService.getPerson();
        persons.value = response as Person[];
        console.log(response as Person[]);
        // ... handle the fetched data
    } catch (err) {
        error.value = err as string;
    } finally {
        isLoading.value = false;
    }
});

</script>

<style scoped>
/* Add your styles here */
</style>
