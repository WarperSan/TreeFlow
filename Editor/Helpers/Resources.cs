using System.IO;
using UnityEditor;
using UnityEngine;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    ///     Class that simplifies the management of resources
    /// </summary>
    internal static class Resources
    {
        private const string EDITOR_ROOT = "dev.warpersan.treeflow/Editor/Resources/";

        public const string NODE_VIEW_UXML = EDITOR_ROOT + "UXML/NodeView.uxml";
        public const string EDITOR_WINDOW_UXML = EDITOR_ROOT + "UXML/TreeFlowEditorWindow.uxml";

        /// <summary>
        ///     Gets the package path from the given relative path
        /// </summary>
        public static string GetPackageResource(string path)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(assembly);

            var sourcePath = Path.GetDirectoryName(packageInfo?.assetPath) ?? Path.GetFileName(Application.dataPath);

            return Path.Combine(sourcePath, path);
        }

        /// <summary>
        ///     Loads the editor resource at the given relative path
        /// </summary>
        public static T LoadResource<T>(string path) where T : Object
        {
            AssetDatabase.Refresh();
            return AssetDatabase.LoadAssetAtPath<T>(GetPackageResource(path));
        }

        /// <summary>
        ///     Saves the given asset to the given absolute path
        /// </summary>
        public static void SaveAsset(Object asset, string path)
        {
            var projectPath = Path.GetDirectoryName(Application.dataPath);
            path = Path.GetRelativePath(projectPath, path);
            
            AssetDatabase.CreateAsset(asset, path);
            AssetDatabase.Refresh();
        }

        /// <summary>
        ///     Saves the changes done to the given asset
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
        ///     Discards the changes done to the given asset
        /// </summary>
        public static void DiscardChanges(Object asset)
        {
            if (asset is null)
                return;

            UnityEngine.Resources.UnloadAsset(asset);
        }
    }
}