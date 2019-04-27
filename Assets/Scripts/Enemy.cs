using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // TODO i am cheap movment code kill me
        var newPos = transform.position + transform.rotation * Vector3.up * Time.deltaTime * speed;
        if ((newPos - transform.parent.position).sqrMagnitude > 20)
        {
            transform.Rotate(Vector3.forward, 90f); 
        }
        else
        {
            transform.position = newPos;
        }
    }
    
    public Vector3 getPosAt(int node)
    {
        var velocity = transform.rotation * Vector3.up * speed;
        return transform.position + velocity * node;
    }
}
