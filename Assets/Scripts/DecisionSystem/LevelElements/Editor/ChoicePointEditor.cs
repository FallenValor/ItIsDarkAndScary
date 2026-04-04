/*****************************************************************************
// File Name : ChoicePointEditor.cs
// Author : Brandon Koederitz
// Creation Date : 4/4/2026
// Last Modified : 4/4/2026
//
// Brief Description : Custom editor for ChoicePoints that controls easy assignment of a specific choice.
*****************************************************************************/
using System;
using UnityEditor;
using UnityEngine;

namespace IDAS.Decisions.Editors
{
    [CustomEditor(typeof(ChoicePoint))]
    public class ChoicePointEditor : Editor
    {
        #region CONSTS
        private const string CHOICE_TOOLTIP = "The choice that this ChoicePoint " +
                        "controls the position of.  WARNING: if a choice is both moved and renamed at once, " +
                        "this ChoicePoint will break.";
        #endregion

        private int selectionIndex;
        private string selectionName;

        // Serialized Properties
        private SerializedProperty choiceIndex;
        private SerializedProperty choiceName;

        /// <summary>
        /// Initialize SerializedProperties
        /// </summary>
        public void OnEnable()
        {
            choiceIndex = serializedObject.FindProperty(nameof(choiceIndex));
            choiceName = serializedObject.FindProperty(nameof(choiceName));
        }

        /// <summary>
        /// Draw the NodePoint editor.
        /// </summary>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            ChoicePoint point = (ChoicePoint)target;

            // Draw the dropdown for the choice index/name.
            if (point.ParentNode != null && point.ParentNode.Node is DecisionNodeBase dNode)
            {
                if (dNode.Choices.Length > 0)
                {
                    selectionIndex = choiceIndex.intValue;
                    selectionName = choiceName.stringValue;

                    // Clamp the index to ensure it's within the choice array bounds.
                    selectionIndex = Mathf.Clamp(selectionIndex, 0, dNode.Choices.Length);

                    // Check for index and name match
                    if (dNode.Choices[selectionIndex].Name != selectionName)
                    {
                        // First, try and find a choice with a matching name.
                        int movedChoiceIndex = Array.FindIndex(dNode.Choices, n => n.Name == selectionName);
                        if (movedChoiceIndex != -1)
                        {
                            selectionIndex = movedChoiceIndex;
                        }

                        // Update the name field.
                        selectionName = dNode.Choices[selectionIndex].Name;
                    }

                    // Show popup.
                    EditorGUI.BeginChangeCheck();
                    GUIContent popupContent = new GUIContent("Choice", CHOICE_TOOLTIP);
                    selectionIndex = EditorGUILayout.Popup(popupContent, selectionIndex, GetChoiceNamesFormatted(dNode));
                    if (EditorGUI.EndChangeCheck())
                    {
                        // Update the string and index fields.
                        selectionName = dNode.Choices[selectionIndex].Name;
                    }

                    // Update serialized property values.
                    if (choiceIndex.intValue != selectionIndex)
                    {
                        choiceIndex.intValue = selectionIndex;
                    }
                    if (choiceName.stringValue != selectionName)
                    {
                        choiceName.stringValue = selectionName;
                    }
                    
                    // Draw PropertyFields for index and name.
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(choiceIndex);
                    EditorGUILayout.PropertyField(choiceName);
                    GUI.enabled = true;

                }
                else
                {
                    EditorGUILayout.HelpBox($"DecisionNode {dNode.name} has no choices.", MessageType.Warning);
                }
                
            }
            else
            {
                EditorGUILayout.HelpBox($"This ChoiceNode must be a child of a NodePoint that corresponds to a " +
                    $"DecisionNode", MessageType.Error);
            }

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        /// Gets the names of all nodes in the decision tree.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private string[] GetChoiceNamesFormatted(DecisionNodeBase node)
        {
            string[] choiceNames = new string[node.Choices.Length];
            for(int i = 0; i < choiceNames.Length; i++)
            {
                choiceNames[i] = node.Choices[i].Name + " (" + i + ")";
            }
            return choiceNames;
        }
    }
}
