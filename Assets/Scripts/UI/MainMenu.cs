/***********************************************************************
// File Name : MainMenu.cs
// Author : Brandon Koederitz
// Creation Date : 4/15/2026
// Last Modified : 4/15/2026
//
// Brief Description : Functionality for main menu buttons.
*****************************************************************************/
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IDAS.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField, Scene] private string startingScene;

        public void Play()
        {
            SceneManager.LoadScene(startingScene);
        }

        public void Quit()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
