using System;
using UnityEngine;

namespace GameScene
{
    public class MenuButton : MonoBehaviour
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
            openPauseMenu();
        }

        #endregion

        #region Private methods

        private void openPauseMenu()
        {
            gameScene.Pause();
        }

        private void updateInput()
        {
            if (!Input.GetKeyDown(KeyCode.Escape))
            {
                return;
            }
            if (gameScene.State == State.GAME)
            {
                openPauseMenu();
            }
            else
            {
                gameScene.SetLastState();
            }
        }

        #endregion
    }
}
