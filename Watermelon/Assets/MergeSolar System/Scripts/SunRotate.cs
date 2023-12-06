using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunRotate : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] Vector3 rotate;
    [SerializeField] float speed;

    [SerializeField] GameObject backGround;

    private void Start()
    {
        backGround.transform.rotation = Quaternion.Euler(0, 0, Random.Range(-180f, 180f));
        transform.position = new Vector3(Random.Range(-1.5f, 1.5f), transform.position.y, transform.position.z);
    }
    void Update()
    {
        transform.Rotate(rotate, Time.deltaTime * speed);
    }
}
