using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item")]
public class Item : ScriptableObject
{
    [SerializeField] private string itemName;

    [SerializeField] private Sprite sprite;

    [SerializeField] private bool stackable;

    [SerializeField] private bool showWhenHolding;

    [SerializeField] private bool generatedSprite;

    private bool spriteWasGenerated = false;

    public int quantity = 1;

    public string GetName() {
        return itemName;
    }

    public Sprite GetSprite() {
        return sprite;
    }

    public bool GetStackable() {
        return stackable;
    }

    public int GetQuantity() {
        return quantity;
    }

    public bool GetShowWhenHolding() {
        return showWhenHolding;
    }

    public bool GetGeneratedSprite() {
        return generatedSprite;
    }

    public void SetSprite(Sprite newSprite) {
        sprite = newSprite;
    }

    public void SpriteWasGenerated() {
        spriteWasGenerated = true;
    }

    public bool GetSpriteWasGenerated() {
        return spriteWasGenerated;
    }

    public void GenerateSprite() {
        sprite = Sprite.Create(
            RandomTextureGenerator.GetRandomTexture(itemName),
            new Rect(0, 0, RandomTextureGenerator.textureSize.x, RandomTextureGenerator.textureSize.y),
            new Vector2(0.5f, 0.5f),
            16f // pixels per unit
        );
    }

    public virtual void Use() {Debug.Log(itemName + " was used");}
}