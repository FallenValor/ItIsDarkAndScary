/*****************************************************************************
// File Name : SplineHelpersWindow.cs
// Author : Brandon Koederitz
// Creation Date : 4/26/2026
// Last Modified : 4/26/2026
//
// Brief Description : Custom editor window to help with automating splines.
*****************************************************************************/
using IDAS.Decisions;
using IDAS.Decisions.Editors;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IDAS.Editor
{
    public class NodePointHelpers : EditorWindow
    {
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

    }
}
