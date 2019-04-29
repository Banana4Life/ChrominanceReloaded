using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public ColorVariant color;
    public ParticleSystem ps;
    public float hovered;


    // Start is called before the first frame update
    void Start()
    {
        var main = ps.main;
        main.startColor= new ParticleSystem.MinMaxGradient(color.gradient);
    }

    // Update is called once per frame
    void Update()
    {
        hovered -= Time.deltaTime;
        if (hovered > 0)
        {
            var psShape = ps.shape;
            psShape.radius = Mathf.Min(psShape.radius + Time.deltaTime * 4, 1);
            var psEmission = ps.emission;
            psEmission.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Min(psEmission.rateOverTime.constant + Time.deltaTime * 2000, 1000));
            
        }
        else
        {
            var psShape = ps.shape;
            psShape.radius = Mathf.Max(psShape.radius - Time.deltaTime *3, 0.1f);
            var psEmission = ps.emission;
            psEmission.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Max(psEmission.rateOverTime.constant - Time.deltaTime * 1000, 100));
        }
    }
}
