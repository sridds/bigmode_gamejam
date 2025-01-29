using UnityEngine;

public class HeadShadow : MonoBehaviour
{
    [SerializeField] ParticleSystem HeadParticle;
    ParticleSystem.Particle[] m_Particles;
    Vector2 startSize;
    void Start()
    {
        startSize = transform.localScale;
    }
    private void LateUpdate()
    {
        InitializeIfNeeded();

        // GetParticles is allocation free because we reuse the m_Particles buffer between updates
        int numParticlesAlive = HeadParticle.GetParticles(m_Particles);

        // Change only the particles that are alive
        for (int i = 0; i < numParticlesAlive; i++)
        {
            transform.position = new Vector2(m_Particles[i].position.x, transform.position.y);
            transform.localScale = startSize * m_Particles[i].GetCurrentSize(HeadParticle);
        }

        // Apply the particle changes to the particle system
        HeadParticle.SetParticles(m_Particles, numParticlesAlive);
    }

    void InitializeIfNeeded()
    {
        if (HeadParticle == null)
            HeadParticle = GetComponent<ParticleSystem>();

        if (m_Particles == null || m_Particles.Length < HeadParticle.maxParticles)
            m_Particles = new ParticleSystem.Particle[HeadParticle.maxParticles];
    }
}
