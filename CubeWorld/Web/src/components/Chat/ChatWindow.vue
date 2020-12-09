<template>
  <div class="chat">
    <div class="title">
      Chat<button class="toggle" @click="toggleBody">-</button>
    </div>
    <div v-show="isVisible" class="body">
    oops<br>
    oops<br>
    oops<br>
    oops<br>
    oops<br>
    oops<br>
    oops<br>
    oops<br>
    oops<br>
    oops<br>
    aa<br>
    </div>
    <div class="input">
      <input type="text" />
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";

export default defineComponent({
  name: "Chat",
  setup() {
    const isVisible = ref(false);
    const toggleBody = () => {
      isVisible.value = !isVisible.value;
      window.prompt('ほ');
    };
    return { toggleBody, isVisible };
  }
});
</script>

<style scoped>
.chat {
  width: 30%;
  right: 16px;
  top: 16px;
  position: absolute;
  background: #eee;
  display: flex;
  flex-direction: column;
  padding: 4px;
}

.chat div {
  margin: 2px;
}

.title {
  padding: 4px;
  background: #888;
  height: 1.5rem;
}

.toggle {
  float: right;
}

.body {
  max-height: 10rem;
  overflow-x: auto;
  overflow-y: scroll;
}

.input {
  display: flex;
  flex-direction: row;
  height: 1.5rem;
}

.input input {
  flex-grow: 1;
}
</style>
