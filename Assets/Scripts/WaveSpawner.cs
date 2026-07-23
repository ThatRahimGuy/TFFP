using System.Collections;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public TextMeshProUGUI waveCountText;
    int waveCount = 1;
    public float spawnrate = 1.0f;
    public float timeBetweenWaves = 6.0f;
    
    public int enemyCount;
    private int killed;
    

    public GameObject enemy;
    public Transform spawnpoint;

    bool waveIsDone = true;

    private void Start()
    {
        killed = enemyCount;
        StartCoroutine(waveSpawner());
        waveCountText.text = "Wave Count: " + waveCount;
    }
    void Update()
    {
        //waveCountText.text = "Wave: " + waveCount.ToString();
        //if (waveIsDone == true)
        //{
            
        //}
    }

    public void EnemyKilled()
    {
        killed -= 1;
        Debug.Log("enemy count " + killed );
        if (killed == 0)
        {
            spawnrate = 0.1f;
            enemyCount += 2;
            killed = enemyCount;
            waveCount += 1;
            waveCountText.text = "Wave Count: " + waveCount;

            Debug.Log("waveover");
            StartCoroutine(waveSpawner());
        }
    }

    public void WaveDone()
    {
        waveIsDone = true;
    }

    IEnumerator waveSpawner()
    {
  

        for (int i = 0; i < enemyCount; i++)
        {
            GameObject enemyClone = Instantiate(enemy, spawnpoint.position + new Vector3(1f*i, 0, 0), Quaternion.identity);

            yield return new WaitForSeconds(spawnrate);
        }

        

        //yield return new WaitForSeconds(timeBetweenWaves); 

        
    }
}
