using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

    public ParticleSystem particleLauncher;
    public ParticleSystem splatterParticles;
    public ParticleDecalPool splatDecalPool;
    public Gradient particleColorGradient;

    List<ParticleCollisionEvent> collisionEvents;
    private TurretVariant variant;

    void Start () 
    {
        collisionEvents = new List<ParticleCollisionEvent> ();
    }

    void OnParticleCollision(GameObject other)
    {
        ParticlePhysicsExtensions.GetCollisionEvents (particleLauncher, other, collisionEvents);

        foreach (var cEvent in collisionEvents)
        {
            if (Random.Range (0f, 1f) > 0.5f)
            {
                splatDecalPool.ParticleHit (cEvent, particleColorGradient, other);
            }
            EmitAtLocation (cEvent);
        }

        var enemy = other.GetComponent<Enemy>();
        enemy.Damage(variant.damage);
    }

    void EmitAtLocation(ParticleCollisionEvent particleCollisionEvent)
    {
        splatterParticles.transform.position = particleCollisionEvent.intersection;
        splatterParticles.transform.rotation = Quaternion.LookRotation (particleCollisionEvent.normal);
        ParticleSystem.MainModule psMain = splatterParticles.main;
        psMain.startColor = particleColorGradient.Evaluate (Random.Range (0f, 1f));
        splatterParticles.Emit (1);
    }

    public void Shoot(TurretVariant variant, Color color, Gradient gradient)
    {
        //var obj = projectilePool.Get();
        //obj.transform.position = transform.position + offset;
        //obj.transform.rotation = Quaternion.Euler(0, 0, headAngle);
        //obj.GetComponent<Projectile>().variant = turretVariant;
        this.variant = variant;
        particleColorGradient = gradient;
        ParticleSystem.MainModule psMain = particleLauncher.main;
        psMain.startColor =  particleColorGradient.Evaluate (Random.Range (0f, 1f));;
        psMain.startSpeed =  variant.speed;
        particleLauncher.Emit(1);
    }
        
}