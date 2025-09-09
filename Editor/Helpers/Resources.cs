using System.IO;
using UnityEditor;
using UnityEngine;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    /// Class that simplifies the management of resources 
    /// </summary>
    internal static class Resources
    {
        /// <summary>
        /// Converts the given relative path to the absolute path
        /// </summary>
        public static string RelativeToAbsolute(string relativePath)
        {
            const string ROOT_PATH = "Assets/dev.warpersan.treeflow/Editor/Resources/";
            
            return Path.Combine(ROOT_PATH, relativePath);
        }
        
        /// <summary>
        /// Loads the resource at the given path from the root 
        /// </summary>
        public static T Load<T>(string path) where T : Object => AssetDatabase.LoadAssetAtPath<T>(RelativeToAbsolute(path));
    }
}