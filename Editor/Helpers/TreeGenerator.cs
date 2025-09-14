using System.IO;
using System.Text;
using TreeFlow.Editor.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace TreeFlow.Editor.Helpers
{
    /// <summary>
    ///     Class that handles the generator of script from a <see cref="BehaviorTreeAsset" />
    /// </summary>
    internal static class TreeGenerator
    {
        private static void Prepare(BehaviorTreeAsset tree)
        {
            tree.Compute();
            TreeSanitizer.RemoveDetachedNodes(tree);
            TreeSanitizer.FixChildNodes(tree);
            TreeOptimizer.Optimize(tree);
        }

        /// <summary>
        ///     Generates a script from the given <see cref="BehaviorTreeAsset" />
        /// </summary>
        public static void Generate(BehaviorTreeAsset tree)
        {
            var path = EditorUtility.SaveFilePanel(
                "Generate Behavior Tree",
                Application.dataPath,
                tree.name,
                "cs"
            );

            if (string.IsNullOrEmpty(path))
                return;

            Generate(tree, path);
        }

        /// <summary>
        ///     Generates a script from the given <see cref="BehaviorTreeAsset" /> at the given path
        /// </summary>
        public static void Generate(BehaviorTreeAsset tree, string path)
        {
            var template = Resources.LoadResource<TextAsset>("Templates/BehaviorTree.txt");

            if (template == null)
            {
                Debug.LogError("Failed to find the template.");
                return;
            }

            var className = Path.GetFileNameWithoutExtension(path);

            Prepare(tree);

            var output = template.text;
            output = output.Replace("#CLASS_NAME#", className);

            File.WriteAllText(path, output, Encoding.UTF8);
        }
    }
}