using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health;

    [SerializeField] private ParticleSystem particlePlayedOnDeath;
    [SerializeField] private ParticleSystem particlePlayedOnHit;

    public void TakeDamage(float damage) {
        health -= damage;

        PlayParticle(particlePlayedOnHit);

        if(health <= 0) {
            PlayParticle(particlePlayedOnDeath);
            Destroy(gameObject);
        }
    }

    void PlayParticle(ParticleSystem particleSystem) {
        GameObject coppiedParticleGO = Instantiate(particleSystem).gameObject;
        coppiedParticleGO.transform.position = transform.position;

        coppiedParticleGO.GetComponent<ParticleSystem>().Play();

        Destroy(coppiedParticleGO, coppiedParticleGO.GetComponent<ParticleSystem>().main.duration);
    }
}