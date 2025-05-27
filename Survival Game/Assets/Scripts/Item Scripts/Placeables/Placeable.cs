using UnityEngine;

public class Placeable : Item
{
    public override void Use() {
        Debug.Log(GetName() + " was placed");
    }
}