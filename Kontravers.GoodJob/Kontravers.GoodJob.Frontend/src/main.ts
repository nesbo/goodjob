import { createApp } from 'vue'
import './style.css'
import App from './App.vue'
import { createPinia } from 'pinia'
import PrimeVue from 'primevue/config'
import DataTable from 'primevue/datatable'
import Column from 'primevue/column'
import ContextMenu from 'primevue/contextmenu'
import Card from 'primevue/card'
import InputText from 'primevue/inputtext'
import InputSwitch from 'primevue/inputswitch'
import Textarea from 'primevue/textarea'
import Button from 'primevue/button'
import router from './router'
import Chip from 'primevue/chip'
import Toast from 'primevue/toast';
import AutoComplete from 'primevue/autocomplete';
import MultiSelect from 'primevue/multiselect';
import InputGroup from 'primevue/inputgroup';
import InputGroupAddon from 'primevue/inputgroupaddon';
import Checkbox from 'primevue/checkbox';


import DynamicDialog from 'primevue/dynamicdialog';

import ToastService from 'primevue/toastservice';
import DialogService from 'primevue/dialogservice';

import "primeflex/primeflex.css"
import 'primeicons/primeicons.css'

const pinia = createPinia()
const app = createApp(App)

app.use(pinia)
app.use(PrimeVue)
app.use(router)
app.use(ToastService)
app.use(DialogService)

app.component('DataTable', DataTable)
app.component('DynamicDialog', DynamicDialog)
app.component('Toast', Toast)
app.component('Column', Column)
app.component('ContextMenu', ContextMenu)
app.component('Card', Card)
app.component('InputText', InputText)
app.component('InputSwitch', InputSwitch)
app.component('Textarea', Textarea)
app.component('Button', Button)
app.component('Chip', Chip)
app.component('AutoComplete', AutoComplete)
app.component('MultiSelect', MultiSelect)
app.component('InputGroup', InputGroup)
app.component('InputGroupAddon', InputGroupAddon)
app.component('Checkbox', Checkbox)


app.mount('#app')
