using UnityEngine;

namespace GameScene
{
    public class SaveMenu : MonoBehaviour
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private Activator activator;
        [SerializeField]
        private Shared.MenuItem[] slots;

        #region Unity lifecycles

        void OnEnable()
        {
            UpdateUI();
        }

        #endregion

        #region Unity events

        public void OnSlot(int number)
        {
            gameManagerUnity.worldManagerUnity.SaveWorld(number - 1);
            activator.SetLastState();
        }

        #endregion

        #region Private methods

        private void UpdateUI()
        {
            for (int i = 0; i < 5; i++)
            {
                System.DateTime fileDateTime = WorldManagerUnity.GetWorldFileInfo(i);

                if (fileDateTime != System.DateTime.MinValue)
                {
                    string prefix;
                    prefix = "上書き ";
                    slots[i].SetText(prefix + (i + 1).ToString() + "\n[ " + fileDateTime.ToString() + " ]");
                }
                else
                {
                    slots[i].SetText("-- データなし --");
                }
                slots[i].SetActiveFlag(true);
            }
        }

        #endregion
    }
}
