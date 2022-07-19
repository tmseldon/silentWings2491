using Game.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    [CreateAssetMenu(fileName = "WaveConfig", menuName = "Waves/WaveConfig", order = 1)]
    public class WaveConfig: ScriptableObject
    {
        [Header("Main Data")]
        [SerializeField] GameObject[] _listPaths;
        [SerializeField] ProgressionsLevels _levelsData;
        [SerializeField] AirplanesTypes _typeAirplane;
        [Range(1, 4)]
        [SerializeField] int _levelAirplane = 1;

        [Header("Wave Config")]
        [SerializeField] bool _isWaypointBased = true;
        [SerializeField] int _numberEnemies = 1;
        [SerializeField] float _timeBetweenSpawn;
        [Range(0f, 1f)]
        [SerializeField] float _rangeRandomtime = 0.15f;

        private float _modifiedTimeBetweenSpawns = 0.5f;

        public bool IsWaypointBased { get { return _isWaypointBased; }}

        public float NumberEnemiesInWave { get { return _numberEnemies; }}

        public float ModifiedTimeBetweenSpawns {get { return _modifiedTimeBetweenSpawns; }}

        private void OnEnable()
        {
            TimeToSpawn();
        }

        private void TimeToSpawn()
        {
            float randomness = Random.Range(-_rangeRandomtime, _rangeRandomtime);
            _modifiedTimeBetweenSpawns = _timeBetweenSpawn * (1 + randomness);
        }

        public List<Transform> GetWayPoints()
        {
            //if(!_isWaypointBased) { return null; }
            
            int indexRandom = Random.Range(0, _listPaths.Length);
            GameObject selectedPath = _listPaths[indexRandom];
            List<Transform> wayPoints = new List<Transform>();

            foreach(Transform childTransform in selectedPath.transform)
            {
                wayPoints.Add(childTransform);
            }

            return wayPoints;
        }

        public AirplanesTypes GetAirplaneInfo(out float health, out float moveSpeed, 
                                                out float timeToAttack, out float speedAttack)
        {
            health = _levelsData.GetStat(BasicStats.health, _typeAirplane, _levelAirplane);
            moveSpeed = _levelsData.GetStat(BasicStats.movementSpeed, _typeAirplane, _levelAirplane);
            timeToAttack = _levelsData.GetStat(BasicStats.timeAttack, _typeAirplane, _levelAirplane);
            speedAttack = _levelsData.GetStat(BasicStats.speedAttack, _typeAirplane, _levelAirplane);

            return _typeAirplane;
        }
    }
}