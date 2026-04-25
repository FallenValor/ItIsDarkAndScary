/*****************************************************************************
// File Name : LinkingInNodeEditor.cs
// Author : Brandon Koederitz
// Creation Date : 4/23/2026
// Last Modified : 4/23/2026
//
// Brief Description : Custom editor for LinkingInNodes to hide certain fields.
*****************************************************************************/
using UnityEngine;
using XNodeEditor;

namespace IDAS
{
    [CustomNodeEditor(typeof(LinkingInNode))]
    public class LinkingInNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("inputChoice"));
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("linkedNode"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
