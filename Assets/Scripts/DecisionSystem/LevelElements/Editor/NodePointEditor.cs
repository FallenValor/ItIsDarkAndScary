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
using Unity.VisualScripting;
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
        private bool showChoicePoints;

        // Serialized Properties
        private SerializedProperty tree;
        private SerializedProperty node;

        private SerializedProperty associatedItem;

        private SerializedProperty choicePoints;

        private SerializedProperty splines;
        private SerializedProperty nextPoints;

        private SerializedProperty oldNodeName;
        private SerializedProperty isDuplicate;

        private SerializedProperty cCam;

        private SerializedProperty NodeVisitedEvent;

        /// <summary>
        /// Initialize SerializedProperties
        /// </summary>
        public void OnEnable()
        {
            tree = serializedObject.FindProperty(nameof(tree));
            node = serializedObject.FindProperty(nameof(node));
            associatedItem = serializedObject.FindProperty(nameof(associatedItem));

            choicePoints = serializedObject.FindProperty(nameof(choicePoints));

            splines = serializedObject.FindProperty(nameof(splines));
            nextPoints = serializedObject.FindProperty(nameof(nextPoints));

            oldNodeName = serializedObject.FindProperty(nameof(oldNodeName));
            isDuplicate = serializedObject.FindProperty(nameof(isDuplicate));

            cCam = serializedObject.FindProperty(nameof(cCam));
            NodeVisitedEvent = serializedObject.FindProperty(nameof(NodeVisitedEvent));
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

            if (point.Tree != null)
            {
                DarkScaryNode[] nodes = point.Tree.nodes.Select(n => n as DarkScaryNode)
                    .Where(n => n is not LinkingInNode && n is not LinkingOutNode).ToArray();

                // Checks for initialization.
                if (!initialized)
                {
                    Initialize(point, nodes);
                }

                string[] nodeNames = nodes.Select(n => n.name).ToArray();
                UpdateSelectionIndex(node, nodes);

                DrawNodeSelector(point, nodeNames);

                if (!point.IsDuplicate)
                {
                    DrawChoicePoints(point);

                    DrawItems(point);

                    DrawConnections(point, splines);
                }

                EditorGUILayout.Space(10);
                EditorGUILayout.PropertyField(NodeVisitedEvent);
            }

            //Show components if null.
            if (cCam.objectReferenceValue == null)
            {
                EditorGUILayout.PropertyField(cCam);
            }
               
            serializedObject.ApplyModifiedProperties();
        }

        #region Draw Funcions
        private void DrawNodeSelector(NodePoint point, string[] nodeNames)
        {
            // Display error text if the node point has an invalid name.
            if (oldNodeName.stringValue != node.objectReferenceValue.name)
            {
                EditorGUILayout.Space();
                EditorGUILayout.HelpBox($"Old node reference was deleted.  " +
                    $"Old Node Name: {oldNodeName.stringValue}", MessageType.Warning);
                // Update the splines for this node to another node.
                if (GUILayout.Button("Update Name"))
                {
                    UpdateName(point.Node.name, point);
                }
            }

            // Show the popup for choosing a name of a node.
            EditorGUI.BeginChangeCheck();
            selectionIndex = EditorGUILayout.Popup("Node Selector", selectionIndex, nodeNames);
            if (EditorGUI.EndChangeCheck())
            {
                // Update the string field.
                DarkScaryNode newNode = point.Tree.nodes[selectionIndex] as DarkScaryNode;
                node.objectReferenceValue = newNode;
                UpdateName(newNode.name, point);

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

            // Draw readonly debug fields.
            GUI.enabled = false;
            EditorGUILayout.PropertyField(node);
            GUI.enabled = true;
        }

        private void DrawChoicePoints(NodePoint point)
        {
            // Draw Choice Points
            if (point.Node is DecisionNode decisionNode && choicePoints.isArray)
            {
                EditorGUILayout.Space(10);
                showChoicePoints = EditorGUILayout.Foldout(showChoicePoints, "Choice Points", EditorStyles.boldFont);
                if (showChoicePoints)
                {
                    EditorGUI.indentLevel++;

                    // Set the size of the choice points array.
                    choicePoints.arraySize = decisionNode.Choices.Length;

                    // Draw each choice point element.
                    for (int i = 0; i < choicePoints.arraySize; i++)
                    {
                        EditorGUILayout.PropertyField(choicePoints.GetArrayElementAtIndex(i),
                            new GUIContent(decisionNode.Choices[i].Name));
                    }

                    EditorGUI.indentLevel--;
                }
            }
        }

        private void DrawConnections(NodePoint point, SerializedProperty splinesProp)
        {
            // Show buttons for spline management.
            if (point.HasSplines)
            {
                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("Splines", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;

                GUI.enabled = false;
                EditorGUILayout.PropertyField(nextPoints);
                EditorGUILayout.PropertyField(splinesProp);
                GUI.enabled = true;
                // Update the splines for this node to another node.
                if (GUILayout.Button("Create Splines"))
                {
                    CreateNodeSplines(point, splinesProp, nextPoints);
                }
                // Update the splines for this node to another node.
                if (GUILayout.Button("Update Spline End Points"))
                {
                    UpdateSplineEndPoints(point);
                }

                EditorGUI.indentLevel--;
            }
            else if (splinesProp.arraySize > 0)
            {
                // Display a button to clear splines if the node should have no splines.
                if (GUILayout.Button("Clear Splines"))
                {
                    ClearSplines(splinesProp, nextPoints);
                }
            }
        }

        private void DrawItems(NodePoint point)
        {
            // Draw an item field if this is an item node.
            if (point.Item != Items.ItemID.None)
            {

                EditorGUILayout.Space(10);
                EditorGUILayout.LabelField("Items", EditorStyles.boldLabel);
                EditorGUI.indentLevel++;
                EditorGUILayout.LabelField($"Item ID: {point.Item}");
                EditorGUILayout.PropertyField(associatedItem);
                EditorGUI.indentLevel--;
            }
        }
        #endregion

        /// <summary>
        /// Updates the point's name.
        /// </summary>
        /// <param name="name">The name of the node this point connects to.</param>
        /// <param name="point">The point to update the name of.</param>
        private void UpdateName(string name, NodePoint point)
        {
            oldNodeName.stringValue = name;

            // Update the node's name.
            point.gameObject.name = nameof(NodePoint) + " (" + point.Tree.nodes[selectionIndex].name + ")";
        }

        /// <summary>
        /// Controls functions that hsould only be called once the gui starts rendering.
        /// </summary>
        private void Initialize(NodePoint point, DarkScaryNode[] nodes)
        {
            UpdateSelectionIndex(node, nodes);

            // Verify the node is unique.
            isDuplicate.boolValue = CheckIsDuplicate(point, point.Node);
        }

        private void UpdateSelectionIndex(SerializedProperty nodeProp, DarkScaryNode[] nodes)
        {
            selectionIndex = GetSelectionIndex(nodeProp, nodes);
            // If no node is selected, then set it to the first node.
            if (selectionIndex == -1)
            {
                // Do not update the OldNodeName property.
                selectionIndex = 0;
                nodeProp.objectReferenceValue = nodes[selectionIndex];
            }
        }

        /// <summary>
        /// Gets the index of the current stored node name value.
        /// </summary>
        /// <param name="nodeProp">The SerializedProperty storing the name of the node this point represents.</param>
        /// <param name="nodes">The string of all node names in the DecisionTree.</param>
        /// <returns>The index of the current node name.</returns>
        private static int GetSelectionIndex(SerializedProperty nodeProp, DarkScaryNode[] nodes)
        {
            DarkScaryNode node = nodeProp.objectReferenceValue as DarkScaryNode;

            if (node == null)
            {
                return -1;
            }

            return Array.IndexOf(nodes, node);
        }

        #region Utilities
        /// <summary>
        /// Converts an array SerializedProperty of object values into a normal array.
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        private static T[] PropertyToArray<T>(SerializedProperty property) where T : class
        {
            if (!property.isArray) { return null; }
            T[] arr = new T[property.arraySize];
            for (int i = 0; i < property.arraySize; i++)
            {
                arr[i] = property.GetArrayElementAtIndex(i).objectReferenceValue as T;
            }
            return arr;
        }

        /// <summary>
        /// Checks if there are other points that reference this node.
        /// </summary>
        /// <param name="point">The point to check for duplicates of.</param>
        /// <returns></returns>
        private static bool CheckIsDuplicate(NodePoint point, DarkScaryNode node)
        {
            return GetAllNodePointsInScene().Any(x => x.Node == node && x != point);
        }

        /// <summary>
        /// Gets all node points in the current scene.
        /// </summary>
        /// <returns>A list of all node points in the current scene.</returns>
        private static List<NodePoint> GetAllNodePointsInScene()
        {
            List<NodePoint> nodes = new List<NodePoint>();
            GameObject[] roots = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var root in roots)
            {
                nodes.AddRange(root.GetComponentsInChildren<NodePoint>().Where(x => !x.IsDuplicate));
            }
            return nodes;
        }
        #endregion

        #region Splines
        /// <summary>
        /// Automatically links this node to it's transition nodes with a cinemachine spline.
        /// </summary>
        /// <param name="point">The node point to update splines for.</param>
        private void CreateNodeSplines(NodePoint point, SerializedProperty splinesProp, 
            SerializedProperty nextPointsProp)
        {
            // Store existing spline and point arrays.
            SplineContainer[] oldSplines = PropertyToArray<SplineContainer>(splinesProp);
            NodePoint[] oldNextPoints = PropertyToArray<NodePoint>(nextPointsProp);

            // Find the other NodePoints in the scene.
            List<NodePoint> points = GetAllNodePointsInScene();
            DarkScaryNode[] nextNodes = point.Node.GetAllNextNodes();

            List<SplineContainer> toDelete = oldSplines.ToList();

            splinesProp.ClearArray();
            nextPointsProp.ClearArray();

            splinesProp.arraySize = nextNodes.Length;
            nextPointsProp.arraySize = nextNodes.Length;
            // Create new splines.
            for (int i = 0; i < nextNodes.Length; i++)
            {
                if (nextNodes[i] == null) { continue; }
                // Find the corresponding node point for this node.
                NodePoint linkedPoint = points.Find(x => x.Node == nextNodes[i]);

                // Check for an existing spline.  If one exists, just update it's end point and leave.
                int index = Array.IndexOf(oldNextPoints, linkedPoint);
                if (index > -1)
                {
                    SplineContainer existingSpline = oldSplines[index];
                    if (existingSpline != null)
                    {
                        SetSplineEndPoint(existingSpline, linkedPoint.transform.position);
                        splinesProp.GetArrayElementAtIndex(i).objectReferenceValue = existingSpline;
                        nextPointsProp.GetArrayElementAtIndex(i).objectReferenceValue = linkedPoint;
                        toDelete.Remove(existingSpline);
                        continue;
                    }
                }

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
                nextPointsProp.GetArrayElementAtIndex(i).objectReferenceValue = linkedPoint;
            }

            // Clear unused splines.
            for (int i = 0; i < toDelete.Count; i++)
            {
                if (toDelete[i] == null) { continue; }
                DestroyImmediate(toDelete[i].gameObject);
            }
        }

        /// <summary>
        /// Clears all splines that extend from a given point point.
        /// </summary>
        /// <param name="splinesProp">The SerializedProperty of the splines array.</param>
        /// <param name="nextPointsProp">The SerializedProperty for the next nodes array.</param>
        private void ClearSplines(SerializedProperty splinesProp, SerializedProperty nextPointsProp)
        {
            SplineContainer[] splines = PropertyToArray<SplineContainer>(splinesProp);
            foreach (SplineContainer spline in splines)
            {
                if (spline == null) { continue; }
                DestroyImmediate(spline.gameObject);
            }

            splinesProp.ClearArray();
            nextPointsProp.ClearArray();
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
            foreach (var p in allInPoints)
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
        #endregion
    }
}
