<template>
  <div :id="unityContainerId"></div>
</template>

<script lang="ts">
import { defineComponent, onMounted, reactive, PropType } from "vue";
import { GetUnityLoader, UnityInstance } from "./UnityLoaderUtil";

export default defineComponent({
  name: "Unity",
  props: {
    loaderUrl: {
      type: String,
      required: true
    },
    configUrl: {
      type: String,
      required: true
    },
    onLoad: {
      type: Function as PropType<null | ((instance: UnityInstance, canvas: HTMLCanvasElement) => void)>,
      required: false,
      default: null
    },
    onProgressChanged: {
      type: Function as PropType<null | ((progress: number) => void)>,
      required: false,
      default: null
    }
  },
  setup(props) {
    const unityContainerId = `unityContainer-${Math.random()
      .toFixed(10)
      .toString()
      .substr(2)}`;
    const state = reactive<{ hasError: boolean }>({
      hasError: false
    });

    onMounted(async () => {
      try {
        const unityLoader = await GetUnityLoader(props.loaderUrl);
        unityLoader.SystemInfo.mobile = false;
        unityLoader.instantiate(unityContainerId, props.configUrl, {
          onProgress: (instance, progress) => {
            if (props.onProgressChanged !== null) {
              props.onProgressChanged(progress);
            }
            if (progress >= 1 && props.onLoad !== null) {
              const canvas = document.getElementById("#canvas");
              props.onLoad(instance, canvas as HTMLCanvasElement);
            }
          }
        });
      } catch (e) {
        state.hasError = true;
      }
    });

    return { unityContainerId };
  }
});
</script>
