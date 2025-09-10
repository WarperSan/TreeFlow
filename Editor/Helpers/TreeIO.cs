using System.IO;
using TreeFlow.Editor.ScriptableObjects;
using TreeFlow.Editor.Nodes.Composite;
using UnityEditor;
using UnityEngine;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    /// Class that simplifies the IO for <see cref="BehaviorTreeAsset"/> 
    /// </summary>
    internal static class TreeIO
    {
        /// <summary>
        /// Prompts the user if they want to load the given tree or not
        /// </summary>
        public static void PromptLoadTree(BehaviorTreeAsset asset)
        {
            var isOkay = EditorUtility.DisplayDialog(
                "Open Tree",
                $"Would you like to open '{asset.name}'?",
                "Yes",
                "No"
            );
            
            if (!isOkay)
                return;
            
            TreeFlowEditorWindow.Open(asset);
        }
        
        /// <summary>
        /// Prompts the user to create a new <see cref="BehaviorTreeAsset"/>
        /// </summary>
        public static BehaviorTreeAsset PromptCreateTree()
        {
            var newPath = EditorUtility.SaveFilePanelInProject(
                "Create a Tree",
                "NewBehaviorTree",
                "asset",
                ""
            );

            if (string.IsNullOrEmpty(newPath))
                return null;
            
            var asset = Resources.Create<BehaviorTreeAsset>(newPath);
            
            if (asset is null)
                return null;

            var root = asset.AddNode<SelectorNodeAsset>();
            asset.RootGUID = root.GUID;

            Resources.SaveChanges(asset);
            return asset;
        }
        
        /// <summary>
        /// Prompts the user to open a <see cref="BehaviorTreeAsset"/>
        /// </summary>
        public static BehaviorTreeAsset PromptOpenTree()
        {
            var path = EditorUtility.OpenFilePanel("Open a Tree", "Assets/", "asset");
                
            if (string.IsNullOrEmpty(path))
                return null;

            if (Path.IsPathRooted(path))
            {
                var targetPath = Path.GetDirectoryName(Application.dataPath);
                path = Path.GetRelativePath(targetPath, path);
            }

            var asset = AssetDatabase.LoadAssetAtPath<BehaviorTreeAsset>(path);

            if (asset is not null)
                return asset;

            Debug.LogWarningFormat("Failed to load a '{0}' from '{1}'.", nameof(BehaviorTreeAsset), path);
            return null;
        }

        /// <summary>
        /// Prompts the user to save the given <see cref="BehaviorTreeAsset"/>
        /// </summary>
        public static string PromptSaveTree(BehaviorTreeAsset asset)
        {
            var path = AssetDatabase.GetAssetPath(asset);

            if (string.IsNullOrEmpty(path))
                return null;
            
            var newPath = EditorUtility.SaveFilePanelInProject(
                "Save Tree As...",
                "NewBehaviourTree",
                "asset",
                ""
            );
            
            if (string.IsNullOrEmpty(newPath))
                return null;

            AssetDatabase.CopyAsset(path, newPath);
            AssetDatabase.Refresh();
            return newPath;
        }
    }
}