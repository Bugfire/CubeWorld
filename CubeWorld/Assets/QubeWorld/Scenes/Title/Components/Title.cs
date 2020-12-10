using UnityEngine;

namespace Title
{
    public class Title : MonoBehaviour
    {
        [SerializeField]
        private MenuActivator activator;

        #region Unity user events

        public void OnCreateRandomWorld()
        {
            activator.State = MenuState.GENERATOR;
        }

        public void OnLoadWorld()
        {
            activator.State = MenuState.LOAD;
        }

        public void OnJoinMultiPlayer()
        {
            activator.State = MenuState.JOIN_MULTIPLAYER;
        }

        public void OnOptions()
        {
            activator.State = MenuState.OPTIONS;
        }

        public void OnAbout()
        {
            activator.State = MenuState.ABOUT;
        }

        #endregion
    }
}
