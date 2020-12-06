using UnityEngine;
using System.Runtime.InteropServices;

namespace CommonScene
{
    public class NativeHandler : MonoBehaviour
    {
        #region Unity lifecycles

#if UNITY_WEBGL && !UNITY_EDITOR
        void Start()
        {
            WebGLInput.captureAllKeyboardInput = false;
            Application.targetFrameRate = -1;
            gameToWeb("Kernel", "Initialized");
        }
#endif

        #endregion

        #region Public methods

        public static void OpenTextInput()
        {
            gameToWeb("Input", "メッセージ");
        }

        #endregion

        #region Private methods

        private static void gameToWeb(string tag, string _message)
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            GameToWebNative(tag, _message);
#endif
        }

        #endregion

        #region bindings

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
                    // そのまま、チャットとして送り返す
                    gameToWeb("Send", filterInput(_message));
                    break;
                case "userMessage":
                    Message.AddMessage(filterInput(_message));
                    break;
                case "systemMessage":
                    Message.AddMessage(string.Format("<b>{0}</b>", filterInput(_message)));
                    break;
            }
        }

        private string filterInput(string input)
        {
            return input.Replace('<', '＜').Replace('>', '＞');
        }


#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void GameToWebNative(string tag, string _message);
#endif

        #endregion
    }
}
