using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    public float health = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            health -= 1;
            if (health < 0)
            {
                Destroy(gameObject);
            }
        }
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy)
        {
            health -= 5;
            if (health < 0)
            {
                Destroy(gameObject);
            }
        }

    }
}
