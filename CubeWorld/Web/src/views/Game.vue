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
import { JSON_FILENAME } from "../unity_settings";
import { PhotonClient } from "../lib/photon/PhotonClient";
import { PHOTON_APP_ID } from "../photon_settings";

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
    let photonEngine: PhotonClient | null = null;
    const PHOTON_APP_VER = "0.1";

    const onSystemMessage = (msg: string) => {
      if (!unityInstance) {
        return;
      }
      unityInstance.SendMessage(
        "NativeHandler",
        "WebToGame",
        "systemMessage," + msg
      );
    };
    const onUserMessage = (msg: string) => {
      if (!unityInstance) {
        return;
      }
      unityInstance.SendMessage(
        "NativeHandler",
        "WebToGame",
        "userMessage," + msg
      );
    };
    const unityMessageHandler = (tag: string, message: string) => {
      switch (tag) {
        case "Kernel":
          {
            const isHttps = window.location.protocol === "https:";
            switch (message) {
              case "Initialized":
                isLoading.value = false;
                if (PHOTON_APP_ID) {
                  photonEngine = new PhotonClient(
                    isHttps,
                    PHOTON_APP_ID,
                    PHOTON_APP_VER,
                    onSystemMessage,
                    onUserMessage
                  );
                  photonEngine.Connect();
                }
                return;
            }
          }
          break;
        case "Input":
          {
            const inputText = window.prompt(message);
            if (
              inputText !== null &&
              inputText !== "" &&
              unityInstance !== null
            ) {
              unityInstance.SendMessage(
                "NativeHandler",
                "WebToGame",
                "intputResult," + inputText
              );
            }
          }
          break;
        case "Send":
          if (photonEngine) {
            photonEngine.SendMessage(message);
          } else {
            onSystemMessage(`未接続です[${message}]`);
          }
          break;
        default:
          console.log(`Unknown message: tag: ${tag}, message: ${message}`);
      }
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
      if (photonEngine) {
        photonEngine.LeaveRoom();
        photonEngine = null;
      }
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
