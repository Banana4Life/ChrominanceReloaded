using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 2;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.rotation * Vector3.up * Time.deltaTime * speed;
        if ((transform.position - transform.parent.position).sqrMagnitude > 100)
        {
            Destroy(gameObject);
        }
    }
}
