using UnityEngine;
using CubeWorld.Configuration;

namespace Shared
{
    public static class Configure
    {
        static private string GetConfigText(string resourceName)
        {
            string configText = ((TextAsset)Resources.Load(resourceName)).text;

            if (Application.isEditor == false && Application.platform != RuntimePlatform.WebGLPlayer)
            {
                try
                {
                    string exePath = System.IO.Directory.GetParent(Application.dataPath).FullName;
                    string fileConfigPath = System.IO.Path.Combine(exePath, resourceName + ".xml");

                    if (System.IO.File.Exists(fileConfigPath))
                        configText = System.IO.File.ReadAllText(fileConfigPath);
                }
                catch (System.Exception ex)
                {
                    Debug.Log(ex.ToString());
                }
            }

            return configText;
        }

        static public AvailableConfigurations Load()
        {
            AvailableConfigurations availableConfigurations =
                new ConfigParserXML().Parse(
                    GetConfigText("config_misc"),
                    GetConfigText("config_tiles"),
                    GetConfigText("config_avatars"),
                    GetConfigText("config_items"),
                    GetConfigText("config_generators"));

            return availableConfigurations;
        }
    }
}
