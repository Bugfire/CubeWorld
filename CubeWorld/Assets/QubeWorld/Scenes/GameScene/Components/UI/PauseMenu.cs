using UnityEngine;

namespace GameScene
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private GameScene activator;
        [SerializeField]
        private Shared.MenuItem reCreate;

        #region Unity lifecycles

        void OnEnable()
        {
            UpdateUI();
        }

        #endregion

        #region Unity user events

        public void OnReCreateRandomWorld()
        {
            gameManagerUnity.ReGenerate();
        }

        public void OnSaveWorld()
        {
            activator.State = MenuState.SAVE;
        }

        public void OnOptions()
        {
            activator.State = MenuState.OPTIONS;
        }

        public void OnExitToMainMenu()
        {
            gameManagerUnity.ReturnToTitleMenu();
        }

        public void OnReturnToGame()
        {
            gameManagerUnity.Unpause();
        }

        #endregion

        #region Private methods

        private void UpdateUI()
        {
            if (gameManagerUnity.HasLastConfig())
            {
                reCreate.SetActiveFlag(true);
            }
            else
            {
                reCreate.SetActiveFlag(false);
            }
        }

        #endregion
    }
}
