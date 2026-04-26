/*****************************************************************************
// File Name : SplineHelpersWindow.cs
// Author : Brandon Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Custom editor window to help with automating splines.
*****************************************************************************/
using Codice.Client.BaseCommands.Differences;
using IDAS.Decisions;
using IDAS.Decisions.Editors;
using IDAS.Decisions.Tree;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IDAS.Editor
{
    public class NodePointHelpers : EditorWindow
    {
        [SerializeField] private DecisionTree checkedTree;

        [MenuItem("Window/Node Point Helpers")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<NodePointHelpers>("Node Point Helpers");
        }

        private void OnGUI()
        {
            // Update the splines for this node to another node.
            if (GUILayout.Button("Create All Splines"))
            {
                CreateAllSplines();
            }
            // Update the splines for this node to another node.
            if (GUILayout.Button("Update All Spline End Points"))
            {
                UpdateAllSplineEndPoints();
            }
            if (GUILayout.Button("Auto Assign Choice Points"))
            {
                AutoAssignAllChoicePoints(false);
            }
            // Update the splines for this node to another node.
            if (GUILayout.Button("Auto Assign Choice Points (Override)"))
            {
                AutoAssignAllChoicePoints(true);
            }
            // Update the splines for this node to another node.
            if (GUILayout.Button("Clear Choice Points"))
            {
                ClearAllChoicePoints();
            }
            checkedTree = (DecisionTree)EditorGUILayout.ObjectField("Checked Tree", checkedTree, typeof(DecisionTree), true);
            // Update the splines for this node to another node.
            if (GUILayout.Button("Check Points"))
            {
                CheckChoicePoints(checkedTree);
            }
            // Update the splines for this node to another node.
            if (GUILayout.Button("Fix Duplicates"))
            {
                FixDuplicates();
            }
        }

        private void CreateAllSplines()
        {
            List<NodePoint> allPoints = NodePointEditor.GetAllNodePointsInScene();
            List<SerializedObject> serialOs = allPoints.Select(n => new SerializedObject(n)).ToList();
            for (int i = 0; i < serialOs.Count; i++)
            {
                SerializedProperty splinesProp = serialOs[i].FindProperty("splines");
                SerializedProperty nextPointsProp = serialOs[i].FindProperty("nextPoints");
                NodePointEditor.CreateNodeSplines(allPoints[i], splinesProp, nextPointsProp);
                serialOs[i].ApplyModifiedProperties();
            }

        }

        private void UpdateAllSplineEndPoints()
        {
            List<NodePoint> allPoints = NodePointEditor.GetAllNodePointsInScene();
            for (int i = 0; i < allPoints.Count; i++)
            {
                NodePointEditor.UpdateSplineEndPoints(allPoints[i]);
            }
        }

        private void AutoAssignAllChoicePoints(bool overwrite)
        {
            List<NodePoint> allPoints = NodePointEditor.GetAllNodePointsInScene();
            List<SerializedObject> serialOs = allPoints.Select(n => new SerializedObject(n)).ToList();
            for (int i = 0; i < serialOs.Count; i++)
            {
                if (allPoints[i].Node is DecisionNode decisionNode)
                {
                    SerializedProperty pointsProp = serialOs[i].FindProperty("choicePoints");
                    if (pointsProp.isArray)
                    {
                        pointsProp.arraySize = decisionNode.Choices.Length;
                        NodePointEditor.AutoAssignChoicePoints(allPoints[i], pointsProp, overwrite);
                        serialOs[i].ApplyModifiedProperties();
                    }
                }
                
            }
        }

        private void ClearAllChoicePoints()
        {
            List<NodePoint> allPoints = NodePointEditor.GetAllNodePointsInScene();
            List<SerializedObject> serialOs = allPoints.Select(n => new SerializedObject(n)).ToList();
            for (int i = 0; i < serialOs.Count; i++)
            {
                if (allPoints[i].Node is DecisionNode decisionNode)
                {
                    SerializedProperty pointsProp = serialOs[i].FindProperty("choicePoints");
                    if (pointsProp.isArray)
                    {
                        pointsProp.arraySize = decisionNode.Choices.Length;
                        NodePointEditor.ClearChoicePoints(allPoints[i], pointsProp);
                        serialOs[i].ApplyModifiedProperties();
                    }
                }

            }
        }

        private void CheckChoicePoints(DecisionTree toCheck)
        {
            DarkScaryNode[] nodes = toCheck.nodes.Where(n => n is DarkScaryNode).Select(n => n as DarkScaryNode)
                    .Where(n => n is not LinkingInNode && n is not LinkingOutNode && n != null).ToArray();
            List<NodePoint> allPoints = NodePointEditor.GetAllNodePointsInScene();
            bool isMissing = false;
            for (int i = 0; i < nodes.Length; i++)
            {
                if (!allPoints.Any(n => n.Node == nodes[i]))
                {
                    Debug.LogWarning($"This scene is missing a point for the node: {nodes[i]}");
                    isMissing |= true;
                }
            }

            if (!isMissing)
            {
                Debug.Log($"This level has points for every node in the tree {toCheck.name}.");
            }
        }

        private void FixDuplicates()
        {
            List<NodePoint> allPoints = NodePointEditor.GetAllNodePointsInScene();
            List<SerializedObject> serialOs = allPoints.Select(n => new SerializedObject(n)).ToList();
            for (int i = 0; i < serialOs.Count; i++)
            {
                SerializedProperty isDupe = serialOs[i].FindProperty("isDuplicate");
                isDupe.boolValue = NodePointEditor.CheckIsDuplicate(allPoints[i], allPoints[i].Node);
                serialOs[i].ApplyModifiedProperties();
            }
            Debug.Log("Fixed all IsDuplicate flags.");
        }
    }
}
