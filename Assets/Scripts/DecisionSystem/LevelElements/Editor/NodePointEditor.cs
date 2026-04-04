/*****************************************************************************
// File Name : NodePointEditor.cs
// Author : Brandon Koederitz
// Creation Date : 4/4/2026
// Last Modified : 4/4/2026
//
// Brief Description : Custom editor for NodePoints that controls easy assignment of the linked node.
*****************************************************************************/
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

        // Serialized Properties
        private SerializedProperty tree;
        private SerializedProperty node;
        private SerializedProperty oldNodeName;

        /// <summary>
        /// Initialize SerializedProperties
        /// </summary>
        public void OnEnable()
        {
            tree = serializedObject.FindProperty(nameof(tree));
            node = serializedObject.FindProperty(nameof(node));
            oldNodeName = serializedObject.FindProperty(nameof(oldNodeName));
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
                }
                GUI.enabled = false;
                EditorGUILayout.PropertyField(node);
                GUI.enabled = true;
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
    }
}
