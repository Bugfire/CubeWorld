using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Message : MonoBehaviour
{
    [SerializeField]
    private UnityEngine.UI.Image backgroundImage;
    [SerializeField]
    private UnityEngine.UI.Text text;

    private struct MessageText
    {
        public float timeDelta;
        public string message;
    }

    private const float TIMEOUT_MESSAGE = 30f;

    private List<MessageText> messages = new List<MessageText>(100);
    private bool isDirty;

    #region Unity lifecycles

    void Start()
    {
        isDirty = true;
        messages.Clear();
        updateTexts();
    }

    void Update()
    {
        updateDeltaTime(Time.deltaTime);
        updateTexts();
    }

    #endregion

    #region Public methods

    public void AddMessage(string _message)
    {
        messages.Add(new MessageText() { timeDelta = 0f, message = _message });
        isDirty = true;
    }

    #endregion

    #region Private methods

    private void updateDeltaTime(float deltaTime)
    {
        var lastRemoved = -1;
        for (var i = 0; i < messages.Count; i++)
        {
            var newDeltaTime = messages[i].timeDelta + deltaTime;
            messages[i] = new MessageText() { timeDelta = newDeltaTime, message = messages[i].message };
            if (newDeltaTime > TIMEOUT_MESSAGE)
            {
                lastRemoved = i;
            }
        }
        if (lastRemoved >= 0)
        {
            messages.RemoveRange(0, lastRemoved + 1);
            isDirty = true;
        }
    }

    private void updateTexts()
    {
        if (!isDirty)
        {
            return;
        }

        var stringBuilder = new System.Text.StringBuilder(1024);
        for (var i = 0; i < messages.Count; i++)
        {
            stringBuilder.Append(messages[i].message);
            if (i != messages.Count - 1)
            {
                stringBuilder.Append('\n');
            }
        }
        var str = stringBuilder.ToString();
        var isActive = !string.IsNullOrEmpty(str);
        backgroundImage.enabled = isActive;
        text.enabled = isActive;
        text.text = str;
        isDirty = false;
        UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
    }

    #endregion
}
