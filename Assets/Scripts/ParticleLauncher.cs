using System.Collections.Generic;
using UnityEngine;

public class ParticleLauncher : MonoBehaviour {

    public ParticleSystem particleLauncher;
    public GameObject decalEmitterPrefab;
    public Gradient particleColorGradient;

    List<ParticleCollisionEvent> collisionEvents;
    private TurretVariant variant;

    public static GameObject decalEmitter;

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

        foreach (var cEvent in collisionEvents)
        {
            decalEmitter.GetComponent<ParticleDecalPool>().ParticleHit (particleColorGradient, other.transform.position);
            EmitAtLocation (cEvent);
        }

        var enemy = other.GetComponent<Enemy>();
        enemy.Damage(variant.damage);
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

    public void Shoot(TurretVariant variant, Vector3 offset, Gradient gradient)
    {
        particleLauncher.transform.localPosition = variant.launcherOffset + offset;
        
        this.variant = variant;
        particleColorGradient = gradient;
        ParticleSystem.MainModule psMain = particleLauncher.main;
        psMain.startColor =  particleColorGradient.Evaluate (Random.Range (0f, 1f));;
        psMain.startSpeed =  variant.speed;
        particleLauncher.Emit(1);
    }
        
}