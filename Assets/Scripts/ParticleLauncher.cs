using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

    public ParticleSystem particleLauncher;
    public GameObject decalEmitterPrefab;
    public Gradient particleColorGradient;

    List<ParticleCollisionEvent> collisionEvents;
    private TurretVariant variant;

    public static GameObject decalEmitter;
    private ColorVariant colorVariant;
    private int colorVariantInt;

    void Start ()
    {
        collisionEvents = new List<ParticleCollisionEvent> ();
        if (decalEmitter == null)
        {
            decalEmitter = Instantiate(decalEmitterPrefab);
        }
    }

    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents (particleLauncher, other, collisionEvents);

        var enemy = other.GetComponent<Enemy>();
        enemy.Damage(variant.damage);
            
        foreach (var cEvent in collisionEvents)
        {
            decalEmitter.GetComponent<ParticleDecalPool>().ParticleHit (particleColorGradient, other.transform.position);
            EmitAtLocation (cEvent);
        }   
    }

    void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        /*
        splatterParticles.transform.position = particleCollisionEvent.intersection;
        splatterParticles.transform.rotation = Quaternion.LookRotation (particleCollisionEvent.normal);
        ParticleSystem.MainModule psMain = splatterParticles.main;
        psMain.startColor = particleColorGradient.Evaluate (Random.Range (0f, 1f));
        splatterParticles.Emit (1);
        */
    }

    public void Shoot(TurretVariant variant, Vector3 offset, ColorVariant colorVariant, int colorVariantInt)
    {
        particleLauncher.transform.localPosition = variant.launcherOffset + offset;

        var collisionQuality = particleLauncher.collision;
        switch (colorVariantInt)
        {
            case 0:
                collisionQuality.collidesWith = LayerMask.GetMask("EnemyGreen");
                break;                
            case 1:
                collisionQuality.collidesWith = LayerMask.GetMask("EnemyRed");
                break;
            case 2:
                collisionQuality.collidesWith = LayerMask.GetMask("EnemyBlue");
                break;
        }
        
        this.variant = variant;
        this.colorVariantInt = colorVariantInt;
        particleColorGradient = colorVariant.gradient;
        this.colorVariant = colorVariant;
        ParticleSystem.MainModule psMain = particleLauncher.main;
        psMain.startColor =  particleColorGradient.Evaluate (Random.Range (0f, 1f));;
        psMain.startSpeed =  variant.speed;
        particleLauncher.Emit(1);
    }
        
}