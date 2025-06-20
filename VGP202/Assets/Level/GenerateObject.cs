using System.Collections.Generic;
using UnityEngine;

public class GenerateObject : MonoBehaviour
{
    [SerializeField] private List<ObjectType> obstacleTypes;
    [SerializeField] private float obstacleChance = 0.5f;
    [SerializeField] private float obstacleCooldown = 1.5f;

    private float lastSpawnTime = 0f;

    public void GenerateObstacle(GameObject chunk, int x)
    {
        // Cooldown check
        if (Time.time - lastSpawnTime < obstacleCooldown) return;

        if (Random.value > obstacleChance) return;

        // Randomly choose top or bottom
        bool spawnTop = Random.value > 0.5f;

        // Filter for valid spawnable obstacles
        List<ObjectType> validObstacles = obstacleTypes.FindAll(o =>
            (spawnTop && o.canSpawnOnTop || !spawnTop && o.canSpawnOnBottom) &&
            Random.value <= o.spawnChance
        );

        if (validObstacles.Count == 0) return;

        ObjectType selected = validObstacles[Random.Range(0, validObstacles.Count)];

        Vector3 spawnPosition = new Vector3(x, spawnTop ? 4.1f : -4f, 0f);

        GameObject obstacle = Instantiate(selected.prefab, chunk.transform);
        obstacle.transform.localPosition = spawnPosition;

        lastSpawnTime = Time.time;
    }
}

