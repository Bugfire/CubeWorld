using System;
using UnityEngine;

namespace GameScene
{
    public class TalkButton : MonoBehaviour
    {
        #region Unity user events

        public void OnClicked()
        {
            CommonScene.NativeHandler.OpenTextInput();
        }

        #endregion
    }
}
