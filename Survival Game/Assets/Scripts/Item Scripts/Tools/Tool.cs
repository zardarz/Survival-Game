using UnityEngine;

public class Tool : Item
{
    [Header("Tool")]
    [Range(0,30)]
    [SerializeField] private float toolSpeed;
    [SerializeField] private float toolStrength;
    [SerializeField] public float toolRange;
    [SerializeField] public LayerMask tilemap;

    public override float GetToolSpeed() {
        return toolSpeed;
    }

    public float GetToolStrength() {
        return toolStrength;
    }
}