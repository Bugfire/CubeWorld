using UnityEngine;

namespace Shared
{
    public class MenuItem : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text text;
        [SerializeField]
        private CanvasGroup canvasGroup;

        #region Public methods

        public void SetText(string _text)
        {
            text.text = _text;
        }

        public void SetActiveFlag(bool isActive)
        {
            canvasGroup.interactable = isActive;
            canvasGroup.alpha = isActive ? 1f : 0.66f;
        }

        #endregion
    }
}
