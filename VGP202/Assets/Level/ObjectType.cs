using UnityEngine;

[System.Serializable]
public class ObjectType : MonoBehaviour
{
    public GameObject prefab;
    public bool canSpawnOnTop = true;
    public bool canSpawnOnBottom = true;
    [Range(0f, 1f)] public float spawnChance = 1f;
}
