using UnityEngine;
using System.Runtime.InteropServices;

public class UnityNativeHandler : MonoBehaviour
{
    [SerializeField]
    private Message message;

#if UNITY_WEBGL && !UNITY_EDITOR
    void Start()
    {
        WebGLInput.captureAllKeyboardInput = false;
        Application.targetFrameRate = -1;
    }
#endif

    public void WebToGame(string msgs)
    {
        var m = msgs.Split(new[] { ',' }, 2);
        var tag = m[0];
        var _message = m[1];
        Debug.LogFormat("ReceiveMessage: -> {0}:{1}", tag, _message);
        switch (tag)
        {
            case "setFocus":
                WebGLInput.captureAllKeyboardInput = _message == "1";
                Debug.LogFormat("    captureAllKeyboardInput -> {0}", WebGLInput.captureAllKeyboardInput);
                break;
            case "intputResult":
                message.AddMessage(_message);
                break;
        }
    }

    public static void GameToWeb(string tag, string _message)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        GameToWebNative(tag, _message);
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void GameToWebNative(string tag, string _message);
#endif
}
