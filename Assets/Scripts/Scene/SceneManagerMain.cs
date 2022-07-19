using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.SceneControl
{
    public class SceneManagerMain : MonoBehaviour
    {
        [SerializeField] string _firstLevelName;

        private const string MAIN_MENU = "MainMenu";

        public void GoToMain() => SceneManager.LoadScene(MAIN_MENU);
        public void GoToFirstLevel() => SceneManager.LoadScene(_firstLevelName);
        public void QuitGame() => Application.Quit();
    }
}

