using System;
using UnityEngine;

namespace GameScene
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private Activator activator;

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
            gameManagerUnity.Pause();
        }

        private void updateInput()
        {
            if (!Input.GetKeyDown(KeyCode.Escape))
            {
                return;
            }
            if (gameManagerUnity.GetState() == GameState.GAME)
            {
                openPauseMenu();
            }
            else
            {
                activator.SetLastState();
            }
        }

        #endregion
    }
}
