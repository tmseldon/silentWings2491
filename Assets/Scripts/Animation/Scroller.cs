using Game.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Animation
{
    //Landscape Scroller for the level
    //Here is where is calculated the movemnet of the terrain based on time duration of level
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
            //Need params to determine speed of the scroller based on duration (seconds) of the level
            _gameManager = FindObjectOfType<GameManager>();

            _secondsPerRound = _gameManager.SecondsPerRound;
            _initialPos = transform.position;
            _speedVertical = _distanceToMove / _secondsPerRound;

            _lastPos = new Vector3(0f, _initialPos.y, -1 * _distanceToMove);
        }

        //Subscribe to notifications of game's events
        //Based on events it change the scroller status
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

        //Scrollers status
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