using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Animation
{
    public class Oscillate : MonoBehaviour
    {
        [SerializeField] float _speed, _amplitude;
        [SerializeField] bool _randomInitPos = false;

        Vector3 _posInitial;
        Vector3 _addPosLocal = new Vector3(0, 0, 0);
        float _time = 0;

        // Start is called before the first frame update
        void Start()
        {
            _posInitial = transform.position;

            //in case that initial position needed to be random
            if (_randomInitPos)
            {
                _time = Random.Range(0f, 55f);
            }
        }

        void Update()
        {
            Vibrar();
        }

        //SIN Oscillation based on parameters
        private void Vibrar()
        {
            _addPosLocal.y = Mathf.Sin(_time * _speed) * _amplitude;
            _time += Time.deltaTime;
            transform.position = _addPosLocal + _posInitial;
        }
    }
}