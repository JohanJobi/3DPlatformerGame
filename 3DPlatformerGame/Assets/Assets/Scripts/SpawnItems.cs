using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class environmentScript : MonoBehaviour
{
    public ItemDatabaseObject database;
    public GroundItem itemPrefab;
    public int itemsToBeSpawned;
    public GameObject enemyPrefab;
    public GameObject bossPrefab;
    private int enemycount;
    private int bosscount;

    public playerHP p1;
    public playerHP p2;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < itemsToBeSpawned ; i++)
        {
            var RandomValueZ = UnityEngine.Random.Range(81, 213);
            var RandomValueX = UnityEngine.Random.Range(111, 282);
            var RandomId = UnityEngine.Random.Range(0, 8);
            ItemObject item =database.GetItem[RandomId];
            itemPrefab.item = item;
            Instantiate(itemPrefab, new Vector3(RandomValueX, 0, RandomValueZ), Quaternion.identity);
        }
        StartCoroutine(SpawnEnemy());
        StartCoroutine(SpawnBoss());


    }
    IEnumerator SpawnEnemy()
    {

        while (enemycount < 25)
        {
            Instantiate(enemyPrefab, new Vector3(111f, 1f, 81f), Quaternion.identity);
            Debug.Log("enemy spawned");
            bosscount++;
            yield return new WaitForSeconds(80);
        }
    }

    IEnumerator SpawnBoss()
    {
        while (bosscount < 2)
        {
            Instantiate(bossPrefab, new Vector3(200f, 0f, 95f), Quaternion.identity);
            bosscount++;
            yield return new WaitForSeconds(200);
        }


    }
    private void Update()
    {
        if(p1.dead && p2.dead)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }
}
