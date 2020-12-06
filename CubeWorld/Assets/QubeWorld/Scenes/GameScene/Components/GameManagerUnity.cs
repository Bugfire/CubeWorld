using UnityEngine;
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

        [SerializeField]
        private ProgressBar progressBar;

        [HideInInspector]
        public PlayerUnity playerUnity;

        [SerializeField]
        public GameController gameController;
        [SerializeField]
        private GameScene gameScene;

        public SectorManagerUnity sectorManagerUnity;

        public CWObjectsManagerUnity objectsManagerUnity;
        public CWFxManagerUnity fxManagerUnity;

        private IWorldGenerator worldGenerator;

        private const string CubeworldWebServerServerRegister = "http://cubeworldweb.appspot.com/register?owner={owner}&description={description}&port={port}";

        public void Start()
        {
            UnityEngine.SceneManagement.SceneManager.SetActiveScene(gameObject.scene);

            MeshUtils.InitStaticValues();
            PreferencesUpdated();

            sectorManagerUnity = new SectorManagerUnity(this);
            objectsManagerUnity = new CWObjectsManagerUnity(gameScene, this);
            fxManagerUnity = new CWFxManagerUnity(this);

            var args = Shared.SceneLoader.GetWorldGeneratorArgs();
            worldGenerator = WorldGeneratorFactory.Run(this, args);
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

        public void ReGenerate()
        {
            if (!HasLastConfig())
            {
                return;
            }
            worldGenerator = RandomWorldGenerator.Factory(this, lastConfig);
            gameScene.State = State.GENERATING;
        }

        public void LoadCustomTextures()
        {
            if (Application.isEditor == false && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                try
                {
                    string fileTexturePath = System.IO.Path.Combine(Application.persistentDataPath, "TexturaPrincipal.png");

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

        public void Update()
        {
#if !UNITY_WEBPLAYER
            if (registerInWebServer)
                RegisterInWebServer();
#endif

            if (gameScene.IsGenerating)
            {
                worldGenerator.Update();
                if (worldGenerator.IsFinished)
                {
                    worldGenerator = null;
                    gameScene.State = State.GAME;
                    PreferencesUpdated();
                    CommonScene.Message.AddMessage("<b>Welcome!</b>");
                }
                else
                {
                    progressBar.SetText(worldGenerator.ProcessText);
                    progressBar.SetProgress(worldGenerator.Progress);
                }
            }

            if (!gameScene.IsGenerating && world != null && playerUnity != null)
            {
                surroundingsUnity.UpdateSkyColor();
                playerUnity.UpdateControlled();
                world.Update(Time.deltaTime);
                UpdateAnimatedTexture();
            }
        }

        public void SaveWorld(int n)
        {
            var map = world.Save();
            Shared.WorldFileIO.Save(n, map);
        }

        public CubeWorld.Configuration.Config CreateConfig(Shared.RandomWorldGeneratorArgs args)
        {
            var availableConfigurations = Shared.Configure.Load();
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

            return lastConfig;
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
                playerUnity.mainCamera.farClipPlane = Shared.Settings.farClipPlanes[Shared.Settings.viewDistance];
            }
        }

        private CubeWorld.Configuration.Config lastConfig;


        public bool HasLastConfig()
        {
            return lastConfig != null;
        }
    }
}
