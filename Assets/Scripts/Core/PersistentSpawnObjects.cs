using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Game.Core
{
    //Singleton implementation
    //The different systems that should not be destroyed are child objects of _persistantObjectPrefab
    public class PersistentSpawnObjects : MonoBehaviour
    {
        [SerializeField] GameObject _persistentObjectPrefab;

        static bool _hasSpawned = false;
        private void Awake()
        {
            if (_hasSpawned) return;

            SpawnPersistantObjects();
            _hasSpawned = true;
        }

        private void SpawnPersistantObjects()
        {
            GameObject persistantObject = Instantiate(_persistentObjectPrefab);
            DontDestroyOnLoad(persistantObject);
        }
    }

}