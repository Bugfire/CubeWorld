using System;
using UnityEngine;

namespace GameScene
{
    public class TalkButton : MonoBehaviour
    {
        #region Unity User Events

        public void OnClicked()
        {
            CommonScene.NativeHandler.OpenTextInput();
        }

        #endregion
    }
}
