using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleSpawner : MonoBehaviour
{
    public GameObject holePrefab; // Assigner le prefab du trou ici
    public float minDistance = 2f; // Distance minimale devant la caméra
    public float maxDistance = 5f; // Distance maximale devant la caméra
    public float spawnHeightOffset = -0.4f; // Ajustement vertical du spawn

    private GameObject currentHole;

    void Start()
    {
        SpawnHole();
    }

    public void SpawnHole()
    {
        if (currentHole != null)
        {
            Destroy(currentHole); // Supprime l'ancien trou s'il existe
        }

        // Générer une distance aléatoire devant la caméra
        float distance = Random.Range(minDistance, maxDistance);

        // Calculer la position devant la caméra
        Vector3 spawnPosition = transform.position + transform.forward * distance;
        spawnPosition.y += spawnHeightOffset; // Ajustement pour que ça soit sur le sol

        // Instancier le trou
        currentHole = Instantiate(holePrefab, spawnPosition, Quaternion.identity);
    }
}
