using System;
using UnityEngine;

public class TalkButton : MonoBehaviour
{
    #region Unity User Events

    public void OnClicked()
    {
        Common.NativeHandler.OpenTextInput();
    }

    #endregion
}
