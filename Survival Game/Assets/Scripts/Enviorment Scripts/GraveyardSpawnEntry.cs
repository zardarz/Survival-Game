using UnityEngine;

[System.Serializable]
public class GraveyardSpawnEntry
{
    public GameObject enemy;
    public int amountToSpawn;
    
    [Range(0,1)]
    public float probabilityToSpawn;
}