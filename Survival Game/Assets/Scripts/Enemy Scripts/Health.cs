using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float health;

    public void TakeDamage(float damage) {
        health -= damage;

        if(health <= 0) {
            Destroy(gameObject);
        }
    }
}