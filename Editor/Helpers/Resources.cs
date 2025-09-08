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
        /// Loads the resource at the given path from the root 
        /// </summary>
        public static T Load<T>(string path) where T : Object
        {
            const string ROOT_PATH = "Assets/dev.warpersan.treeflow/Editor/Resources/";
            
            return AssetDatabase.LoadAssetAtPath<T>(Path.Combine(ROOT_PATH, path));
        }
    }
}