using UnityEngine;
using CubeWorld.Gameplay;
using CubeWorld.Configuration;

namespace Menu
{
    public class Generator : MonoBehaviour
    {
        [SerializeField]
        private GameManagerUnity gameManagerUnity;
        [SerializeField]
        private Activator activator;
        [SerializeField]
        private Prefabs.MenuButton gamePlay;
        [SerializeField]
        private Prefabs.MenuButton worldSize;
        [SerializeField]
        private Prefabs.MenuButton dayLength;
        [SerializeField]
        private Prefabs.MenuButton generator;
        [SerializeField]
        private Prefabs.MenuButton hostMultiplayer;

        private AvailableConfigurations availableConfigurations;

        private int currentGameplayOffset = 0;
        private int currentSizeOffset = 0;
        private int currentGeneratorOffset = 0;
        private int currentDayInfoOffset = 0;
        private bool multiplayer = false;

        #region Unity Lifecycles

        void OnEnable()
        {
            if (availableConfigurations == null)
            {
                availableConfigurations = GameManagerUnity.LoadConfiguration();
                currentDayInfoOffset = 0;
                currentGeneratorOffset = 0;
                currentSizeOffset = 0;
                currentGameplayOffset = 0;
            }
            UpdateUI();
        }

        #endregion

        #region Unity Events

        public void OnGameplay()
        {
            currentGameplayOffset = (currentGameplayOffset + 1) % GameplayFactory.AvailableGameplays.Length;
            UpdateUI();
        }

        public void OnWorldSize()
        {
            currentSizeOffset = (currentSizeOffset + 1) % availableConfigurations.worldSizes.Length;
            UpdateUI();
        }

        public void OnDayLength()
        {
            currentDayInfoOffset = (currentDayInfoOffset + 1) % availableConfigurations.dayInfos.Length;
            UpdateUI();
        }

        public void OnGenerator()
        {
            if (GameplayFactory.AvailableGameplays[currentGameplayOffset].hasCustomGenerator == false)
            {
                currentGeneratorOffset = (currentGeneratorOffset + 1) % availableConfigurations.worldGenerators.Length;
            }
            UpdateUI();
        }

        public void OnHostMultplayer()
        {
            multiplayer = !multiplayer;
            UpdateUI();
        }

        public void OnGenerate()
        {
            gameManagerUnity.Generate(currentDayInfoOffset, currentGeneratorOffset, currentSizeOffset, currentGameplayOffset, multiplayer);
        }

        #endregion

        #region Private methods

        private void UpdateUI()
        {
            gamePlay.SetText("Gameplay: " + GameplayFactory.AvailableGameplays[currentGameplayOffset].name);
            worldSize.SetText("World Size: " + availableConfigurations.worldSizes[currentSizeOffset].name);
            dayLength.SetText("Day Length: " + availableConfigurations.dayInfos[currentDayInfoOffset].name);
            hostMultiplayer.SetText("Host Multiplayer: " + (multiplayer ? "Yes" : "No"));
            if (GameplayFactory.AvailableGameplays[currentGameplayOffset].hasCustomGenerator == false)
            {
                generator.SetText("Generator: " + availableConfigurations.worldGenerators[currentGeneratorOffset].name);
                generator.SetActiveFlag(true);
            }
            else
            {
                generator.SetText("Generator: -");
                generator.SetActiveFlag(false);
            }
        }

        #endregion
    }
}
