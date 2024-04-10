<template>
    <div>

        <Card class="mb-4">
            <template #content class="text-left">
                <span class="text-left">
                    <h2 class="text-left">
                        {{ personDetails.name }}
                        <Chip class="ml-2"> {{ personDetails.email }}</Chip>

                    </h2>
                </span>
            </template>
        </Card>

        <div class="flex flex-row gap-4">

            <Card class="w-6">
                <template #content>
                    <div class="flex gap-4 align-items-center">

                        <h3 class="text-left">Feeds list</h3>
                        <Button icon="pi pi-plus" rounded
                            @click="onAddNewFeed()" />
                    </div>
                    <p v-if="isLoading">Loading...</p>


                    <DataTable selectionMode="single" v-else
                        :value="personDetails.upworkRssFeeds"
                        @rowClick="onFeedRowClick">
                        <Column v-for="col of columns" :key="col.field"
                            :field="col.field" :header="col.header"></Column>
                        <Column key="actions" field="actions" header="Actions">
                            <!-- <template #body="slotProps">
                                <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="onFeedRowClick(slotProps.data)" />
                                <Button icon="pi pi-trash" outlined rounded severity="danger" @click="onFeedRowClick(slotProps.data)" />
                            </template> -->
                        </Column>
                    </DataTable>
                </template>
            </Card>

            <Card class="w-6">
                <template #content>
                    <div class="flex gap-4 align-items-center">

                        <h3 class="text-left">Profiles list</h3>
                        <Button icon="pi pi-plus" rounded
                            @click="onAddNewProfile()" />
                    </div>
                    <p v-if="isLoading">Loading...</p>
                    <!-- <p v-if="personDetails.profiles.length === 0">Loading...</p> -->

                    <DataTable selectionMode="single" v-else
                        :value="personDetails.profiles"
                        @rowClick="onProfileRowClick">
                        <Column v-for="col of columns" :key="col.field"
                            :field="col.field" :header="col.header"></Column>
                        <Column key="actions" field="actions" header="Actions">

                            <!-- <template #body="slotProps">
                                <Button icon="pi pi-pencil" outlined rounded class="mr-2" @click="onProfileRowClick(slotProps.data)" />
                                <Button icon="pi pi-trash" outlined rounded severity="danger" @click="onProfileRowClick(slotProps.data)" />
                            </template> -->
                        </Column>
                        <template #empty>
                            No records found
                        </template>
                    </DataTable>
                </template>
            </Card>
        </div>

    </div>
</template>

<script lang="ts" setup>
import { ref, onMounted, defineAsyncComponent } from 'vue';
import { useDialog } from 'primevue/usedialog';
import PersonsService, { PersonDetails } from '../services/PersonsService';
import FeedService, { UpworkRssFeedDetails } from '../services/FeedService';
import ProfileService, { ProfileDetails } from '../services/ProfileService';
import { useRoute } from 'vue-router';

const EditUpworkRSSFeedDialogTemplate = defineAsyncComponent(() => import('../components/dialogs/EditUpworkRSSFeedDialogTemplate.vue'));
const EditProfileDialogTemplate = defineAsyncComponent(() => import('../components/dialogs/EditProfileDialogTemplate.vue'));
let personId = '';
const dialog = useDialog();
// const toast = useToast();


const columns = [
    { field: 'id', header: 'ID' },
    { field: 'title', header: 'Name' },

];

const personDetails = ref<PersonDetails>({
    id: '',
    name: '',
    email: '',
    organisationId: '',
    upworkRssFeeds: [],
    profiles: []
});
const error = ref<string | null>(null);
const isLoading = ref<boolean>(false);
const feedDetails = ref<any>({});
const profileDetails = ref<any>({});

const onFeedRowClick = async (event: any) => {

    try {
        const response = await FeedService.getUpworkRssFeedDetails(personDetails.value.id as string, event.data.id as string);
        console.log('Pre dialoga', response);

        dialog.open(EditUpworkRSSFeedDialogTemplate, {
            props: {
                header: 'Edit Upwork RSS Feed',
                style: {
                    width: '80%',
                },
                modal: true,
            },
            data: {
                personId: personDetails.value.id,
                feed: response,
                profiles: personDetails.value.profiles,
            },

            onClose: () => {
                getPerson();
            },
        });
        feedDetails.value = response as UpworkRssFeedDetails;
        console.log('Response FEEDA ZAM ODAL BRATOOOo', dialog);
        // ... handle the fetched data
    } catch (err) {
        error.value = err as string;
    } finally {
        isLoading.value = false;
    }
    // navigate(personId as string, event.data.id);

};

const onProfileRowClick = async (event: any) => {

    try {
        const response = await ProfileService.getProfileDetails(personDetails.value.id as string, event.data.id as string);
        console.log('Pre dialoga', response);

        dialog.open(
            EditProfileDialogTemplate, {
            props: {
                header: 'Edit profile',
                style: {
                    minWidth: '1200px',
                    width: '80%',
                },
                modal: true,
            },
            data: {
                personId: personDetails.value.id,
                feed: response,
                profiles: personDetails.value.profiles,
            },

            onClose: () => {
                getPerson();
            },
        });
        profileDetails.value = response as ProfileDetails;
        console.log('Response FEEDA ZAM ODAL BRATOOOo', dialog);
        // ... handle the fetched data
    } catch (err) {
        error.value = err as string;
    } finally {
        isLoading.value = false;
    }
    // navigate(personId as string, event.data.id);

};

const onAddNewFeed = async () => {


    dialog.open(
        EditUpworkRSSFeedDialogTemplate, {
        props: {
            header: 'Add new Feed',
            style: {
                minWidth: '800px',
                width: '80%',
            },
            modal: true,
        },
        data: {
            personId: personId,
            profiles: personDetails.value.profiles,

        },

        onClose: () => {
            getPerson();
        },
    });
    // ... handle the fetched data

};


const onAddNewProfile = async () => {

    dialog.open(
        EditProfileDialogTemplate, {
        props: {
            header: 'Add new profile',
            style: {
                minWidth: '1200px',
                width: '80%',
            },
            modal: true,
        },
        data: {
            personId: personId,
        },

        onClose: () => {
            getPerson();
        },
    });
    // ... handle the fetched data

};

// const navigate = (personId: string, feedId: string) => {
//     router.push({ name: 'RSSFeedPage', params: { id: personId, feedId: feedId } });
// };

onMounted(async () => {
    personId = useRoute().params.id as string;

    console.log('This is person ID', personId);
    await getPerson();
});

isLoading.value = true;

const getPerson = async () => {
    isLoading.value = true;
    // console.log('This is person ID', personId)
    try {
        const response = await PersonsService.getPerson();
        personDetails.value = response as PersonDetails;
        console.log(response as PersonDetails);
        // ... handle the fetched data
    } catch (err) {
        error.value = err as string;
    } finally {
        isLoading.value = false;
    }
};

</script>

<style scoped>
/* Add your styles here */
</style>
