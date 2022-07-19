using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Animation
{
    public class Scroller : MonoBehaviour
    {
        [SerializeField] float _distanceToMove = 460f;

        private GameManager _gameManager;
        private float _secondsPerRound;
        private Vector3 _initialPos;
        private Vector3 _lastPos;
        private float _speedVertical;

        private bool _isScrolling = false;
        
        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();

            _secondsPerRound = _gameManager.SecondsPerRound;
            _initialPos = transform.position;
            _speedVertical = _distanceToMove / _secondsPerRound;

            _lastPos = new Vector3(0f, _initialPos.y, -1 * _distanceToMove);
        }

        private void OnEnable()
        {
            _gameManager.StartGame += StartScrolling;
            _gameManager.StopGame += StopScrolling;
            _gameManager.GoingToNextLevel += RestartScrolling;
        }

        private void OnDisable()
        {
            _gameManager.StartGame -= StartScrolling;
            _gameManager.StopGame -= StopScrolling;
            _gameManager.GoingToNextLevel -= RestartScrolling;
        }

        private void Update()
        {
            if(_isScrolling)
            {
                transform.position = Vector3.MoveTowards(transform.position, _lastPos, _speedVertical * Time.deltaTime);
            }
        }

        private void StartScrolling()
        {
            _isScrolling = true; 
        }

        private void StopScrolling()
        {
            _isScrolling = false;
        }

        private void RestartScrolling(int none)
        {
            transform.position = _initialPos;
            StartScrolling();
        }

    }
}