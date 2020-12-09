using UnityEngine;
using System.Runtime.InteropServices;

public class UnityNativeHandler : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    void Start()
    {
        WebGLInput.captureAllKeyboardInput = false;
    }
#endif

    public void WebToGame(string msgs)
    {
        var m = msgs.Split(new[] { ',' }, 2);
        var tag = m[0];
        var message = m[1];
        Debug.Log("ReceiveMessage: -> " + tag + ":" + message);
        switch (tag)
        {
            case "setFocus":
                WebGLInput.captureAllKeyboardInput = message == "1";
                break;
        }
    }

    public void GameToWeb(string tag, string message)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GameToWebNative(tag, message);
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void GameToWebNative(string tag, string message);
#endif
}
