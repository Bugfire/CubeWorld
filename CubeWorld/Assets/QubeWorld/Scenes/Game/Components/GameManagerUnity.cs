﻿using UnityEngine;
using System.Collections.Generic;
using CubeWorld.Configuration;
using CubeWorld.Gameplay;

namespace GameScene
{
    public class GameManagerUnity : MonoBehaviour
    {
        public CubeWorld.World.CubeWorld world;

        public Material material;
        public Material materialTransparent;
        public Material materialTranslucid;
        public Material materialDamaged;
        public Material materialItems;

        public Material materialLiquidAnimated;

        public CubeWorld.Configuration.ConfigExtraMaterials extraMaterials;

        public SurroundingsUnity surroundingsUnity;
        public WorldManagerUnity worldManagerUnity;

        [SerializeField]
        private GameScene.ProgressBar progressBar;

        [HideInInspector]
        public PlayerUnity playerUnity;

        [SerializeField]
        public GameScene.GameController gameController;
        [SerializeField]
        private GameScene.Activator menuActivator;
        [SerializeField]
        private GameScene.Message message;

        public SectorManagerUnity sectorManagerUnity;

        public CWObjectsManagerUnity objectsManagerUnity;
        public CWFxManagerUnity fxManagerUnity;

        private bool isGenerating;

        private const string CubeworldWebServerServerRegister = "http://cubeworldweb.appspot.com/register?owner={owner}&description={description}&port={port}";

        public void Start()
        {
            MeshUtils.InitStaticValues();
            PreferencesUpdated();

            isGenerating = true;
            menuActivator.State = GameScene.MenuState.NONE;

            sectorManagerUnity = new SectorManagerUnity(this);
            objectsManagerUnity = new CWObjectsManagerUnity(this);
            fxManagerUnity = new CWFxManagerUnity(this);

            worldManagerUnity = new WorldManagerUnity(this);

            if (!Shared.SceneLoader.SetupGameWithArgs(this))
            {
                Debug.Log("No args. Run with default arguments...");
                var args = new Shared.GameLaunchArgsGenerate();
                args.Reset();
                args.Setup(this);
            }

            CommonScene.NativeHandler.SendStartEvent();
        }

        public void DestroyWorld()
        {
            objectsManagerUnity.Clear();

            sectorManagerUnity.Clear();

            if (world != null)
            {
                world.Clear();
                world = null;
            }

            surroundingsUnity.Clear();

            playerUnity = null;

            System.GC.Collect(System.GC.MaxGeneration, System.GCCollectionMode.Forced);
        }

        public bool IsPaused()
        {
            return state == GameScene.GameState.PAUSE_MENU;
        }

        public void LoadCustomTextures()
        {
#if !UNITY_WEBPLAYER

            if (Application.isEditor == false && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                try
                {
                    string exePath = System.IO.Directory.GetParent(Application.dataPath).FullName;
                    string fileTexturePath = System.IO.Path.Combine(exePath, "TexturaPrincipal.png");

                    if (System.IO.File.Exists(fileTexturePath))
                    {
                        Texture2D texture = (Texture2D)material.GetTexture("_MainTex");

                        texture.LoadImage(System.IO.File.ReadAllBytes(fileTexturePath));

                        material.mainTexture = texture;
                        materialTransparent.mainTexture = texture;
                        materialTranslucid.mainTexture = texture;
                        materialDamaged.mainTexture = texture;
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.ToString());
                }
            }

#endif
        }

        private bool registerInWebServer = false;
        private WWW registerWebServerRequest;
        private float timerUpdate = 0;

        public void RegisterInWebServer()
        {
            timerUpdate -= Time.deltaTime;

            registerInWebServer = true;

            if (timerUpdate <= 0 && (registerWebServerRequest == null || registerWebServerRequest.isDone == true))
            {
                string url = CubeworldWebServerServerRegister;
                url = url.Replace("{owner}", "fede");
                url = url.Replace("{description}", "cwserver");
                url = url.Replace("{port}", CubeWorld.Gameplay.MultiplayerServerGameplay.SERVER_PORT.ToString());
                registerWebServerRequest = new WWW(url);
                timerUpdate = 30;
            }
        }

        public bool Pause()
        {
            if (state != GameScene.GameState.GAME)
            {
                return false;
            }
            state = GameScene.GameState.PAUSE_MENU;
            menuActivator.State = GameScene.MenuState.PAUSE;
            return true;
        }

