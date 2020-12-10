using UnityEngine;
using System.Collections.Generic;
using CubeWorld.World.Generator;
using CubeWorld.Gameplay;
using CubeWorld.Configuration;

namespace Shared
{
    public static class WorldFileIO
    {
        static private Dictionary<int, System.DateTime> worldFileInfoCache = new Dictionary<int, System.DateTime>();

        #region Public methods

        static public System.DateTime GetInfo(int n)
        {
            if (worldFileInfoCache.ContainsKey(n))
            {
                return worldFileInfoCache[n];
            }
            try
            {
                var path = GetWorldFilePath(n);
                if (!System.IO.File.Exists(path))
                {
                    return worldFileInfoCache[n] = System.DateTime.MinValue;
                }

                var fs = System.IO.File.OpenRead(path);
                try
                {
                    var br = new System.IO.BinaryReader(fs);
                    if (br.ReadString() == CubeWorld.World.CubeWorld.VERSION_INFO)
                    {
                        worldFileInfoCache[n] = System.IO.File.GetLastWriteTime(path);
                    }
                    else
                    {
                        worldFileInfoCache[n] = System.DateTime.MinValue;
                    }
                }
                finally
                {
                    fs.Close();
                }
            }
            catch (System.Exception)
            {
                return System.DateTime.MinValue;
            }
            return worldFileInfoCache[n];
        }

        public static byte[] Load(int n)
        {
            if (!System.IO.File.Exists(GetWorldFilePath(n)))
            {
                return null;
            }
            return System.IO.File.ReadAllBytes(GetWorldFilePath(n));
        }

        public static void Save(int n, byte[] data)
        {
            System.IO.File.WriteAllBytes(GetWorldFilePath(n), data);
            worldFileInfoCache.Clear();
        }

        #endregion

        #region Private methods

        private static string GetWorldFilePath(int n)
        {
            var exePath = System.IO.Directory.GetParent(Application.dataPath).FullName;
            return System.IO.Path.Combine(exePath, "world" + n + ".map");
        }

        #endregion
    }
}
