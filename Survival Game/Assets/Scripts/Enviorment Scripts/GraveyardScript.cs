using UnityEngine;
using Random = UnityEngine.Random;

public class GraveyardScript : MonoBehaviour
{
    [SerializeField] private GraveyardSpawnEntry[] graveyardSpawnEntries;

    [SerializeField] private float timeInbetweenSpawns;

    [SerializeField] private float timeUntilSpawn;

    void Update() {
        if(DayAndNightCycle.isNightTime == false) return;
        
        timeUntilSpawn -= Time.deltaTime;

        if(timeUntilSpawn <= 0f) {
            timeUntilSpawn = timeInbetweenSpawns;
            SpawnEnemy();
        }
    }

    private void SpawnEnemy() {
        float enemyToSpawnNum = Random.Range(0f,1f);

        for(int i = 0; i < graveyardSpawnEntries.Length; i++) {
            GraveyardSpawnEntry graveyardSpawnEntry = graveyardSpawnEntries[i];

            if(!(graveyardSpawnEntry.probabilityToSpawn < enemyToSpawnNum)) continue;

            for(int spawnIndex = 0; spawnIndex < graveyardSpawnEntry.amountToSpawn; spawnIndex++) {
                Vector2 posToSpawn = new Vector2(Random.Range(0f,7f) - 3.5f, Random.Range(0f,7f) - 3.5f);

                GameObject coppiedEnemy = Instantiate(graveyardSpawnEntry.enemy);

                coppiedEnemy.transform.position = (Vector3) posToSpawn + transform.position;

            }
        }
    }
}