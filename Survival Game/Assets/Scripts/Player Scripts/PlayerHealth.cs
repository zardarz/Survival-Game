using UnityEngine;

public class PlayerHealth : Health
{

    [SerializeField] private GameObject deathScreen;

    public override void Die()
    {
        PlayParticle(particlePlayedOnDeath);

        gameObject.GetComponent<PlayerMovement>().enabled = false;

        deathScreen.SetActive(true);
    }
}