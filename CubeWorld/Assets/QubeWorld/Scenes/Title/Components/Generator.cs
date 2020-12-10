using UnityEngine;
using CubeWorld.Gameplay;
using CubeWorld.Configuration;

namespace Title
{
    public class Generator : MonoBehaviour
    {
        [SerializeField]
        private MenuActivator activator;
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

        private Shared.GenerateArgs generateArgs = new Shared.GenerateArgs();

        #region Unity lifecycles

        void OnEnable()
        {
            if (availableConfigurations == null)
            {
                availableConfigurations = GameManagerUnity.LoadConfiguration();
                generateArgs.Reset();
            }
            UpdateUI();
        }

        #endregion

        #region Unity user events

        public void OnGameplay()
        {
            generateArgs.GameplayOffset = (generateArgs.GameplayOffset + 1) % GameplayFactory.AvailableGameplays.Length;
            UpdateUI();
        }

        public void OnWorldSize()
        {
            generateArgs.SizeOffset = (generateArgs.SizeOffset + 1) % availableConfigurations.worldSizes.Length;
            UpdateUI();
        }

        public void OnDayLength()
        {
            generateArgs.DayInfoOffset = (generateArgs.DayInfoOffset + 1) % availableConfigurations.dayInfos.Length;
            UpdateUI();
        }

        public void OnGenerator()
        {
            if (GameplayFactory.AvailableGameplays[generateArgs.GameplayOffset].hasCustomGenerator == false)
            {
                generateArgs.GeneratorOffset = (generateArgs.GeneratorOffset + 1) % availableConfigurations.worldGenerators.Length;
            }
            UpdateUI();
        }

        public void OnHostMultplayer()
        {
            generateArgs.Multiplayer = !generateArgs.Multiplayer;
            UpdateUI();
        }

        public void OnGenerate()
        {
            Shared.SceneLoader.GoGameWithGenerate(ref generateArgs);
        }

        #endregion

        #region Private methods

        private void UpdateUI()
        {
            gamePlay.SetText("Gameplay: " + GameplayFactory.AvailableGameplays[generateArgs.GameplayOffset].name);
            worldSize.SetText("World Size: " + availableConfigurations.worldSizes[generateArgs.SizeOffset].name);
            dayLength.SetText("Day Length: " + availableConfigurations.dayInfos[generateArgs.DayInfoOffset].name);
            hostMultiplayer.SetText("Host Multiplayer: " + (generateArgs.Multiplayer ? "Yes" : "No"));
            if (GameplayFactory.AvailableGameplays[generateArgs.GameplayOffset].hasCustomGenerator == false)
            {
                generator.SetText("Generator: " + availableConfigurations.worldGenerators[generateArgs.GeneratorOffset].name);
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
