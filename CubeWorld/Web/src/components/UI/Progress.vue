<template>
  <div class="overlay">
    <div class="bg">
      <div class="bar" :style="`width: ${value * 100}%`"></div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "Progress",
  props: {
    value: {
      type: Number,
      required: true
    }
  },
  setup() {
    return {};
  }
});
</script>

<style scoped>
.overlay {
  width: 100%;
  height: 100%;
  background-color: #000;
}

.bg {
  position: absolute;
  top: 0;
  bottom: 0;
  right: 0;
  left: 0;
  margin: auto;
  width: 80%;
  height: 30px;
  background: #eee;
  vertical-align: center;
}

.bar {
  overflow: no-display;
  position: absolute;
  height: 100%;
  background: #048;
  background-position: 100px 100px;
  transition: all 0.1s ease-out;
}
</style>
