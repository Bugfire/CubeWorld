using UnityEngine;

namespace Shared
{
    public class Options : MonoBehaviour
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private Prefabs.MenuButton drawDistance;
        [SerializeField]
        private Prefabs.MenuButton showFPS;
        [SerializeField]
        private Prefabs.MenuButton showEngineStats;
        [SerializeField]
        private Prefabs.MenuButton visibleStrategy;

        #region Unity Lifecycles

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

        #region Unity Events

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
