/*****************************************************************************
// File Name : DecisionService.cs
// Author : Brandon Koederitz
// Creation Date : 4/1/2026
// Last Modified : 4/1/2026
//
// Brief Description : Base class for services within the decision manager.
*****************************************************************************/
using UnityEngine;

public class DecisionService : Service
{
    #region Properties
    protected DecisionManager DecisionManager => Manager as DecisionManager;
    #endregion


}
