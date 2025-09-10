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
        public const string NODE_VIEW_USS = "StyleSheets/NodeView.uss";
        public const string TREE_GRAPH_VIEW_USS = "StyleSheets/TreeGraphView.uss";

        public const string NODE_VIEW_UXML = "UXML/NodeView.uxml";
        public const string EDITOR_WINDOW_UXML = "UXML/TreeFlowEditorWindow.uxml";
        
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

        /// <summary>
        /// Creates a new instance of the asset at the given path
        /// </summary>
        public static T Create<T>(string path) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.Refresh();
            return asset;
        }
        
        /// <summary>
        /// Saves the changes done to the given asset
        /// </summary>
        public static void SaveChanges(Object asset)
        {
            if (asset is null)
                return;

            if (!EditorUtility.IsDirty(asset))
                return;
            
            AssetDatabase.SaveAssetIfDirty(asset);
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Discards the changes done to the given asset
        /// </summary>
        public static void DiscardChanges(Object asset)
        {
            if (asset is null)
                return;
            
            UnityEngine.Resources.UnloadAsset(asset);
        }
    }
}