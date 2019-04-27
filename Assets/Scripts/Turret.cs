using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public Sprite turretHead;
    public GameObject head;
    
    [Range(0.0f,360.0f)]
    public float headAngle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var headrenderer = head.GetComponentInChildren<SpriteRenderer>();
        headrenderer.sprite = turretHead;
        head.transform.rotation = Quaternion.Euler(0, 0, headAngle);
        headAngle += Time.deltaTime * 50;
    }
}
