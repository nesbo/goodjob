
<template>
    <Card>
        <template #content>
            <div class="flex flex-row gap-4">
                <div class="flex flex-column gap-4 w-4">

                    <div class="flex flex-column gap-2 justify-content-start">
                        <label class="text-left	" for="title">Profile title</label>
                        <InputText id="title" v-model="form.feed.title" aria-describedby="title-help" />
                        <!-- <small id="username-help">Enter your username to reset your password.</small> -->
                    </div>

                    <div class="flex flex-column gap-2 justify-content-start">
                        <label class="text-left	" for="title">Skils (separate with comma)</label>
                        <InputText id="title" v-model="form.feed.skills" aria-describedby="title-help" />
                        <!-- <small id="username-help">Enter your username to reset your password.</small> -->
                    </div>
                </div>

                <div class="flex flex-column gap-2 justify-content-start  w-8">
                    <label class="text-left	" for="title">Description</label>
                    <Textarea id="title" v-model="form.feed.description" aria-describedby="title-help" rows="20" />
                    <!-- <small id="username-help">Enter your username to reset your password.</small> -->
                </div>
            </div>
        </template>
        <template #footer>
            <div class="flex flex-row justify-content-end gap-4">
                <Button label="Secondary" @click="onBack" severity="secondary">back</Button>
                <Button label="Primary" @click="onSaveProfile">save changes</Button>
            </div>
        </template>
    </Card>
</template>

<script lang="ts" setup>
import { ref, inject, onBeforeMount, Ref } from 'vue'
import ProfileService from '../../services/ProfileService';
import type { DynamicDialogInstance } from 'primevue/dynamicdialogoptions'
import { useToast } from 'primevue/usetoast'

const toast = useToast()

const dialogRef = inject<Ref<DynamicDialogInstance>>("dialogRef")
// const dialog = useDialog();
const error = ref<string | null>(null)
// const isEditMode = ref<boolean>(false);
let isEditMode = false;
const form = ref<any>({
    personId: '',
    feed: {
        title: '',
        autoSendEmailEnabled: false,
        autoGenerateProposalsEnabled: false,
        absoluteFeedUrl: '',
        preferredProfileId: 0,
    }
})

const profiles = ref<any[]>([])
const preferredProfileSelectedValue = ref<any>({})

onBeforeMount(() => {
    // console.log(dialogRef?.value.data.feed);
    form.value.personId = dialogRef?.value.data.personId;

    if (dialogRef?.value.data.feed) {
        console.log('AJDE', dialogRef?.value.data);

        isEditMode = true;
        form.value = dialogRef?.value.data;
        profiles.value = dialogRef?.value.data.profiles;

        if (form.value.feed && form.value.feed.preferredProfileId) {
            preferredProfileSelectedValue.value = profiles.value.find((profile: any) => profile.id.toString() === form.value.feed.preferredProfileId.toString());
        }
    }
})

const onSaveProfile = async () => {
    if (isEditMode) {
        await updateProfile();
    } else {
        await addNewProfile();
    }

}

const addNewProfile = async () => {
    console.log('ADD NEW PROFILE', form.value.personId, form.value.feed);
    try {
        await ProfileService.addNewProfile(form.value.personId, form.value.feed);
        toast.add({
            severity: 'success',
            summary: 'New profile added',
            life: 3000,
        });
        dialogRef?.value.close();
    } catch (err) {
        error.value = err as string;
    }
}

const updateProfile = async () => {
    try {
        await ProfileService.updateProfileDetails(form.value.personId, form.value.feed);
        toast.add({
            severity: 'success',
            summary: 'Profile updated',
            life: 3000,
        });
        dialogRef?.value.close();
    } catch (err) {
        error.value = err as string;
    }
}

const onBack = () => {
    dialogRef?.value.close();
}

</script>