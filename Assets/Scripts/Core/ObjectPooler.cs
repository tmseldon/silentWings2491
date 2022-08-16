using Game.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] ObjectPoolGroup[] _infoGroups;

        public static ObjectPooler SharedInstance;

        private Dictionary<AirplanesTypes, Queue<GameObject>> _pooledObjects = 
                                                        new Dictionary<AirplanesTypes, Queue<GameObject>>();

        void Awake()
        {
            //Setting up ObjectPooler as static and create pool of objects 
            //based on params
            SharedInstance = this;
            foreach (ObjectPoolGroup group in _infoGroups)
            {
                CreatePoolOfObjects(group);
            }
        }

        private void CreatePoolOfObjects(ObjectPoolGroup group)
        {
            Queue<GameObject> stackObjects = new Queue<GameObject>();

            for(int index = 0; index < group.AmountToPool; index++)
            {
                //randomize objects selection to add diversity
                //this is only model wise
                int randomIndex = UnityEngine.Random.Range(0, group.ObjectsToPool.Length);
                GameObject objectSelected = Instantiate(group.ObjectsToPool[randomIndex]);
                objectSelected.SetActive(false);
                objectSelected.transform.SetParent(group.ParentTransform);
                stackObjects.Enqueue(objectSelected);
            }

            _pooledObjects.Add(group.AirplaneType, stackObjects);
        }

        public GameObject GetPooledObject(AirplanesTypes type)
        {
            if(!_pooledObjects.ContainsKey(type)) { Debug.Log("no hay tipo"); return null; }

            if (_pooledObjects[type].Count > 0)
            {
                GameObject sendThisObject = _pooledObjects[type].Dequeue();
                sendThisObject.SetActive(true);
                
                return sendThisObject;
            }
            else
            {
                Debug.Log("no hay elementos");
                return null;
            }
        }

        public void ReturnObjectToPool(AirplanesTypes type, GameObject objectToPush)
        {
            if (_pooledObjects.ContainsKey(type))
            {
                try
                {
                    objectToPush.SetActive(false);
                    _pooledObjects[type].Enqueue(objectToPush);
                }
                catch(Exception e)
                {
                    Debug.Log(e);
                }
            }
        }
    }

    [System.Serializable]
    public class ObjectPoolGroup
    {
        public AirplanesTypes AirplaneType;
        public int AmountToPool;
        public Transform ParentTransform;
        public GameObject[] ObjectsToPool;
    }
}