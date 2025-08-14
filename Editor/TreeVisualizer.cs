using TreeFlow.Editor.Renderers;
using TreeFlow.Runtime.Core;
using UnityEditor;
using UnityEngine;

namespace TreeFlow.Editor
{
    public class TreeVisualizer : EditorWindow
    {
        private Vector2 scrollPos;
        private TreeRenderer treeRenderer;

        [MenuItem("Window/TreeFlow/Visualizer")]
        public static void ShowWindow() => GetWindow<TreeVisualizer>("Tree Visualizer");

        private void Update()
        {
            if (Application.isPlaying)
                Repaint();
        }

        private void OnGUI()
        {
            treeRenderer ??= new TreeRenderer();
            
            var target = Selection.activeGameObject;

            if (target is not null && target.TryGetComponent(out BaseTree tree))
                treeRenderer.SetTree(tree);
            //else
            //    treeRenderer.SetTree(null);
            
            InitializeStyles();
            
            DrawHeader();
            //DrawOptionsPanel();
            //GUILayout.Space(10);
            DrawVisualizerArea();
        }

        private void OnSelectionChange() => Repaint();

        #region Styles

        private GUIStyle darkBackgroundStyle;

        private void InitializeStyles()
        {
            darkBackgroundStyle = new GUIStyle(GUI.skin.box)
            {
                normal = new GUIStyleState
                {
                    background = Texture2D.whiteTexture, // Fallback, will be overwritten
                    textColor = Color.white
                }
            };

            // Set the dark background color
            var bgTexture = new Texture2D(1, 1);
            bgTexture.SetPixel(0, 0, new Color(0.15f, 0.15f, 0.15f, 1f)); // Dark gray background
            bgTexture.Apply();
            darkBackgroundStyle.normal.background = bgTexture;
        }

        #endregion

        // ReSharper disable Unity.PerformanceAnalysis
        private static void DrawHeader()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            GUILayout.Label("TreeFlow - Tree Visualizer", EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();
            
            if (GUILayout.Button("Refresh", EditorStyles.toolbarButton))
            {
                Debug.Log("Refresh clicked");
            }

            GUILayout.EndHorizontal();
        }
        
        private void DrawOptionsPanel()
        {
            GUILayout.BeginVertical("box");

            GUILayout.Label("Options", EditorStyles.boldLabel);

            GUILayout.Space(4);
            EditorGUILayout.LabelField("Visual Settings", EditorStyles.miniBoldLabel);

            GUILayout.Space(4);
            EditorGUILayout.LabelField("Behavior Settings", EditorStyles.miniBoldLabel);

            GUILayout.EndVertical();
        }

        private void DrawVisualizerArea()
        {
            GUILayout.BeginVertical("box");
            GUILayout.Label("Visualizer", EditorStyles.boldLabel);

            scrollPos = GUILayout.BeginScrollView(
                scrollPos,
                darkBackgroundStyle
            );
            
            treeRenderer?.Draw();
            
            GUILayout.EndScrollView();

            GUILayout.EndVertical();
        }
    }
}