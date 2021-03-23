using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class GeneradorEnemigos : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] zone;
    private GameObject[] enemyToGenerate;
    public float spawnEnemyRadius;
    private int rdm;

    private void Awake()
    {
        enemyToGenerate = new GameObject[zone.Length];
    }

    void Start()
    {
        rdm = Random.Range(0, enemies.Length);
    }

    void Update()
    {
        float distance = Vector3.Distance(PlayerController.position, transform.position);

        if(distance < spawnEnemyRadius)
        {
            SpawnEnemies();
        }
    }

    private void SpawnEnemies()
    {
        for (int i = 0; i < enemyToGenerate.Length; i++)
        {
            enemyToGenerate[i] = enemies[rdm];
            Instantiate(enemyToGenerate[i], zone[i].position, zone[i].rotation);
        }
        gameObject.SetActive(false);
    }

    // Podemos dibujar el radio de spawn sobre la escena dibujando una esfera
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, spawnEnemyRadius);
    }

}
