using UnityEngine;

namespace Menu
{
    public class Main : MonoBehaviour
    {
        [SerializeField]
        private Activator activator;

        #region Unity Events

        public void OnCreateRandomWorld()
        {
            activator.SetState(State.GENERATOR);
        }

        public void OnLoadWorld()
        {
            activator.SetState(State.LOAD);
        }

        public void OnJoinMultiPlayer()
        {
            activator.SetState(State.JOIN_MULTIPLAYER);
        }

        public void OnOptions()
        {
            activator.SetState(State.OPTIONS);
        }

        public void OnAbout()
        {
            activator.SetState(State.ABOUT);
        }

        #endregion
    }
}
