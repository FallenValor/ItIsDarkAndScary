/*****************************************************************************
// File Name : NodePointEditor.cs
// Author : Brandon Koederitz
// Creation Date : 4/4/2026
// Last Modified : 4/4/2026
//
// Brief Description : Custom editor for NodePoints that controls easy assignment of the linked node.
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;

namespace IDAS.Decisions.Editors
{
    [CustomEditor(typeof(NodePoint))]
    public class NodePointEditor : UnityEditor.Editor
    {
        private int selectionIndex;

        // Serialized Properties
        private SerializedProperty tree;
        private SerializedProperty node;
        private SerializedProperty oldNodeName;
        private SerializedProperty splines;

        /// <summary>
        /// Initialize SerializedProperties
        /// </summary>
        public void OnEnable()
        {
            tree = serializedObject.FindProperty(nameof(tree));
            node = serializedObject.FindProperty(nameof(node));
            oldNodeName = serializedObject.FindProperty(nameof(oldNodeName));
            splines = serializedObject.FindProperty(nameof(splines));
        }


        /// <summary>
        /// Draw the NodePoint editor.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            NodePoint point = (NodePoint)target;

            // Draw the default tree property
            EditorGUILayout.PropertyField(tree);

            // Draw the dropdown for the node name.
            if (point.Tree != null)
            {
                DarkScaryNode[] nodes = point.Tree.nodes.Select(n => n as DarkScaryNode).ToArray();
                string[] nodeNames = nodes.Select(n => n.name).ToArray();
                selectionIndex = GetSelectionIndex(node, nodes);

                // Display error text if the node point has an invalid name.
                if (selectionIndex == -1 && oldNodeName.stringValue != "")
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox($"Old node reference was deleted.  " +
                        $"Old Node Name: {oldNodeName.stringValue}", MessageType.Warning);
                }

                // Show the popup for choosing a name of a node.
                EditorGUI.BeginChangeCheck();
                selectionIndex = EditorGUILayout.Popup("Node Selector", selectionIndex, nodeNames);
                if (EditorGUI.EndChangeCheck())
                {
                    // Update the string field.
                    node.objectReferenceValue = point.Tree.nodes[selectionIndex];
                    oldNodeName.stringValue = point.Tree.nodes[selectionIndex].name;

                    // Update the node's name.
                    point.gameObject.name = nameof(NodePoint) +  " (" + point.Tree.nodes[selectionIndex].name + ")";
                }
                GUI.enabled = false;
                EditorGUILayout.PropertyField(node);
                EditorGUILayout.PropertyField(splines);
                GUI.enabled = true;
            }

            // Update the splines for this node to another node.
            if (GUILayout.Button("Automatic Link Splines"))
            {
                UpdateNodeSpline(point, splines);
            }
               
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Gets the index of the current stored node name value.
        /// </summary>
        /// <param name="nodeProp">The SerializedProperty storing the name of the node this point represents.</param>
        /// <param name="nodes">The string of all node names in the DecisionTree.</param>
        /// <returns>The index of the current node name.</returns>
        private int GetSelectionIndex(SerializedProperty nodeProp, DarkScaryNode[] nodes)
        {
            DarkScaryNode node = nodeProp.objectReferenceValue as DarkScaryNode;

            // If no node is selected, then set it to the first node.
            if (node == null)
            {
                // Do not update the OldNodeName property.
                selectionIndex = 0;
                nodeProp.objectReferenceValue = nodes[selectionIndex];
                return selectionIndex;
            }

            return Array.IndexOf(nodes, node);
        }

        /// <summary>
        /// Automatically links this node to it's transition nodes with a cinemachine spline.
        /// </summary>
        /// <param name="point">The node point to update splines for.</param>
        private void UpdateNodeSpline(NodePoint point, SerializedProperty splinesProp)
        {
            // Find the other NodePoints in the scene.
            List<NodePoint> nodes = GetAllNodePointsInScene();
            DarkScaryNode[] nextNodes = point.Node.GetAllNextNodes();

            // Clear the existing splines.
            for(int i = 0; i < point.Splines.Length; i++)
            {
                if (point.Splines[i] == null) { continue; }
                DestroyImmediate(point.Splines[i].gameObject);
            }
            splinesProp.ClearArray();

            splinesProp.arraySize = nextNodes.Length;
            // Create new splines.
            for (int i = 0; i < nextNodes.Length; i++)
            {
                if (nextNodes[i] == null) { continue; }
                // Find the corresponding node point for this node.
                NodePoint linkedPoint = nodes.Find(x => x.Node == nextNodes[i]);

                // Create the spline
                Spline newSpline = new Spline();
                // Set the spline's start knot.
                float3 startPos = new float3(0, 0, 0);
                BezierKnot startKnot = new BezierKnot(startPos);
                newSpline.Add(startKnot);
                // Set the spline's end knot.
                Vector3 toLinkVector = linkedPoint != null ? linkedPoint.transform.position - 
                    point.transform.position : Vector3.zero;
                float3 endPos = new float3(toLinkVector.x, toLinkVector.y, toLinkVector.z);
                BezierKnot endKnot = new BezierKnot(endPos);
                newSpline.Add(endKnot);

                // Create the spline GameObject.
                GameObject splineGO = new GameObject(nextNodes[i].name + " Spline");
                splineGO.transform.SetParent(point.transform, false);
                SplineContainer splineCont = splineGO.AddComponent<SplineContainer>();
                splineGO.AddComponent<CinemachineSplineSmoother>();

                // Add the spline.
                SplineUtility.AddSpline(splineCont, newSpline);
                splinesProp.GetArrayElementAtIndex(i).objectReferenceValue = splineCont;
            }
        }

        /// <summary>
        /// Gets all node points in the current scene.
        /// </summary>
        /// <returns>A list of all node points in the current scene.</returns>
        private List<NodePoint> GetAllNodePointsInScene()
        {
            List<NodePoint > nodes = new List<NodePoint>();
            GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach(var root in roots)
            {
                nodes.AddRange(root.GetComponentsInChildren<NodePoint>());
            }
            return nodes;
        }
    }
}
