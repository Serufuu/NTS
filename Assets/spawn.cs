using UnityEngine;

public class RandomSpawner2D : MonoBehaviour
{
    public GameObject objectToSpawn; // Objet à spawn
    public float spawnRadius = 5f;   // Rayon de spawn

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        // Position aléatoire dans un cercle
        Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPosition.x, 0, randomPosition.y);

        // Instancier l'objet à la position calculée
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        // Assigner le tag "Target" à l'objet instancié
        spawnedObject.tag = "Target";
    }
}