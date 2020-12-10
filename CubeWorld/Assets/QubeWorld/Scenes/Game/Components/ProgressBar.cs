using UnityEngine;

namespace GameScene
{
    public class ProgressBar : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text text;
        [SerializeField]
        private UnityEngine.UI.Slider slider;

#region public

        public void SetVisible(bool enable)
        {
            gameObject.SetActive(enable);
        }

        public void SetText(string _text)
        {
            text.text = _text;
        }

        public void SetProgress(float value)
        {
            slider.value = value;
        }

#endregion
    }
}
