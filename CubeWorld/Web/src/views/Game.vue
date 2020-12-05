<template>
  <div class="game">
    <div v-if="isLoading" class="overlay">
      <Progress :value="unityProgress" />
    </div>
    <Unity
      class="unity"
      ref="unity"
      loaderUrl="unity/UnityLoader.js"
      :configUrl="configUrl"
      style="width: 100%; height: 100%"
      :onLoad="onUnityLoad"
      :onProgressChanged="onUnityProgressChanged"
    />
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, onUnmounted, ref } from "vue";
import Unity from "@/components/Unity/Unity.vue";
import Progress from "@/components/UI/Progress.vue";
import { UnityInstance } from "../components/Unity/UnityLoaderUtil";
import { JSON_FILENAME } from "../settings";

export default defineComponent({
  name: "Game",
  components: {
    Unity,
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
      if (tag === "Input") {
        switch (message) {
          case "text": {
            const inputText = window.prompt("メッセージ");
            if (
              inputText !== null &&
              inputText !== "" &&
              unityInstance !== null
            ) {
              unityInstance.SendMessage(
                "NativeHandler",
                "WebToGame",
                "intputResult,>" + inputText
              );
            }
            return;
          }
        }
      }
      console.log(`Unknown message: tag: ${tag}, message: ${message}`);
    };

    const onResize = () => {
      document.body.style.height = window.innerHeight + "px";
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

    const onUnityLoad = (_unityInstance: UnityInstance) => {
      unityInstance = _unityInstance;
      document?.getElementById("#canvas")?.setAttribute("tabindex", "1");
      unityInstance.SendMessage("NativeHandler", "WebToGame", "setFocus,1");
    };

    return {
      unity,
      onUnityProgressChanged,
      onUnityLoad,
      unityProgress,
      isLoading,
      configUrl: `unity/${JSON_FILENAME}`
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
