<script setup lang="ts">
import { onBeforeMount } from 'vue';
// const kento = null;
onBeforeMount(() => {
  // if (kento === null) {
  //   window.location.href = 'https://goodjob-auth.kontrave.rs/Identity/Account/Login?ReturnUrl=http://localhost:5173';
  // }
  console.log('kento is null');
  console.log('At this point, events and lifecycle have been initialized.')
})

console.log(import.meta.env.VITE_BASE_URL);
</script>

<template>
  <img src="./assets/goodjob.gif" class="mb-4" />
  <RouterView />
  <Toast />
  <DynamicDialog />
</template>

<style>
img {
  width: 200px;
}
</style>
