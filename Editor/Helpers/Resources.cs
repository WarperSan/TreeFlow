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

        public static string NODE_VIEW_USS => FromPackageRoot("StyleSheets/NodeView.uss");
        public static string TREE_GRAPH_VIEW_USS => FromPackageRoot("StyleSheets/TreeGraphView.uss");

        public static string NODE_VIEW_UXML => FromPackageRoot("UXML/NodeView.uxml");
        public static string EDITOR_WINDOW_UXML => FromPackageRoot("UXML/TreeFlowEditorWindow.uxml");

        /// <summary>
        ///     Converts the given absolute path to a path relative to <see cref="Application.dataPath" />
        /// </summary>
        public static string AbsoluteToRelative(string absolutePath)
        {
            var projectPath = Path.GetDirectoryName(Application.dataPath);

            return Path.GetRelativePath(projectPath, absolutePath);
        }

        /// <summary>
        ///     Converts the given relative path of an editor resource to an absolute path
        /// </summary>
        private static string FromPackageRoot(string relativePath)
        {
            var assembly = System.Reflection.Assembly.GetExecutingAssembly();
            var packageInfo = UnityEditor.PackageManager.PackageInfo.FindForAssembly(assembly);
            var sourcePath = packageInfo?.assetPath ?? Application.dataPath;
            
            return Path.Combine(
                sourcePath,
                EDITOR_ROOT,
                relativePath
            );
        }

        /// <summary>
        ///     Loads the resource at the given absolute path
        /// </summary>
        public static T Load<T>(string path) where T : Object
        {
            AssetDatabase.Refresh();
            return AssetDatabase.LoadAssetAtPath<T>(AbsoluteToRelative(path));
        }

        /// <summary>
        ///     Saves the given asset to the given absolute path
        /// </summary>
        public static void Save(Object asset, string path)
        {
            AssetDatabase.CreateAsset(asset, AbsoluteToRelative(path));
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