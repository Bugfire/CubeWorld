<template>
  <div class="overlay">
    <div class="bg">
      <div class="bar" :style="`width: ${value * 100}%`"></div>
      <div class="text">
        ロード中です、しばしお待ちください。
      </div>
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
}

.bg {
  position: absolute;
  top: 25%;
  bottom: 0;
  right: 0;
  left: 0;
  margin: auto;
  width: 80%;
  height: 30px;
  background-color: #fff;
}

.bar {
  position: absolute;
  height: 100%;
  background-color: #068;
  background-position: 100px 100px;
  transition: all 0.1s ease-out;
}

.text {
  position: absolute;
  top: 25%;
  bottom: 0;
  right: 0;
  left: 0;
  margin: auto;
  height: 100%;
  color: #fff;
  text-align: center;
  vertical-align: middle;
}
</style>
