﻿using UnityEngine;

namespace Shared
{
    public class Settings
    {
        static public int[] farClipPlanes;
        static public int viewDistance;
        static public bool showFPS;
        static public bool showEngineStats;
        static public GameScene.SectorManagerUnity.VisibleStrategy visibleStrategy;

        static public void LoadPreferences()
        {
            farClipPlanes = new int[6];
            farClipPlanes[0] = 1024;
            farClipPlanes[1] = 512;
            farClipPlanes[2] = 256;
            farClipPlanes[3] = 128;
            farClipPlanes[4] = 64;
            farClipPlanes[5] = 32;

            viewDistance = PlayerPrefs.GetInt("ViewDistance", 4);
            showFPS = PlayerPrefs.GetInt("ShowFPS", 0) != 0;
            showEngineStats = PlayerPrefs.GetInt("ShowEngineStats", 0) != 0;
            visibleStrategy = (GameScene.SectorManagerUnity.VisibleStrategy)System.Enum.Parse(typeof(GameScene.SectorManagerUnity.VisibleStrategy), PlayerPrefs.GetString("VisibleStrategy", "All"), true);
        }

        static public void StorePreferences()
        {
            try
            {
                PlayerPrefs.DeleteAll();

                PlayerPrefs.SetInt("ViewDistance", viewDistance);
                PlayerPrefs.SetInt("ShowFPS", showFPS ? 1 : 0);
                PlayerPrefs.SetInt("ShowEngineStats", showEngineStats ? 1 : 0);
                PlayerPrefs.SetString("VisibleStrategy", System.Enum.GetName(typeof(GameScene.SectorManagerUnity.VisibleStrategy), visibleStrategy));
            }
            catch (PlayerPrefsException ex)
            {
                Debug.Log("Failed to update preferences: " + ex.ToString());
            }
        }
    }
}
