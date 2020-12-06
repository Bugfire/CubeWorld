using UnityEngine;
using CubeWorld.Gameplay;
using CubeWorld.Configuration;

namespace TitleScene
{
    public class GeneratorMenu : MonoBehaviour
    {
        [SerializeField]
        private Shared.MenuItem gamePlay;
        [SerializeField]
        private Shared.MenuItem worldSize;
        [SerializeField]
        private Shared.MenuItem dayLength;
        [SerializeField]
        private Shared.MenuItem generator;
        [SerializeField]
        private Shared.MenuItem hostMultiplayer;

        private AvailableConfigurations availableConfigurations;
        private Shared.RandomWorldGeneratorArgs generateArgs = new Shared.RandomWorldGeneratorArgs();

        #region Unity lifecycles

        void OnEnable()
        {
            if (availableConfigurations == null)
            {
                availableConfigurations = Shared.Configure.Load();
                generateArgs.Reset();
            }
            updateUI();
        }

        #endregion

        #region Unity user events

        public void OnGameplay()
        {
            generateArgs.GameplayOffset = (generateArgs.GameplayOffset + 1) % GameplayFactory.AvailableGameplays.Length;
            updateUI();
        }

        public void OnWorldSize()
        {
            generateArgs.SizeOffset = (generateArgs.SizeOffset + 1) % availableConfigurations.worldSizes.Length;
            updateUI();
        }

        public void OnDayLength()
        {
            generateArgs.DayInfoOffset = (generateArgs.DayInfoOffset + 1) % availableConfigurations.dayInfos.Length;
            updateUI();
        }

        public void OnGenerator()
        {
            if (GameplayFactory.AvailableGameplays[generateArgs.GameplayOffset].hasCustomGenerator == false)
            {
                generateArgs.GeneratorOffset = (generateArgs.GeneratorOffset + 1) % availableConfigurations.worldGenerators.Length;
            }
            updateUI();
        }

        public void OnHostMultplayer()
        {
            generateArgs.Multiplayer = !generateArgs.Multiplayer;
            updateUI();
        }

        public void OnGenerate()
        {
            Shared.SceneLoader.GoToGameScene(generateArgs);
        }

        #endregion

        #region Private methods

        private void updateUI()
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
