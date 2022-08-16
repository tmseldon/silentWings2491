using Game.Core;
using Game.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class EnemyMovement : MonoBehaviour
    {
        private float _speedMovement = 0f;
        private List<Transform> _waypoints = new List<Transform>();
        private int _currentWaypointIndex = 0;
        private bool _isWaypointWhereToMove = true;
        private AirplanesTypes _thisPlane;
        private bool _isHunterSetting = true;

        private GameObject _playerObject;
        private Vector3 _determinePlayerDirection;

        public AirplanesTypes ThisPlane { get { return _thisPlane; } }

        private void Awake()
        {
            _playerObject = GameObject.FindGameObjectWithTag("Player");
        }

        void Update()
        {
            Move();
        }

        public void SetParams(bool isWaypointBased, AirplanesTypes type)
        {
            _isWaypointWhereToMove = isWaypointBased;
            _thisPlane = type;
        }

        public void SetWaypoints(List<Transform> pointsTofollow, float speed)
        {
            _waypoints = pointsTofollow;
            _speedMovement = speed;
        }

        private void Move()
        {

            if(_isWaypointWhereToMove)
            {
                MovementByWaypoints();
            }
            else
            {
                HunterMovement();
            }
        }

        private void HunterMovement()
        {
            if(_isHunterSetting)
            {
                //Randomize initial spawning point
                float randomPoint = UnityEngine.Random.Range(0f, 1f);
                Vector3 appearingPos = Vector3.Lerp(_waypoints[0].position, _waypoints[1].position, randomPoint);
                transform.position = appearingPos;

                //determine Player Pos
                _determinePlayerDirection = (_playerObject.transform.position - appearingPos).normalized;
                transform.rotation = Quaternion.LookRotation(_determinePlayerDirection);
                _isHunterSetting = false;
            }
            //Hunt player based on the _determinePlayerDirection
            //It does not hunt all the time, just based on that position at the spawning moment
            transform.position +=  _determinePlayerDirection * _speedMovement * Time.deltaTime;
        }

        private void MovementByWaypoints()
        {
            if (_currentWaypointIndex <= _waypoints.Count - 1)
            {
                var nextWaypoint = _waypoints[_currentWaypointIndex].position;
                transform.position = Vector3.MoveTowards(transform.position, nextWaypoint, _speedMovement * Time.deltaTime);

                if (transform.position == nextWaypoint)
                {
                    _currentWaypointIndex++;
                }
            }
            else
            {
                _currentWaypointIndex = 0;
                ObjectPooler.SharedInstance.ReturnObjectToPool(_thisPlane, gameObject);
            }
        }
    }
}