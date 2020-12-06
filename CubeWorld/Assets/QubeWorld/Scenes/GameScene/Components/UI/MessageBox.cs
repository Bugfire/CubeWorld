using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class MessageBox : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Image backgroundImage;
        [SerializeField]
        private UnityEngine.UI.Text text;

        private int lastVersion;

        #region Unity lifecycles

        void Start()
        {
            lastVersion = CommonScene.Message.Version - 1;
            updateTexts();
        }

        void Update()
        {
            CommonScene.Message.Update(Time.deltaTime);
            updateTexts();
        }

        #endregion

        #region Private methods

        private void updateTexts()
        {
            if (lastVersion == CommonScene.Message.Version)
            {
                return;
            }
            lastVersion = CommonScene.Message.Version;
            var str = CommonScene.Message.GetMessages();
            var isActive = !string.IsNullOrEmpty(str);
            backgroundImage.enabled = isActive;
            text.enabled = isActive;
            text.text = str;
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        #endregion
    }
}
