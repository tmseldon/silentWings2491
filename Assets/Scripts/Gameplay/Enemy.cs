using Game.Core;
using Game.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class Enemy : MonoBehaviour, IGoBackToPool
    {
        [SerializeField] AudioClip _shootSoundFX;

        private float _enemyHealth, _timeAttack, _bulletSpeed = 1f;
        private float _shootCounter = 0f;
        private GameManager _gameController;
        private AudioSource _enemyAudioControl;

        public void SetEnemyAttributes(float newHealth, float timeToAttack, float speedAttack)
        {
            _enemyHealth = newHealth;
            _timeAttack = timeToAttack;
            _bulletSpeed = speedAttack;

            SetShootTimer();
        }

        private void OnEnable()
        {
            _gameController.StopGame += GoBackToPool;
        }

        private void OnDisable()
        {
            _gameController.StopGame -= GoBackToPool;
        }

        private void Awake()
        {
            _gameController = FindObjectOfType<GameManager>();
            _enemyAudioControl = GetComponent<AudioSource>();
            _enemyAudioControl.clip = _shootSoundFX;
        }

        private void Update()
        {
            CountDownAndShoot();
        }

        private void SetShootTimer()
        {
            _shootCounter = UnityEngine.Random.Range(_timeAttack / 2, _timeAttack);
        }

        private void CountDownAndShoot()
        {
            _shootCounter -= Time.deltaTime;
            if (_shootCounter <= 0)
            {
                Fire();
                SetShootTimer();
            }
        }

        private void Fire()
        {
            GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject(AirplanesTypes.bullet);
            bullet.transform.position = transform.position;
            bullet.GetComponent<Rigidbody>().velocity = new Vector3(0f, 0f, -1*_bulletSpeed);

            _enemyAudioControl.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 6) 
            {
                _enemyHealth--;
                if( _enemyHealth <= 0 )
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            //FX explotion


            GoBackToPool();
        }

        public void GoBackToPool()
        {
            ObjectPooler.SharedInstance.ReturnObjectToPool(GetComponent<EnemyMovement>().ThisPlane, gameObject);
        }
    }
}