<script setup lang="ts">
import { ref } from 'vue'



defineProps<{ persons: string[] }>()

const count = ref(0)
</script>

<template>
  <h4>persons list</h4>

  <div class="card">
    <button type="button" @click="count++">count is {{ count }}</button>
  </div>
</template>

<style scoped>
.read-the-docs {
  color: #888;
}
</style>
