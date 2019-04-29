using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinnyThing : MonoBehaviour
{
    public float speed = 20;
    
    void Update()
    {
        transform.Rotate(Vector3.forward, speed * Time.deltaTime);
    }
}
