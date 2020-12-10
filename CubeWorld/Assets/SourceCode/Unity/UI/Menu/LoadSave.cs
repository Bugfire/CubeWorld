using UnityEngine;

namespace Menu
{
    public class LoadSave : MonoBehaviour
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private Activator activator;
        [SerializeField]
        private UnityEngine.UI.Text titleText;
        [SerializeField]
        private Prefabs.MenuButton[] slots;

        #region Unity Lifecycles

        void OnEnable()
        {
            UpdateUI();
        }

        #endregion

        #region Unity Events

        public void OnSlot(int number)
        {
            var isLoad = activator.State == State.LOAD;
            if (isLoad)
            {
                gameManagerUnity.worldManagerUnity.LoadWorld(number - 1);
            }
            else
            {
                gameManagerUnity.worldManagerUnity.SaveWorld(number - 1);
            }
            activator.SetLastState();
        }

        #endregion

        #region Private methods

        private void UpdateUI()
        {
            var isLoad = activator.State == State.LOAD;
            titleText.text = isLoad ? "ロード" : "セーブ";

            for (int i = 0; i < 5; i++)
            {
                System.DateTime fileDateTime = WorldManagerUnity.GetWorldFileInfo(i);

                if (fileDateTime != System.DateTime.MinValue)
                {
                    string prefix;
                    if (isLoad)
                    {
                        prefix = "ロード ";
                    }
                    else
                    {
                        prefix = "上書き ";
                    }
                    slots[i].SetText(prefix + (i + 1).ToString() + "\n[ " + fileDateTime.ToString() + " ]");
                    slots[i].SetActiveFlag(true);
                }
                else
                {
                    slots[i].SetText("-- データなし --");
                    slots[i].SetActiveFlag(!isLoad);
                }
            }
        }

        #endregion
    }
}
