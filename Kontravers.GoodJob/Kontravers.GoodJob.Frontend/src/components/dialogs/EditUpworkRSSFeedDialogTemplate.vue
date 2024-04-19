<template>
    <Card>
        <template #content>
            <div class="flex flex-column gap-4">
                <div class="flex flex-column gap-2 justify-content-start">
                    <label class="text-left" for="title">Feed name</label>
                    <InputText id="title" v-model="form.feed.title"
                        aria-describedby="title-help" />
                </div>

                <div class="flex flex-column gap-2">
                    <label class="text-left	">Preffered profile</label>

                    <AutoComplete v-model="preferredProfileSelectedValue"
                        style="width:100%" :input-style="{ width: '100%' }"
                        :completeOnFocus="true" optionLabel="title"
                        @item-select="setSelectedValue"
                        :suggestions="filteredList" @complete="search">

                    </AutoComplete>
                </div>

                <div class="flex flex-column gap-2 justify-content-start">
                    <label class="text-left	" for="title">Filter by search
                        terms</label>
                    <InputText id="title"
                        v-model="form.feed.filters.searchTerms"
                        aria-describedby="title-help" />
                </div>
                <div class="flex flex-column gap-2 justify-content-start">
                    <label class="text-left">
                        Filter search by skill level
                    </label>
                    <div class="flex flex-row gap-4">
                        <label v-for="item in filtersList.expLevel.items">
                            <Checkbox v-model="selectedExpLevel" name="expLevel"
                                :value="item.value" :on-click="filterout()" />
                            {{ item.name }}
                        </label>
                    </div>

                </div>

                <div class="flex flex-column gap-2 justify-content-start">
                    <label class="text-left">
                        Filter search by Locations
                    </label>

                    <MultiSelect v-on:blur="filterout()" class="w-full"
                        v-model="selectedCategories"
                        :options="categoriesList.categories"
                        :showToggleAll="false" filter
                        optionGroupChildren="subcategories"
                        optionGroupLabel="label" optionLabel="label"
                        display="chip" placeholder="Select Cities">
                        <template #optiongroup="slotProps">
                            <div class="flex align-items-center">
                                <div>{{ slotProps.option.name }}</div>
                            </div>
                        </template>
                    </MultiSelect>

                </div>


                <div class="w-full">

                    <label class="text-left	">
                        Filter Fixed price projects
                    </label>
                    <div class="flex flex-row gap-4">
                        <label v-for="item in filtersList.fixedPrice.items">
                            <Checkbox v-model="selectedFixedPriceProjectsRange"
                                name="expLevel" :value="item.value"
                                :on-click="filterout()" />
                            {{ item.name }}
                        </label>
                    </div>

                    <div v-if="selectedFixedPriceProjectsRange.includes('custom')"
                        class="flex gap-2 mt-2">
                        <InputGroup>
                            <InputGroupAddon>
                                <i class="pi pi-dollar"></i>
                            </InputGroupAddon>
                            <InputText placeholder="from"
                                v-model="selectedFixedPriceProjectsCustomRange.from" />
                        </InputGroup>
                        -
                        <InputGroup>
                            <InputGroupAddon>
                                <i class="pi pi-dollar"></i>
                            </InputGroupAddon>
                            <InputText placeholder="to"
                                v-model="selectedFixedPriceProjectsCustomRange.to" />
                        </InputGroup>
                    </div>
                </div>

                <div class="flex flex-row gap-4">
                    <div class="w-full">

                        <label class="text-left	" for="fromHourly">Filter by
                            Hourly
                            rate</label>
                        <div class="flex gap-2">
                            <InputGroup>
                                <InputGroupAddon>
                                    <i class="pi pi-dollar"></i>
                                </InputGroupAddon>
                                <InputText
                                    v-model="selectedHourlyRateRange.from"
                                    placeholder="from" />
                            </InputGroup>
                            -
                            <InputGroup>
                                <InputGroupAddon>
                                    <i class="pi pi-dollar"></i>
                                </InputGroupAddon>
                                <InputText v-model="selectedHourlyRateRange.to"
                                    placeholder="to" />
                            </InputGroup>
                        </div>

                    </div>
                </div>

                <div class="flex flex-column gap-2 justify-content-start">
                    <label class="text-left	" for="username">Filter search by
                        Locations</label>

                    <MultiSelect v-on:blur="filterout()" class="w-full"
                        v-model="selectedCountries" :options="countriesList"
                        :showToggleAll="false" filter
                        optionGroupChildren="items" optionGroupLabel="name"
                        display="chip" placeholder="Select Cities">
                        <template #optiongroup="slotProps">
                            <div class="flex align-items-center">
                                <div>{{ slotProps.option.name }}</div>
                            </div>
                        </template>
                    </MultiSelect>

                </div>
                <div class="flex flex-column gap-2 justify-content-start">
                    <label class="text-left	" for="username">feed url</label>
                    <Textarea id="username" v-model="form.feed.absoluteFeedUrl"
                        rows="10" />
                    <!-- <small id="username-help">Enter your username to reset your password.</small> -->
                </div>

            </div>
        </template>
        <template #footer>
            <div class="flex flex-row justify-content-end gap-4">
                <Button label="Secondary" @click="onBack"
                    severity="secondary">back</Button>
                <Button label="Primary" @click="onSaveRssFeed">save
                    changes</Button>
            </div>
        </template>
    </Card>
