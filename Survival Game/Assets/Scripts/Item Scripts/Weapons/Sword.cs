using UnityEngine;

[CreateAssetMenu(fileName = "New Sword", menuName = "Weapon/Sword")]
public class Sword : Weapon
{
    [Header("Sword")]
    [Range(0,30)]
    [SerializeField] private float swordRange;

    public float GetSwordRange() {
        return swordRange;
    }

    public override void Use() {
        Transform playerTransform = PlayerMovement.GetPlayerTransform();

        Vector2 dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - playerTransform.position;

        dir *= swordRange/2;

        Vector2 posOfDamageCircle = new(playerTransform.position.x + dir.x, playerTransform.position.y + dir.y);

        Collider2D[] collidersInDamageCircle = Physics2D.OverlapCircleAll(posOfDamageCircle, swordRange/2);

        for(int i = 0; i < collidersInDamageCircle.Length; i++) {
            if(collidersInDamageCircle[i].CompareTag("Damageable")) {
                Health coliderHealth = collidersInDamageCircle[i].gameObject.GetComponent<Health>();

                coliderHealth.TakeDamage(GetWeaponDamage());
            }
        }
    }
}