/*****************************************************************************
// File Name : UIService.cs
// Author : Brandon Koederitz
// Creation Date : 4/6/2026
// Last Modified : 4/6/2026
//
// Brief Description : Base class for services that manage the UI.
*****************************************************************************/
namespace IDAS.UI
{
    public class UIService : Service
    {
        #region Properties
        protected UIManager UIManager => Manager as UIManager;
        #endregion
    }
}
