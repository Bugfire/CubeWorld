using UnityEngine;

namespace CommonScene
{
    public class CommonScene : MonoBehaviour
    {
        #region Unity lifecycles

        void Start()
        {
            Shared.Settings.LoadPreferences();
        }

        #endregion
    }
}
