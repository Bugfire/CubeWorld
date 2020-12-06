using System;
using UnityEngine;

namespace GameScene
{
    public class InventryButton : MonoBehaviour
    {
        [SerializeField]
        private GameScene gameScene;
 
        #region Unity lifecycles

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
            gameScene.State = State.INVENTRY;
        }

        #endregion
    }
}
