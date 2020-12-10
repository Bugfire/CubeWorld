using UnityEngine;

namespace TitleScene
{
    public class LoadMenu : MonoBehaviour
    {
        [SerializeField]
        private Shared.MenuItem[] slots;

        #region Unity lifecycles

        void OnEnable()
        {
            UpdateUI();
        }

        #endregion

        #region Unity user events

        public void OnSlot(int _number)
        {
            var args = new Shared.GameLaunchArgsLoadWorld() { number = _number - 1 };
            Shared.SceneLoader.GoToGameScene(args);
        }

        #endregion

        #region Private methods

        private void UpdateUI()
        {
            for (int i = 0; i < 5; i++)
            {
                var fileDateTime = Shared.WorldFileIO.GetInfo(i);

                if (fileDateTime != System.DateTime.MinValue)
                {
                    slots[i].SetText(string.Format("ロード {0}\n[ {1} ]", i + 1, fileDateTime));
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
