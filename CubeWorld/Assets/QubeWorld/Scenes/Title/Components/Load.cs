using UnityEngine;

namespace Title
{
    public class Load : MonoBehaviour
    {
        [SerializeField]
        private MenuActivator activator;
        [SerializeField]
        private Prefabs.MenuButton[] slots;

        #region Unity lifecycles

        void OnEnable()
        {
            UpdateUI();
        }

        #endregion

        #region Unity user events

        public void OnSlot(int number)
        {
            Shared.SceneLoader.GoGameWithLoadWorld(number - 1);
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
                    prefix = "ロード ";
                    slots[i].SetText(prefix + (i + 1).ToString() + "\n[ " + fileDateTime.ToString() + " ]");
                    slots[i].SetActiveFlag(true);
                }
                else
                {
                    slots[i].SetText("-- データなし --");
                    slots[i].SetActiveFlag(false);
                }
            }
        }

        #endregion
    }
}
