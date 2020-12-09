<template>
  <div class="game">
    <div v-if="isLoading" class="overlay">
      <Progress :value="unityProgress" />
    </div>
    <Unity
      class="unity"
      ref="unity"
      loaderUrl="unity/UnityLoader.js"
      configUrl="unity/BuildWebGL.json"
      style="width: 100%; height: 100%"
      :onLoad="onUnityLoad"
      :onProgressChanged="onUnityProgressChanged"
    />
    <ChatWindow>
    </ChatWindow>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, onUnmounted, ref } from "vue";
import Unity from "@/components/Unity/Unity.vue";
import ChatWindow from "@/components/Chat/ChatWindow.vue";
import Progress from "@/components/UI/Progress.vue";
import { UnityInstance } from "../components/Unity/UnityLoaderUtil";

export default defineComponent({
  name: "Game",
  components: {
    Unity,
    ChatWindow,
    Progress
  },
  setup() {
    const unity = ref<typeof Unity | null>(null);
    const isLoading = ref(true);
    const unityProgress = ref(0);
    let unityInstance: UnityInstance | null = null;

    const unityMessageHandler = (tag: string, message: string) => {
      if (tag === "Kernel") {
        switch (message) {
          case "Initialized":
            isLoading.value = false;
            return;
        }
      }
      console.log(`Unknown message: tag: ${tag}, message: ${message}`);
    };

    const onResize = () => {
      document.body.style.height = window.innerHeight + "px";
    };

    let lastFocus = false;
    const onClick = (e: MouseEvent) => {
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      const newFocus = (e as any).target.id === "#canvas";
      if (lastFocus === newFocus) {
        return;
      }
      lastFocus = newFocus;
      unityInstance?.SendMessage(
        "NativeHandler",
        "WebToGame",
        `setFocus,${newFocus ? "1" : "0"}`
      );
    };

    // 行儀が悪いが全画面のために外部の要素も変更する
    const externalElements = () => {
      const elems: HTMLElement[] = [];
      elems.push(document.body);
      elems.push(document.getElementById("app") as HTMLElement);
      return elems;
    };

    onMounted(() => {
      window.addEventListener("resize", onResize);
      window.addEventListener("click", onClick);
      externalElements().forEach(elem => {
        elem.style.overflow = "hidden";
        elem.style.height = "100%";
      });
      onResize();
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      (window as any).UnityGameToWebHandler = unityMessageHandler;
    });

    onUnmounted(() => {
      window.removeEventListener("resize", onResize);
      window.removeEventListener("click", onResize);
      externalElements().forEach(elem => {
        elem.style.overflow = "";
        elem.style.height = "";
      });
      // eslint-disable-next-line @typescript-eslint/no-explicit-any
      delete (window as any).UnityGameToWebHandler;
    });

    const onUnityProgressChanged = (progress: number) => {
      unityProgress.value = progress;
    };

    const onUnityLoad = (
      _unityInstance: UnityInstance,
      canvas: HTMLCanvasElement
    ) => {
      unityInstance = _unityInstance;
      canvas.setAttribute("tabindex", "1");
    };

    return {
      unity,
      onUnityProgressChanged,
      onUnityLoad,
      unityProgress,
      isLoading
    };
  }
});
</script>

<style scoped>
.game {
  height: 100%;
}

.overlay {
  z-index: 1;
  position: absolute;
  width: 100%;
  height: 100%;
  top: 50%;
  left: 50%;
  transform: translate(-50%, -50%);
  -webkit-transform: translate(-50%, -50%);
  -ms-transform: translate(-50%, -50%);
}
</style>
