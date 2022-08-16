using Game.Gameplay;
using Game.Stats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Core
{
    public class Shredder : MonoBehaviour
    {
        AirplanesTypes typeObject;

        //Shredder deactivate bullets and enemies airplanes when they cross limit regions
        private void OnTriggerEnter(Collider other)
        {    
            if(other.gameObject.CompareTag("Bullet"))
            {
                typeObject = other.gameObject.GetComponent<BulletID>().TipoBullet;
            }
            else
            {
                typeObject = other.gameObject.GetComponent<EnemyMovement>().ThisPlane;
            }
            ObjectPooler.SharedInstance.ReturnObjectToPool(typeObject, other.gameObject);
        }
    }
}