using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health
{

    [SerializeField] private GameObject deathScreen;

    [SerializeField] private Image greenHealthBar;
    [SerializeField] private TMP_Text healthText;

    public float regenPerSecond;

    void Update()
    {
        health += regenPerSecond * Time.deltaTime;

        health = Mathf.Min(health, maxHealth);

        greenHealthBar.fillAmount = health / maxHealth;

        healthText.text = Mathf.Round(health) + "/" + maxHealth;
    }

    public override void Die()
    {
        PlayParticle(particlePlayedOnDeath);

        gameObject.GetComponent<PlayerMovement>().enabled = false;

        gameObject.GetComponent<InventoryManager>().enabled = false;

        deathScreen.SetActive(true);
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    public void Respawn() {
        health = maxHealth / 2;

        transform.position = new Vector3(0f,0f,0f);

        gameObject.GetComponent<PlayerMovement>().enabled = true;

        gameObject.GetComponent<InventoryManager>().enabled = true;

        deathScreen.SetActive(false);
    }
}