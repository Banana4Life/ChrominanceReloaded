using UnityEngine;

public class ParticleDecalPool : MonoBehaviour {

    public int maxDecals = 100;
    public float decalSizeMin = .5f;
    public float decalSizeMax = 1.5f;

    private ParticleSystem decalParticleSystem;
    private int particleDecalDataIndex;
    private ParticleDecalData[] particleData;
    private ParticleSystem.Particle[] particles;

    void Start () 
    {
        decalParticleSystem = GetComponent<ParticleSystem> ();
        particles = new ParticleSystem.Particle[maxDecals];
        particleData = new ParticleDecalData[maxDecals];
        for (int i = 0; i < maxDecals; i++) 
        {
            particleData [i] = new ParticleDecalData ();    
        }
    }

    public void ParticleHit(Gradient colorGradient, Vector3 position)
    {
        SetParticleData (colorGradient, position);
        DisplayParticles ();
    }

    void SetParticleData(Gradient colorGradient, Vector3 position)
    {
        if (particleDecalDataIndex >= maxDecals) 
        {
            particleDecalDataIndex = 0;
        }

        particleData[particleDecalDataIndex].position = position; //particleCollisionEvent.intersection;
        particleData [particleDecalDataIndex].rotation = new Vector3(0, 0,  Random.Range (0, 360));
        particleData [particleDecalDataIndex].size = Random.Range (decalSizeMin, decalSizeMax);
        particleData [particleDecalDataIndex].color = colorGradient.Evaluate (Random.Range (0f, 1f));

        particleDecalDataIndex++;
    }

    void DisplayParticles()
    {
        for (int i = 0; i < particleData.Length; i++) 
        {
            particles [i].position = particleData [i].position;
            particles [i].rotation3D = particleData [i].rotation;
            particles [i].startSize = particleData [i].size;
            particles [i].startColor = particleData [i].color;
        }

        decalParticleSystem.SetParticles (particles, particles.Length);
    }
}
