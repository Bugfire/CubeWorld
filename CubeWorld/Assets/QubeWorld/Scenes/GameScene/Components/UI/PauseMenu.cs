using UnityEngine;

namespace GameScene
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private GameScene gameScene;
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
            gameScene.State = State.SAVE;
        }

        public void OnSettings()
        {
            gameScene.State = State.SETTINGS;
        }

        public void OnExitToMainMenu()
        {
            gameScene.ReturnToTitleMenu();
        }

        public void OnReturnToGame()
        {
            gameScene.Unpause();
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
