using System.Collections.Generic;
using TreeFlow.Editor.Renderers;
using TreeFlow.Runtime.Core;
using TreeFlow.Runtime.Nodes.Composite;
using TreeFlow.Runtime.Nodes.Decorator;
using TreeFlow.Runtime.Nodes.Leaf;
using UnityEditor;
using UnityEngine;

namespace TreeFlow.Editor
{
    public class TreeVisualizer : EditorWindow
    {
        private Vector2 scrollPos;

        [MenuItem("Window/TreeFlow/Visualizer")]
        public static void ShowWindow()
        {
            var window = GetWindow<TreeVisualizer>("Tree Visualizer");
            window.minSize = new Vector2(600, 400);
        }

        private void OnGUI()
        {
            InitializeStyles();
            
            DrawHeader();
            DrawOptionsPanel();
            GUILayout.Space(10);
            DrawVisualizerArea();

            EditorGUILayout.HelpBox("Visualizer Canvas (Nodes would appear here)", MessageType.Warning);
            GUILayout.Space(4);
        }

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
            var treeRenderer = new TreeRenderer();

            var tree = new SequenceNode(
                new SequenceNode(),
                new SequenceNode(
                    new SequenceNode(),
                    new SequenceNode(
                        new CallbackNode(n => NodeStatus.FAILURE)
                    ),
                    new InverterNode(
                        new SequenceNode(
                            new CallbackNode(n => NodeStatus.FAILURE), 
                            new CallbackNode(n => NodeStatus.FAILURE),
                            new CallbackNode(n => NodeStatus.FAILURE)
                        )
                    )
                )
            );
            
            GUILayout.BeginVertical("box");
            GUILayout.Label("Visualizer", EditorStyles.boldLabel);

            scrollPos = GUILayout.BeginScrollView(
                scrollPos,
                darkBackgroundStyle
            );
            
            treeRenderer.Draw(tree);
            
            GUILayout.EndScrollView();

            GUILayout.EndVertical();

            return;
            
            /*GUILayout.FlexibleSpace();

            GUILayout.Label("No Tree Selected", new GUIStyle(EditorStyles.centeredGreyMiniLabel)
            {
                alignment = TextAnchor.MiddleCenter
            });

            GUILayout.FlexibleSpace();
            
            
            
            //Debug.Log(nodeRect);
            
            

            scrollPos = GUILayout.BeginScrollView(
                scrollPos,
                true,  // horizontal
                true,  // vertical
                GUI.skin.horizontalScrollbar,
                GUI.skin.verticalScrollbar
            );
            
            var bgTexture = new Texture2D(1, 1);
            bgTexture.SetPixel(0, 0, new Color(0.63f, 0.07f, 0f));
            bgTexture.Apply();
            
            Rect contentRect = new Rect(0, 0, 300, 300);
            GUILayout.BeginArea(contentRect, new GUIStyle(GUI.skin.box)
            {
                normal =
                {
                    background = bgTexture
                }
            });

            
            
            GUILayout.EndArea();

            GUILayout.EndScrollView();
            
            GUILayout.EndVertical();*/
        }
    }
}