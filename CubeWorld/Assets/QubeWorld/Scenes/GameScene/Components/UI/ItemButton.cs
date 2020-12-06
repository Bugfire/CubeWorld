using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class ItemButton : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text text;
        [SerializeField]
        private UnityEngine.UI.Text number;
        [SerializeField]
        private UnityEngine.UI.Image image;

        private int index;
        private Inventry inventry;

        #region Unity user events

        public void OnClick()
        {
            inventry.OnItemClicked(index);
        }

        #endregion

        #region Public methods

        public void Setup(Inventry inventry, int index, string text, int number, Sprite sprite)
        {
            this.inventry = inventry;
            this.index = index;
            this.text.text = text;
            this.number.text = number.ToString();
            this.image.sprite = sprite;
        }

        #endregion
    }
}
