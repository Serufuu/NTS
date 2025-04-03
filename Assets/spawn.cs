using UnityEngine;

public class RandomSpawner2D : MonoBehaviour
{
    public GameObject objectToSpawn; // Objet � spawn
    public float spawnRadius = 5f;   // Rayon de spawn

    void Start()
    {
        SpawnObjects();
    }

    void SpawnObjects()
    {
        // Position al�atoire dans un cercle
        Vector2 randomPosition = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomPosition.x, 0, randomPosition.y);

        // Instancier l'objet � la position calcul�e
        GameObject spawnedObject = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);

        // Assigner le tag "Target" � l'objet instanci�
        spawnedObject.tag = "Target";
    }
}