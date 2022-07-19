using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Animation
{
    public class Helices : MonoBehaviour
    {
        [SerializeField] float _angleRotation = 10f;
        [SerializeField] float _speedRotation = 1.5f;

        private Vector3 _rotationVector;

        private void Awake()
        {
            _rotationVector = new Vector3(_angleRotation * _speedRotation, 0f, 0f);
        }

        void Update()
        {
            transform.Rotate(_rotationVector * Time.deltaTime);
        }
    }

}
