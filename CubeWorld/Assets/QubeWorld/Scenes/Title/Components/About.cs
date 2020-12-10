using UnityEngine;

namespace Title
{
    public class About : MonoBehaviour
    {
        [SerializeField]
        private UnityEngine.UI.Text text;
        [SerializeField]
        private TextAsset license;

        private int counter;

        #region Unity lifecycles

        void OnEnable()
        {
            counter = 0;
        }

        void OnDisable()
        {
            text.text = "";
        }

        void Update()
        {
            counter++;
            if (counter == 1)
            {
                // 表示前にテキストを設定してもレイアウトが走らないので崩れる。なんでや。
                text.text = license.text;
            }
        }

        #endregion
    }
}
