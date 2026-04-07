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
        private bool initialized;


        // Serialized Properties
        private SerializedProperty tree;
        private SerializedProperty node;
        private SerializedProperty oldNodeName;
        private SerializedProperty splines;
        private SerializedProperty nextPoints;
        private SerializedProperty cCam;
        private SerializedProperty isDuplicate;

        /// <summary>
        /// Initialize SerializedProperties
        /// </summary>
        public void OnEnable()
        {
            tree = serializedObject.FindProperty(nameof(tree));
            node = serializedObject.FindProperty(nameof(node));
            oldNodeName = serializedObject.FindProperty(nameof(oldNodeName));
            splines = serializedObject.FindProperty(nameof(splines));
            cCam = serializedObject.FindProperty(nameof(cCam));
            nextPoints = serializedObject.FindProperty(nameof(nextPoints));
            isDuplicate = serializedObject.FindProperty(nameof(isDuplicate));
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

                // Checks for initialization.
                if (!initialized)
                {
                    Initialize(point, nodes);
                }

                string[] nodeNames = nodes.Select(n => n.name).ToArray();
                selectionIndex = GetSelectionIndex(node, nodes);

                // Display error text if the node point has an invalid name.
                if (oldNodeName.stringValue != node.objectReferenceValue.name)
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
                    DarkScaryNode newNode = point.Tree.nodes[selectionIndex] as DarkScaryNode;
                    node.objectReferenceValue = newNode;
                    oldNodeName.stringValue = newNode.name;

                    // Update the node's name.
                    point.gameObject.name = nameof(NodePoint) +  " (" + point.Tree.nodes[selectionIndex].name + ")";

                    // Verify the node is unique.
                    isDuplicate.boolValue = CheckIsDuplicate(point, newNode);
                }

                if (isDuplicate.boolValue == true)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox($"This point is a duplicate point referencing " +
                        $"{node.objectReferenceValue.name} and will be ignored.  Ensure that each node only has " +
                        $"one point.", MessageType.Error);
                }

                GUI.enabled = false;
                EditorGUILayout.PropertyField(node);
                if (!point.IsDuplicate)
                {
                    EditorGUILayout.PropertyField(nextPoints);
                    EditorGUILayout.PropertyField(splines);
                }
                GUI.enabled = true;

                // Show buttons for spline management.
                if (!point.IsDuplicate)
                {
                    // Update the splines for this node to another node.
                    if (GUILayout.Button("Create Splines"))
                    {
                        CreateNodeSplines(point, splines, nextPoints);
                    }
                    // Update the splines for this node to another node.
                    if (GUILayout.Button("Update Spline End Points"))
                    {
                        UpdateSplineEndPoints(point);
                    }
                }
            }

            //Show components if null.
            if (cCam.objectReferenceValue == null)
            {
                EditorGUILayout.PropertyField(cCam);
            }
               
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Controls functions that hsould only be called once the gui starts rendering.
        /// </summary>
        private void Initialize(NodePoint point, DarkScaryNode[] nodes)
        {
            selectionIndex = GetSelectionIndex(node, nodes);

            // Verify the node is unique.
            isDuplicate.boolValue = CheckIsDuplicate(point, point.Node);
        }

        /// <summary>
        /// Updates all the end points of splines related to this node.
        /// </summary>
        /// <param name="point"></param>
        private void UpdateSplineEndPoints(NodePoint point)
        {
            // Update the in spline's end point.
            //SetSplineEndPoint(point.InSpline, point.transform.position);
            List<NodePoint> allPoints = GetAllNodePointsInScene();
            List<NodePoint> allInPoints = allPoints.Where(x => x.Node.GetAllNextNodes().Contains(point.Node)).ToList();
            foreach(var p in allInPoints)
            {
                DarkScaryNode[] nextNodes = p.Node.GetAllNextNodes();
                for (int i = 0; i < p.Splines.Length; i++)
                {
                    if (nextNodes[i] == point.Node)
                    {
                        SetSplineEndPoint(p.Splines[i], point.transform.position);
                    }
                }
            }

            // Update all of the out spline's end points.
            for (int i = 0; i < point.Splines.Length; i++)
            {
                SetSplineEndPoint(point.Splines[i], point.NextPoints[i].transform.position);
            }
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
        private void CreateNodeSplines(NodePoint point, SerializedProperty splinesProp, SerializedProperty nextPoints)
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
            nextPoints.ClearArray();

            splinesProp.arraySize = nextNodes.Length;
            nextPoints.arraySize = nextNodes.Length;
            // Create new splines.
            for (int i = 0; i < nextNodes.Length; i++)
            {
                if (nextNodes[i] == null) { continue; }
                // Find the corresponding node point for this node.
                NodePoint linkedPoint = nodes.Find(x => x.Node == nextNodes[i]);

                // Create the spline GameObject.
                GameObject splineGO = new GameObject(nextNodes[i].name + " Spline");
                splineGO.transform.SetParent(point.transform, false);
                SplineContainer splineCont = splineGO.AddComponent<SplineContainer>();
                splineGO.AddComponent<CinemachineSplineSmoother>();

                // Create the spline
                Spline spline = splineCont.Spline;
                // Set the spline's start knot.
                float3 startPos = new float3(0, 0, 0);
                BezierKnot startKnot = new BezierKnot(startPos);
                spline.Add(startKnot);
                // Set the spline's end knot.
                Vector3 toLinkVector = linkedPoint != null ? linkedPoint.transform.position - 
                    point.transform.position : Vector3.zero;
                float3 endPos = new float3(toLinkVector.x, toLinkVector.y, toLinkVector.z);
                BezierKnot endKnot = new BezierKnot(endPos);
                spline.Add(endKnot);

                // Add the spline.
                splineCont.Spline = spline;
                splinesProp.GetArrayElementAtIndex(i).objectReferenceValue = splineCont;
                nextPoints.GetArrayElementAtIndex(i).objectReferenceValue = linkedPoint;
            }
        }

        /// <summary>
        /// Updates the spline's end point based on the world position of the spline's end point.
        /// </summary>
        /// <param name="endWorldPos"></param>
        private void SetSplineEndPoint(SplineContainer spline, Vector3 endWorldPos)
        {
            if (spline == null) { return; }
            int knotIndex = spline.Spline.Knots.Count() - 1;
            BezierKnot endKnot = spline.Spline.Knots.ElementAt(knotIndex);
            Vector3 toLinkVector = endWorldPos - spline.transform.position;
            float3 endPos = new float3(toLinkVector.x, toLinkVector.y, toLinkVector.z);
            endKnot.Position = endPos;
            spline.Spline.SetKnot(knotIndex, endKnot);
            EditorUtility.SetDirty(spline);
        }

        /// <summary>
        /// Checks if there are other points that reference this node.
        /// </summary>
        /// <param name="point">The point to check for duplicates of.</param>
        /// <returns></returns>
        private bool CheckIsDuplicate(NodePoint point, DarkScaryNode node)
        {
            return GetAllNodePointsInScene().Any(x => x.Node == node && x != point );
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
                nodes.AddRange(root.GetComponentsInChildren<NodePoint>().Where(x => !x.IsDuplicate));
            }
            return nodes;
        }
    }
}
