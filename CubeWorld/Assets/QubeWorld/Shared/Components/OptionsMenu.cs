using UnityEngine;

namespace Shared
{
    public class OptionsMenu : MonoBehaviour
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
            CubeWorldPlayerPreferences.StorePreferences();
            if (gameManagerUnity)
            {
                gameManagerUnity.PreferencesUpdated();
            }
        }

        #endregion

        #region Unity user events

        public void OnDrawDistance()
        {
            CubeWorldPlayerPreferences.viewDistance = (CubeWorldPlayerPreferences.viewDistance + 1) % CubeWorldPlayerPreferences.farClipPlanes.Length;

            if (gameManagerUnity && gameManagerUnity.playerUnity)
                gameManagerUnity.playerUnity.mainCamera.farClipPlane = CubeWorldPlayerPreferences.farClipPlanes[CubeWorldPlayerPreferences.viewDistance];

            UpdateUI();
        }

        public void OnShowFPS()
        {
            CubeWorldPlayerPreferences.showFPS = !CubeWorldPlayerPreferences.showFPS;
            UpdateUI();
        }

        public void OnShowEngineStats()
        {
            CubeWorldPlayerPreferences.showEngineStats = !CubeWorldPlayerPreferences.showEngineStats;
            UpdateUI();
        }

        public void OnVisibleStrategy()
        {
            if (System.Enum.IsDefined(typeof(SectorManagerUnity.VisibleStrategy), (int)CubeWorldPlayerPreferences.visibleStrategy + 1))
            {
                CubeWorldPlayerPreferences.visibleStrategy = CubeWorldPlayerPreferences.visibleStrategy + 1;
            }
            else
            {
                CubeWorldPlayerPreferences.visibleStrategy = 0;
            }
            UpdateUI();
        }

        #endregion

        #region Private methods

        private void UpdateUI()
        {
            drawDistance.SetText("Draw Distance: " + CubeWorldPlayerPreferences.farClipPlanes[CubeWorldPlayerPreferences.viewDistance]);
            showFPS.SetText("Show FPS: " + CubeWorldPlayerPreferences.showFPS);
            showEngineStats.SetText("Show Engine Stats: " + CubeWorldPlayerPreferences.showEngineStats);
            visibleStrategy.SetText("Visible Strategy: " + System.Enum.GetName(typeof(SectorManagerUnity.VisibleStrategy), CubeWorldPlayerPreferences.visibleStrategy));
        }

        #endregion
    }
}
