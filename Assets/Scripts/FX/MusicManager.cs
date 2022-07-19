using Game.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.FX
{
    public class MusicManager : MonoBehaviour
    {
        private AudioSource _thisAudioSource;
        private GameManager _gameManager;

        private void Awake()
        {
            _thisAudioSource = GetComponent<AudioSource>();
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void OnEnable()
        {
            _gameManager.StartGame += StartMusic;
            _gameManager.StopGame += StopMusic;
        }

        private void OnDisable()
        {
            _gameManager.StartGame -= StartMusic;
            _gameManager.StopGame -= StopMusic;
        }

        private void StopMusic()
        {
            _thisAudioSource.Stop();
        }

        private void StartMusic()
        {
            _thisAudioSource.Play();
        }
    }

}