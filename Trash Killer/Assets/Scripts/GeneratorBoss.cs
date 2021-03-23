using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorBoss : MonoBehaviour
{
    public GameObject[] enemies;
    public Transform[] zone;
    private GameObject[] enemyToGenerate;
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
            SpawnEnemies();
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

}
