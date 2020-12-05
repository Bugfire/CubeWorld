// UnityLoader.js loader

export interface UnityInstance {
  SetFullscreen(mode: number): void;
  SendMessage(gameObject: string, method: string, param: string): void;
}

export interface UnityLoader {
  instantiate(
    container: string,
    configUrl: string,
    options: { onProgress: (instance: UnityInstance, progress: number) => void }
  ): UnityInstance;
  SystemInfo: {
    mobile: boolean;
  };
}

declare let UnityLoader: UnityLoader | undefined;

export function GetUnityLoader(url: string): Promise<UnityLoader> {
  return new Promise((resolve, reject) => {
    if (typeof UnityLoader !== "undefined") {
      return resolve(UnityLoader);
    }
    const s = document.createElement("script");
    s.type = "text/javascript";
    s.src = url;
    s.async = true;
    s.defer = true;
    s.onload = () => {
      if (typeof UnityLoader !== "undefined") {
        return resolve(UnityLoader);
      } else {
        return reject(`Load error on UnityLoader (${url})`);
      }
    };
    s.onerror = () => reject(`Load error on UnityLoader (${url})`);
    document.head.appendChild(s);
  });
}
