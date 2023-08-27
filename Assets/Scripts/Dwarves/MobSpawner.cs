using System.Collections.Generic;
using UnityEngine;

public class MobSpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnableMob
    {
        public GameObject mobPrefab;
        public float spawnFrequency = 1f; // Value between 0 to 1, where 1 is the most frequent.
    }

    [System.Serializable]
    public class SpawnLocation
    {
        public Transform locationTransform;
        public bool isEnabled = true;
        public float spawnFrequencyModifier = 1f;  // To modify frequency for this specific location.
        public Vector3 boundingBoxDimensions = Vector3.one;
        public float density = 0.5f;  // Value between 0 (spread out) to 1 (centered).
    }

    public List<SpawnableMob> mobsToSpawn = new List<SpawnableMob>();
    public List<SpawnLocation> spawnLocations = new List<SpawnLocation>();
    public float overallSpawnRate = 1f;  // Time in seconds.

    private float spawnTimer = 0f;

    public Transform mobTarget; //Transform mobs will target by default

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= overallSpawnRate)
        {
            spawnTimer = 0f; // Reset the timer

            foreach (SpawnLocation location in spawnLocations)
            {
                if (location.isEnabled && Random.value <= location.spawnFrequencyModifier)
                {
                    SpawnMobAtLocation(location);
                }
            }
        }
    }

    private void Test()
    {
        Debug.Log("dead dwarf");
    }

    private void SpawnMobAtLocation(SpawnLocation location)
    {
        Vector3 spawnPosition = GetSpawnPositionWithinBox(location);
        SpawnableMob mobToSpawn = GetMobToSpawn();

        if (mobToSpawn != null)
        {
            GameObject mob = Instantiate(mobToSpawn.mobPrefab, spawnPosition, Quaternion.identity);

            mob.transform.parent = transform;
            //mob.transform.LookAt(new Vector3(mobTarget.position.x, spawnPosition.y, mobTarget.position.y));

            MobBrain mobBrain = mob.GetComponentInChildren<MobBrain>();
            mobBrain.target = mobTarget;
            mob.GetComponentInChildren<Destructible>().OnDeath += mobBrain.Die;
            mob.GetComponentInChildren<Destructible>().OnDeath += Test;
        }
    }

    private Vector3 GetSpawnPositionWithinBox(SpawnLocation location)
    {
        Vector3 randomOffset = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;

        // Modify range based on density.
        Vector3 scaleAdjustment = new Vector3(1f - location.density, 1f - location.density, 1f - location.density);
        Vector3 rangedOffset = Vector3.Scale(randomOffset * 0.5f, Vector3.Scale(scaleAdjustment, location.boundingBoxDimensions));

        return location.locationTransform.position + rangedOffset;
    }

    private SpawnableMob GetMobToSpawn()
    {
        float total = 0;

        foreach (SpawnableMob mob in mobsToSpawn)
        {
            total += mob.spawnFrequency;
        }

        float randomPoint = Random.value * total;

        for (int i = 0; i < mobsToSpawn.Count; i++)
        {
            if (randomPoint < mobsToSpawn[i].spawnFrequency)
            {
                return mobsToSpawn[i];
            }
            else
            {
                randomPoint -= mobsToSpawn[i].spawnFrequency;
            }
        }

        return null;
    }
}
