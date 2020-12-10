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

        #region Unity user events

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
                System.DateTime fileDateTime = Shared.WorldFileIO.GetInfo(i);

                if (fileDateTime != System.DateTime.MinValue)
                {
                    slots[i].SetText(string.Format("上書き {0}\n[ {1} ]", i + 1, fileDateTime.ToString()));
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
