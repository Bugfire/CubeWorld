<template>
  <div class="license">
    <div>
      <router-link to="/">
        Home
      </router-link>
      <div v-html="license" />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";
import marked from "marked";
// eslint-disable-next-line @typescript-eslint/no-var-requires
const licenseText = require("raw-loader!../assets/License.md");

export default defineComponent({
  name: "License",
  setup() {
    const license = ref(marked(licenseText.default));
    return { license };
  }
});
</script>

<style>
.license {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}
</style>
