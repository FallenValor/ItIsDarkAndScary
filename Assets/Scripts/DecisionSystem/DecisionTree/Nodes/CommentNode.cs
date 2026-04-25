/*****************************************************************************
// File Name : CommentNode.cs
// Author : Brandon Koederitz
// Creation Date : 4/23/2026
// Last Modified : 4/23/2026
//
// Brief Description : Simple node for adding programmer comments.
*****************************************************************************/
using UnityEngine;
using XNode;

namespace IDAS
{
    [NodeTint("#fae3a2")]
    public class CommentNode : Node
    {
        [SerializeField, TextArea] private string comment;
    }
}
