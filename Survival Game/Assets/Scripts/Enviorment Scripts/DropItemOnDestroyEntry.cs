using UnityEngine;

[System.Serializable]
public class DropItemOnDestroyEntry
{
    public Item itemToDrop;
    public int minAmountOfItem;
    public int maxAmountOfItem;

    [Range(0,1)]
    public float probablityToDrop;
}