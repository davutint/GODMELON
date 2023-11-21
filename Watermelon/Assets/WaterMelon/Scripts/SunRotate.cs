using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotate : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Vector3 rotate;
    [SerializeField] float speed;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotate, Time.deltaTime * speed);
    }
}
