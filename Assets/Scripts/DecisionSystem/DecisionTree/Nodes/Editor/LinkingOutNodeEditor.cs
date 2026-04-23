/*****************************************************************************
// File Name : LinkingOutNodeEditor.cs
// Author : Brandon Koederitz
// Creation Date : 4/23/2026
// Last Modified : 4/23/2026
//
// Brief Description : Custom editor for LinkingOutNodes to hide certain fields.
*****************************************************************************/
using UnityEngine;
using XNodeEditor;

namespace IDAS
{
    [CustomNodeEditor(typeof(LinkingOutNode))]
    public class LinkingOutNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("outputChoice"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}
