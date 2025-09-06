using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    /// Class that manages the editor resources used by this package
    /// </summary>
    internal static class ResourceManager
    {
        private readonly static Dictionary<string, object> cachedResources = new();

        /// <summary>
        /// Loads the asset with the given GUID
        /// </summary>
        public static T Load<T>(string guid, bool useCache = true) where T : Object
        {
            if (useCache && cachedResources.TryGetValue(guid, out var resource))
                return (T)resource;

            var path = AssetDatabase.GUIDToAssetPath(guid);
            Object asset = null;

            if (path != null)
                asset = AssetDatabase.LoadAssetAtPath<T>(path);
            
            if (useCache)
                cachedResources[guid] = asset;

            return (T)asset;
        }

        /// <summary>
        /// Loads the text from the given GUID
        /// </summary>
        public static string LoadRaw(string guid, bool useCache = true)
        {
            if (useCache && cachedResources.TryGetValue(guid, out var resource))
                return (string)resource;

            var path = AssetDatabase.GUIDToAssetPath(guid);
            string asset = null;

            if (path != null)
                asset = File.ReadAllText(path);
            
            if (useCache)
                cachedResources[guid] = asset;

            return asset;
        }
    }
}