using UnityEngine;

namespace Shared
{
    public class SettingsMenu : MonoBehaviour
    {
        [SerializeField]
        private GameScene.GameManagerUnity gameManagerUnity;
        [SerializeField]
        private MenuItem drawDistance;
        [SerializeField]
        private MenuItem showFPS;
        [SerializeField]
        private MenuItem showEngineStats;
        [SerializeField]
        private MenuItem visibleStrategy;

        #region Unity lifecycles

        void OnEnable()
        {
            UpdateUI();
        }

        void OnDisable()
        {
            Settings.StorePreferences();
            if (gameManagerUnity)
            {
                gameManagerUnity.PreferencesUpdated();
            }
        }

        #endregion

        #region Unity user events

        public void OnDrawDistance()
        {
            Settings.viewDistance = (Settings.viewDistance + 1) % Settings.farClipPlanes.Length;

            if (gameManagerUnity && gameManagerUnity.playerUnity)
                gameManagerUnity.playerUnity.mainCamera.farClipPlane = Settings.farClipPlanes[Settings.viewDistance];

            UpdateUI();
        }

        public void OnShowFPS()
        {
            Settings.showFPS = !Settings.showFPS;
            UpdateUI();
        }

        public void OnShowEngineStats()
        {
            Settings.showEngineStats = !Settings.showEngineStats;
            UpdateUI();
        }

        public void OnVisibleStrategy()
        {
            if (System.Enum.IsDefined(typeof(GameScene.SectorManagerUnity.VisibleStrategy), (int)Settings.visibleStrategy + 1))
            {
                Settings.visibleStrategy = Settings.visibleStrategy + 1;
            }
            else
            {
                Settings.visibleStrategy = 0;
            }
            UpdateUI();
        }

        #endregion

        #region Private methods

        private void UpdateUI()
        {
            drawDistance.SetText("Draw Distance: " + Settings.farClipPlanes[Settings.viewDistance]);
            showFPS.SetText("Show FPS: " + Settings.showFPS);
            showEngineStats.SetText("Show Engine Stats: " + Settings.showEngineStats);
            visibleStrategy.SetText("Visible Strategy: " + System.Enum.GetName(typeof(GameScene.SectorManagerUnity.VisibleStrategy), Settings.visibleStrategy));
        }

        #endregion
    }
}
