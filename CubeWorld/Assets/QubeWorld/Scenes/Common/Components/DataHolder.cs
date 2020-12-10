using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common
{
    public class DataHolder : MonoBehaviour
    {
        #region Unity lifecycles

        void Start()
        {
            CubeWorldPlayerPreferences.LoadPreferences();
        }

        #endregion
    }
}
