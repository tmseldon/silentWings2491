using Game.Gameplay;
using Game.Stats;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Core
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] WavesByStage[] _wavesInformation;

        private GameManager _gameManager;

        private List<WaveConfig> wavesForLevel = new List<WaveConfig>();
        private int _currentLevel = 1;
        private int _initialWaveIndex = 0;

        public int NumberOfWaves { get { return _wavesInformation.Length; } }

        private void Awake()
        {
            _gameManager = GetComponent<GameManager>();
        }

        //Subscriptions to game's events
        private void OnEnable()
        {
            _gameManager.GoingToNextLevel += UpdateCurrentLevel;
            _gameManager.StartGame += StartSpawning;
            _gameManager.StopGame += StopSpawning;
        }

        private void OnDisable()
        {
            _gameManager.GoingToNextLevel -= UpdateCurrentLevel;
            _gameManager.StartGame -= StartSpawning;
            _gameManager.StopGame -= StopSpawning;
        }

        //Set new level and start spawning enemies
        private void UpdateCurrentLevel(int level)
        {
            _currentLevel = level;
            _initialWaveIndex = 0;
            StartSpawning();
        }

        private void StartSpawning()
        {
            //Build List Waves for this Stage
            wavesForLevel.Clear();
            WavesByStage waveInfo = _wavesInformation[_currentLevel - 1];
            wavesForLevel = waveInfo.WavesThisLevel.ToList();

            //Start Spawning
            StartCoroutine(SpawnAllWaves());
        }

        private IEnumerator SpawnAllWaves()
        {
            for (int currentIndexWave = _initialWaveIndex; currentIndexWave < wavesForLevel.Count; currentIndexWave++)
            {
                WaveConfig currentWave = wavesForLevel[currentIndexWave];
                yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            }

            //Random wave in case of no more waves are available
            do
            {
                int randomWaveIndex = Random.Range(0, wavesForLevel.Count);
                yield return StartCoroutine(SpawnAllEnemiesInWave(wavesForLevel[randomWaveIndex]));
            }
            while (!_gameManager.IsGameOver);
        }

        private IEnumerator SpawnAllEnemiesInWave(WaveConfig waveConfig)
        {
            float health = 0f;
            float speed = 0f; 
            float timeAttack = 0f;
            float speedAttack = 0f;
            
            //information about the airplane
            AirplanesTypes type = waveConfig.GetAirplaneInfo(out health, out speed, out timeAttack, out speedAttack);

            //list of waypoints for this wave
            List<Transform> positions = waveConfig.GetWayPoints();

            for (int enemyCount = 0; enemyCount < waveConfig.NumberEnemiesInWave; enemyCount++)
            {
                GameObject enemy = ObjectPooler.SharedInstance.GetPooledObject(type);
                enemy.transform.position = positions[0].position;

                EnemyMovement enemyMove = enemy.GetComponent<EnemyMovement>();
                enemyMove.SetParams(waveConfig.IsWaypointBased, type);
                enemyMove.SetWaypoints(positions, speed);
                enemy.GetComponent<Enemy>().SetEnemyAttributes(health, timeAttack, speedAttack);

                yield return new WaitForSeconds(waveConfig.ModifiedTimeBetweenSpawns);
            }
        }

        private void StopSpawning()
        {
            StopAllCoroutines();
        }
    }

    [System.Serializable]
    public class WavesByStage
    {
        public int Level;
        public WaveConfig[] WavesThisLevel;
    }
}