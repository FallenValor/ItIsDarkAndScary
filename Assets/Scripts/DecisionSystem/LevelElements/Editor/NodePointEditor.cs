/*****************************************************************************
// File Name : NodePointEditor.cs
// Author : Brandon Koederitz
// Creation Date : 4/4/2026
// Last Modified : 4/4/2026
//
// Brief Description : Custom editor for NodePoints that controls easy assignment of the linked node.
*****************************************************************************/
using IDAS.Decisions.Tree;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace IDAS.Decisions.Editors
{
    [CustomEditor(typeof(NodePoint))]
    public class NodePointEditor : Editor
    {
        private int selectionIndex;
        private string[] nodes;

        // Serialized Properties
        private SerializedProperty tree;
        private SerializedProperty nodeName;

        /// <summary>
        /// Initialize SerializedProperties
        /// </summary>
        public void OnEnable()
        {
            tree = serializedObject.FindProperty(nameof(tree));
            nodeName = serializedObject.FindProperty(nameof(nodeName));
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
                // Draw the dropdown for the node name.
                string[] nodeNames = GetTreeNodeNames(point.Tree);
                selectionIndex = GetSelectionIndex(nodeName, nodeNames);

                // Display error text if the node point has an invalid name.
                if (selectionIndex == -1)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox($"No node with the name {nodeName.stringValue} exists in the " +
                        $"chosen DecisionTree.", MessageType.Error);
                    EditorGUILayout.PropertyField(nodeName);
                }

                // Show the popup for choosing a name of a node.
                EditorGUI.BeginChangeCheck();
                selectionIndex = EditorGUILayout.Popup("Node Name", selectionIndex, nodeNames);
                if (EditorGUI.EndChangeCheck())
                {
                    // Update the string field.
                    nodeName.stringValue = nodeNames[selectionIndex];
                }
            }
               
            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Gets the names of all nodes in the decision tree.
        /// </summary>
        /// <param name="tree"></param>
        /// <returns></returns>
        private string[] GetTreeNodeNames(DecisionTree tree)
        {
            string[] nodeNames = tree.nodes.Select(n => n.name).ToArray();
            return nodeNames;
        }

        /// <summary>
        /// Gets the index of the current stored node name value.
        /// </summary>
        /// <param name="nameProp">The SerializedProperty storing the name of the node this point represents.</param>
        /// <param name="nodeNames">The string of all node names in the DecisionTree.</param>
        /// <returns>The index of the current node name.</returns>
        private int GetSelectionIndex(SerializedProperty nameProp, string[] nodeNames)
        {
            string currentNodeName = nameProp.stringValue;

            // If no node is selected, then set it to the first node.
            if (currentNodeName == "")
            {
                selectionIndex = 0;
                nodeName.stringValue = nodeNames[selectionIndex];
                return selectionIndex;
            }

            return Array.IndexOf(nodeNames, currentNodeName); 
        }
    }
}
