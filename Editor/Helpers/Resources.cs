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
        /// Prompts the user to create an asset file
        /// </summary>
        public static T PromptCreateFile<T>(string title, string defaultName, string extension) where T : ScriptableObject
        {
            var newPath = EditorUtility.SaveFilePanelInProject(title, defaultName, extension, "");
            
            if (string.IsNullOrEmpty(newPath))
                return null;
            
            var asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, newPath);
            AssetDatabase.Refresh();
            return asset;
        }
        
        /// <summary>
        /// Prompts the user to open an asset file
        /// </summary>
        public static T PromptOpenFile<T>(string title, string extension) where T : Object
        {
            var path = EditorUtility.OpenFilePanel(title, "Assets/", extension);
                
            if (string.IsNullOrEmpty(path))
                return null;

            if (Path.IsPathRooted(path))
            {
                var targetPath = Path.GetDirectoryName(Application.dataPath);
                path = Path.GetRelativePath(targetPath, path);
            }

            var asset = AssetDatabase.LoadAssetAtPath<T>(path);

            if (asset is not null)
                return asset;

            Debug.LogWarningFormat("Failed to load a '{0}' from '{1}'.", typeof(T).Name, path);
            return null;
        }

        /// <summary>
        /// Prompts the user to save the asset to a file
        /// </summary>
        public static string PromptSaveFile<T>(T asset, string title, string defaultName, string extension) where T : ScriptableObject
        {
            var path = AssetDatabase.GetAssetPath(asset);

            if (string.IsNullOrEmpty(path))
                return null;
            
            var newPath = EditorUtility.SaveFilePanelInProject(title, defaultName, extension, "");
            
            if (string.IsNullOrEmpty(newPath))
                return null;

            AssetDatabase.CopyAsset(path, newPath);
            AssetDatabase.Refresh();
            return newPath;
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