</template>

<script lang="ts" setup>
import { ref, inject, onBeforeMount, Ref } from 'vue'
import FeedService from '../../services/FeedService';
import type { DynamicDialogInstance } from 'primevue/dynamicdialogoptions'
import { useToast } from 'primevue/usetoast'
import countriesJson from '../../assets/countriesList.json'
import filtersJson from '../../assets/filtersList.json'
import categoriesJson from '../../assets/categoriesList.json'

const toast = useToast()

const dialogRef = inject<Ref<DynamicDialogInstance>>("dialogRef")
// const dialog = useDialog();
const error = ref<string | null>(null)
let isEditMode = false;
const form = ref<any>({
    personId: '',
    feed: {
        title: '',
        autoSendEmailEnabled: false,
        autoGenerateProposalsEnabled: false,
        preferredProfileId: 0,
        filters: {
            searchTerms: '',
            expLevel: [],
            hourlyRate: {
                from: null,
                to: null
            },
            fixedPriceProjects: [],
            location: []
        },
    }
})

const profiles = ref<any[]>([])
const preferredProfileSelectedValue = ref<any>({})
const countriesList = countriesJson;
const filtersList = filtersJson;
const categoriesList = categoriesJson;
const selectedCountries = ref<any[]>([])
const selectedCategories = ref<any[]>([])
const selectedExpLevel = ref<any[]>([])
const selectedHourlyRateRange = ref<any>({
    from: null,
    to: null
})
const selectedFixedPriceProjectsRange = ref<any>([])
const selectedFixedPriceProjectsCustomRange = {
    from: null,
    to: null
}

const filteredList = ref<any[]>([])
console.log(countriesList);
const search = (event: any) => {
    console.log(event.query);
    filteredList.value = profiles.value.filter((profile: any) => {
        return profile.title.toLowerCase().startsWith(event.query.toLowerCase());
    });
}

// const changeFixedPriceProjectsRange = (val: any, type: string) => {
//     console.log('val', val, 'type', type);
// }

const filterout = () => {
    // console.log('selectedCountries', selectedCountries.value);
    console.log('selectedExpLevel', selectedExpLevel.value);
    // console.log('form', form.value);
}

onBeforeMount(() => {
    populateFormWithData();
    profiles.value = dialogRef?.value.data.profiles;
    form.value.personId = dialogRef?.value.data.personId;
})

const populateFormWithData = () => {

}

const onSaveRssFeed = async () => {
    if (isEditMode) {
        await updateRssFeed();
    } else {
        await createRssFeed();
    }
}

const createRssFeed = async () => {
    try {
        await FeedService.addNewUpworkRssFeed(form.value.feed);
        toast.add({
            severity: 'success',
            summary: 'RSS Feed added',
            life: 3000,
        });
        dialogRef?.value.close();
    } catch (err) {
        error.value = err as string;
    }
}

const updateRssFeed = async () => {
    try {
        await FeedService.updateUpworkRssFeedDetails(form.value.feed);
        toast.add({
            severity: 'success',
            summary: 'RSS Feed updated',
            life: 3000,
        });
        dialogRef?.value.close();
    } catch (err) {
        error.value = err as string;
    }
}

const setSelectedValue = (e: any) => {

    console.log(e.value);
    form.value.feed.preferredProfileId = Number(e.value.id);
}

const onBack = () => {
    dialogRef?.value.close();
}

</script>