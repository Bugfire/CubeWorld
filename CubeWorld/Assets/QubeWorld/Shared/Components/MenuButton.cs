using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prefabs
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text text;
        [SerializeField]
        private CanvasGroup canvasGroup;

        public void SetText(string _text)
        {
            text.text = _text;
        }

        public void SetActiveFlag(bool isActive)
        {
            canvasGroup.interactable = isActive;
            canvasGroup.alpha = isActive ? 1f : 0.66f;
        }
    }
}
