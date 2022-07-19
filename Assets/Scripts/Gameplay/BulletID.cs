using Game.Core;
using Game.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Gameplay
{
    public class BulletID : MonoBehaviour, IGoBackToPool
    {
        [SerializeField] AirplanesTypes _tipoBullet;
        private GameManager _gameManager;

        public AirplanesTypes TipoBullet { get { return _tipoBullet; } }

        private void Awake()
        {
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void OnEnable()
        {
            _gameManager.StopGame += GoBackToPool;
        }

        private void OnDisable()
        {
            _gameManager.StopGame -= GoBackToPool;
        }

        public void GoBackToPool()
        {
            ObjectPooler.SharedInstance.ReturnObjectToPool(_tipoBullet, gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == 6)
            {
                GoBackToPool();
            }
        }
    }
}