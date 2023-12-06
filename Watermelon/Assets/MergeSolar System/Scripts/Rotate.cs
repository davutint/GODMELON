using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Vector3 rotate;
    [SerializeField] float speed;

    void Update()
    {
        transform.Rotate(rotate, Time.deltaTime * speed);
    }
}
