using UnityEngine;
using System.Runtime.InteropServices;

public class UnityNativeHandler : MonoBehaviour
{
    public void WebToGame(string msgs)
    {
        var m = msgs.Split(new[] { ',' }, 2);
        var tag = m[0];
        var message = m[1];
        Debug.Log("ReceiveMessage: -> " + tag + ":" + message);
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
