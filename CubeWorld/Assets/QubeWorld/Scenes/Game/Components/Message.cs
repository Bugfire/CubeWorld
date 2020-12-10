using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image backgroundImage;
    [SerializeField]
    private UnityEngine.UI.Text text;

    private int lastVersion;

    #region Unity lifecycles

    void Start()
    {
        lastVersion = Common.Message.Version - 1;
        updateTexts();
    }

    void Update()
    {
        Common.Message.Update(Time.deltaTime);
        updateTexts();
    }

    #endregion

    #region Private methods

    private void updateTexts()
    {
        if (lastVersion == Common.Message.Version)
        {
            return;
        }
        lastVersion = Common.Message.Version;
        var str = Common.Message.GetMessages();
        var isActive = !string.IsNullOrEmpty(str);
        backgroundImage.enabled = isActive;
        text.enabled = isActive;
        text.text = str;
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    #endregion
}
