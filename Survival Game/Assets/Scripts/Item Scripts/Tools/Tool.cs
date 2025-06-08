using UnityEngine;

public class Tool : Item
{
    [Header("Tool")]
    [SerializeField] private float toolSpeed;
    [SerializeField] private float toolStrength;
    [SerializeField] public float toolRange;
    [SerializeField] public LayerMask tilemap;
}