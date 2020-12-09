<template>
  <div class="chat">
    <div class="title">
      Chat
    </div>
    <div class="body">
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
      <input type="text" /><button class="send" @click="send">送信</button>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";

export default defineComponent({
  name: "Chat",
  setup() {
    const send = () => {
      window.prompt('ほ');
    };
    return { send };
  }
});
</script>

<style scoped>
.chat {
  width: 100%;
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
  flex-basis: 1.5rem;
}

.body {
  flex-grow: 1;
  overflow-x: auto;
  overflow-y: scroll;
}

.input {
  display: flex;
  flex-direction: row;
  flex-basis: 1.5rem;
}

.input input {
  flex-grow: 1;
}

.input .send {
  float: right;
}
</style>
