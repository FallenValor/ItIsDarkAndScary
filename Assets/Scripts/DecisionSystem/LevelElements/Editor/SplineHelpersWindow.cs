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
    public class SplineHelpersWindow : EditorWindow
    {
        [MenuItem("Window/Spline Helpers")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow<SplineHelpersWindow>("Spline Helpers");
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
    }
}
