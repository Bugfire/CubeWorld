using UnityEngine;

namespace Menu
{
    public class Pause : MonoBehaviour
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private Activator activator;
        [SerializeField]
        private Prefabs.MenuButton reCreate;

        #region Unity Lifecycles

        void OnEnable()
        {
            UpdateUI();
        }

        #endregion

        #region Unity Events

        public void OnReCreateRandomWorld()
        {
            gameManagerUnity.ReGenerate();
        }

        public void OnSaveWorld()
        {
            activator.State = Menu.State.SAVE;
        }

        public void OnOptions()
        {
            activator.State = Menu.State.OPTIONS;
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
