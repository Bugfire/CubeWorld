using UnityEngine;

namespace Menu
{
    public class Title : MonoBehaviour
    {
        [SerializeField]
        private Activator activator;

        #region Unity Events

        public void OnCreateRandomWorld()
        {
            activator.State = State.GENERATOR;
        }

        public void OnLoadWorld()
        {
            activator.State = State.LOAD;
        }

        public void OnJoinMultiPlayer()
        {
            activator.State = State.JOIN_MULTIPLAYER;
        }

        public void OnOptions()
        {
            activator.State = State.OPTIONS;
        }

        public void OnAbout()
        {
            activator.State = State.ABOUT;
        }

        #endregion
    }
}
