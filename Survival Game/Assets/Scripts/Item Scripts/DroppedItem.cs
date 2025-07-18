using System.Collections;
using UnityEngine;

public class DroppedItem : MonoBehaviour
{
    public Item item;

    public bool canBePickedup = false;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void AddForceOnDrop(float strength) {
        float angle = Random.Range(0f, 2*Mathf.PI);
        Vector2 randomDir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        rb.AddForce(randomDir * strength, ForceMode2D.Impulse);
    }

    public void AddForceOnDrop(float strength, Vector2 dir) {
        Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();

        rb.AddForce(dir * strength, ForceMode2D.Impulse);
    }

    public void AddRandomTorque(float max) {
        gameObject.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-max,max));
    }

    public IEnumerator EnablePickingUpAfterTime(float delay) {
        yield return new WaitForSeconds(delay);
        canBePickedup = true;
    }

    public void StartCotrotean(float delay) {
        StartCoroutine(EnablePickingUpAfterTime(delay));
    }

    public void ChangeSprite() {
        if(item == null) {
            return;
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = item.GetSprite();
    }
}