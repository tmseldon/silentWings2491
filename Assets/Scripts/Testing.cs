using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testing : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Me llamaron desde Awake");
    }
    void Start()
    {
        Debug.Log("Me llamaron desde Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Debug.Log("Me llamaron desde Enabled");
    }

    private void OnDisable()
    {
        Debug.Log("Me llamaron desde Disable");
    }
}
