using Game.Gameplay;
using Game.SceneControl;
using Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] float _secondsPerround = 90f;
        [SerializeField] PerksSelector _perksSelector;
        [SerializeField] Canvas _gameOverScreen;
        [SerializeField] Canvas _endGameScreen;

        public float SecondsPerRound { get { return _secondsPerround; } }

        public event Action<int> GoingToNextLevel;
        public event Action StartGame;
        public event Action StopGame;
        public bool IsGameOver { get { return _isGameOver; } }

        private int _currentLevel = 1;
        private bool _isGameOver = false;
        private Player _playerControl;
        private WaitForSeconds _timerCountDown;
        private SceneManagerMain _sceneManager;
        private EnemySpawner _enemySpawner;

        private void Awake()
        {
            _playerControl = FindObjectOfType<Player>();
            _sceneManager = FindObjectOfType<SceneManagerMain>();
            _enemySpawner = GetComponent<EnemySpawner>();
            _timerCountDown = new WaitForSeconds(_secondsPerround);
            _gameOverScreen.enabled = false;
            _endGameScreen.enabled = false;
        }

        private void OnEnable()
        {
            _playerControl.OnPlayerDeath += GameOver;
        }

        private void OnDisable()
        {
            _playerControl.OnPlayerDeath -= GameOver;
        }

        void Start()
        {
            InitiateWave();
        }

        private void InitiateWave()
        {
            StartGame();
            ToogleSystems(true);
            StartCoroutine(TimerCountDown());
        }

        private IEnumerator TimerCountDown()
        {
            yield return _timerCountDown;
            StopGame();
            if (_currentLevel == _enemySpawner.NumberOfWaves)
            {
                _endGameScreen.enabled = true;
            }
            else
            {
                _perksSelector.TurnOnSkillSelector(true);
            }
            
        }

        public void StartNextLevel()
        {
            _currentLevel++;
            GoingToNextLevel(_currentLevel);
            InitiateWave();
        }
        private void GameOver()
        {
            _gameOverScreen.enabled = true;
            StopAllCoroutines();
            ToogleSystems(false);
            //llama a todos los suscriptores (Spawner y otros?)
            StopGame();
        }

        private void ToogleSystems(bool status)
        {
            if (status)
            {
                Time.timeScale = 1;
                //Cursor.lockState = CursorLockMode.Locked;
                //Cursor.visible = false;
            }
            else
            {
                Time.timeScale = 0;
                //Cursor.lockState = CursorLockMode.None;
                //Cursor.visible = true;
            }
        }

        //Game Status methods

        public void RestartGame()
        {
            ToogleSystems(true);
            _sceneManager.GoToFirstLevel();
        }

        public void BackToMainMenu()
        {
            ToogleSystems(true);
            _sceneManager.GoToMain();
        }
    }
}