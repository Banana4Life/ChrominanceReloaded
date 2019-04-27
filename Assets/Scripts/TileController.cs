using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Tilemap))]
public class TileController : MonoBehaviour
{
    private Tilemap tilemap;
    
    // Start is called before the first frame update
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        Input.GetAxis("x");
        Input.GetAxis("y");
    }
}