        public void Unpause()
        {
            if (state != GameScene.GameState.PAUSE_MENU)
            {
                return;
            }
            state = GameScene.GameState.GAME;
            menuActivator.State = GameScene.MenuState.NONE;
        }

        public void ReturnToTitleMenu()
        {
            DestroyWorld();

            GetComponent<Camera>().enabled = true;

            Shared.SceneLoader.GoToTitleScene();
        }

        public void Update()
        {
#if !UNITY_WEBPLAYER
            if (registerInWebServer)
                RegisterInWebServer();
#endif

            if (isGenerating)
            {
                worldManagerUnity.Update();
                if (worldManagerUnity.IsReady)
                {
                    PreferencesUpdated();
                    GetComponent<Camera>().enabled = false;
                    CommonScene.Message.AddMessage("Welcome!");
                    isGenerating = false;
                    state = GameScene.GameState.GAME;
                }
                else
                {
                    progressBar.SetText(worldManagerUnity.ProcessText);
                    progressBar.SetProgress(worldManagerUnity.Progress);
                }
            }

            switch (currentState)
            {
                case GameScene.GameState.PAUSE_MENU:
                case GameScene.GameState.GAME:
                    {
                        if (world != null && playerUnity != null)
                        {
                            surroundingsUnity.UpdateSkyColor();
                            playerUnity.UpdateControlled();
                            world.Update(Time.deltaTime);
                            UpdateAnimatedTexture();
                        }
                        break;
                    }
            }

            currentState = state;
        }

        private float textureAnimationTimer;
        private int animFrames = 5;
        private int animFrame;
        private int textureAnimationFPS = 2;

        private void UpdateAnimatedTexture()
        {
            textureAnimationTimer += Time.deltaTime;

            if (textureAnimationTimer > 1.0f / textureAnimationFPS)
            {
                textureAnimationTimer = 0.0f;

                animFrame++;

                float uvdelta = 1.0f / GraphicsUnity.TILE_PER_MATERIAL_ROW;

                materialLiquidAnimated.mainTextureOffset = new Vector2(-(animFrame % animFrames) * uvdelta, 0.0f);
            }
        }

        public void PreferencesUpdated()
        {
            if (playerUnity)
            {
                playerUnity.mainCamera.farClipPlane = CubeWorldPlayerPreferences.farClipPlanes[CubeWorldPlayerPreferences.viewDistance];
            }
        }

        private CubeWorld.Configuration.Config lastConfig;


        public bool HasLastConfig()
        {
            return lastConfig != null;
        }

        public void Generate(Shared.GameLaunchArgsGenerate args)
        {
            var availableConfigurations = Shared.Configure.Load(); ;
            lastConfig = new CubeWorld.Configuration.Config();
            lastConfig.tileDefinitions = availableConfigurations.tileDefinitions;
            lastConfig.itemDefinitions = availableConfigurations.itemDefinitions;
            lastConfig.avatarDefinitions = availableConfigurations.avatarDefinitions;
            lastConfig.dayInfo = availableConfigurations.dayInfos[args.DayInfoOffset];
            lastConfig.worldGenerator = availableConfigurations.worldGenerators[args.GeneratorOffset];
            lastConfig.worldSize = availableConfigurations.worldSizes[args.SizeOffset];
            lastConfig.extraMaterials = availableConfigurations.extraMaterials;
            lastConfig.gameplay = GameplayFactory.AvailableGameplays[args.GameplayOffset];

            if (args.Multiplayer)
            {
                MultiplayerServerGameplay multiplayerServerGameplay = new MultiplayerServerGameplay(lastConfig.gameplay.gameplay, true);
                GameplayDefinition g = new GameplayDefinition("", "", multiplayerServerGameplay, false);
                lastConfig.gameplay = g;
                RegisterInWebServer();
            }

            worldManagerUnity.CreateRandomWorld(lastConfig);
            menuActivator.State = GameScene.MenuState.NONE;
        }

        public void ReGenerate()
        {
            if (!HasLastConfig())
            {
                return;
            }
            GetComponent<Camera>().enabled = true;
            worldManagerUnity.CreateRandomWorld(lastConfig);
            menuActivator.State = GameScene.MenuState.NONE;
        }
    }
}
