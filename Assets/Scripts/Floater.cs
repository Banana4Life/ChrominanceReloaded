using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public ColorVariant color;
    public ParticleSystem ps;
   
    
    // Start is called before the first frame update
    void Start()
    {
        var main = ps.main;
        main.startColor= new ParticleSystem.MinMaxGradient(color.gradient);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
