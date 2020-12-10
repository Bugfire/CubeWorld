using UnityEngine;

namespace CommonScene
{
    public class Common : MonoBehaviour
    {
        #region Unity lifecycles

        void Start()
        {
            CubeWorldPlayerPreferences.LoadPreferences();
        }

        #endregion
    }
}
