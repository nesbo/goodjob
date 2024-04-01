<script setup lang="ts">
// import PersonsPage from './views/PersonsPage.vue'

</script>

<template>
  <img src="./assets/goodjob.gif" class="mb-4" />
  <RouterView />
  <Toast />
  <DynamicDialog />
  <PersonsPage />
</template>

<style>
img {
  width: 200px;
}
</style>
