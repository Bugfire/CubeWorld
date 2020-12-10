using System;
using UnityEngine;

namespace GameScene
{
    public class InventryButton : MonoBehaviour
    {
        public bool IsOpenInventry
        {
            get
            {
                var t = isOpenInventry;
                isOpenInventry = false;
                return t;
            }
            private set
            {
                isOpenInventry = value;
            }
        }

        private bool isOpenInventry;

        #region Unity lifecycles

        void OnEnable()
        {
            isOpenInventry = false;
        }

        void Update()
        {
            updateInput();
        }

        #endregion

        #region Unity user events

        public void OnClicked()
        {
            openInventry();
        }

        #endregion

        #region Private methods

        private void updateInput()
        {
            if (!Input.GetKeyDown(KeyCode.I))
            {
                return;
            }
            openInventry();
        }

        private void openInventry()
        {
            IsOpenInventry = true;
        }

        #endregion
    }
}
