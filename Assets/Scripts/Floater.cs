using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    public ColorVariant color;
    public ParticleSystem ps;
    public float hovered;


    private float maxRadius = 1;
    private float minRadius = 0.1f;
    
    private float maxEmission = 1000;
    private float minEmission = 100;

    private float radiusHover;
    private float emissionHover;


    // Start is called before the first frame update
    void Start()
    {
        var main = ps.main;
        main.startColor= new ParticleSystem.MinMaxGradient(color.gradient);
    }

    // Update is called once per frame
    void Update()
    {
        if (name == "Floater(Clone)")
        {
            return;
        }
        var baseController = FindObjectOfType<PlayerBaseController>();

        var amountOfColor = 0f;
        
        switch (color.colorId)
        {
            case 0:
                amountOfColor = baseController.greenAmount;
                break;
            case 1:
                amountOfColor = baseController.redAmount;
                break;
            case 2:
                amountOfColor = baseController.blueAmount;
                break;
        }
        
        var percentSize = amountOfColor / baseController.maxAmount;

        var newRadius = (maxRadius - minRadius) * percentSize + minRadius;
        var newEmission = (maxEmission - minEmission) * percentSize + minEmission;
        
        
        var psShape = ps.shape;
        var psEmission = ps.emission;

        psShape.radius = newRadius;
        psEmission.rateOverTime = newEmission;
        
//
//        hovered -= Time.deltaTime;
//        if (hovered > 0)
//        {
//            
//            psShape.radius = Mathf.Min(psShape.radius + Time.deltaTime * 4, maxRadius);
//            psEmission.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Min(psEmission.rateOverTime.constant + Time.deltaTime * 2000, 1000));
//            
//        }
//        else
//        {
//            psShape.radius = Mathf.Max(psShape.radius - Time.deltaTime *3, minRadius);
//            psEmission.rateOverTime = new ParticleSystem.MinMaxCurve(Mathf.Max(psEmission.rateOverTime.constant - Time.deltaTime * 1000, 100));
//        }
    }
}
