using UnityEngine;
using TMPro;
using System.Collections;

//credit: https://forum.unity.com/threads/how-to-make-infinite-wave-spawner-with-different-enemy-types.1067810/

public class GhostSpawner : MonoBehaviour
{
    [SerializeField]
    private Ghost enemyPrefab;

    [SerializeField]
    public GameObject[] spawnPoints;

    // In seconds
    [SerializeField] private float interval = 2f;

    // [SerializeField] private TextMeshProUGUI countdownUI;
    [SerializeField] private GameObject countdownUI;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemyWave(GameManager.gameManager.waveNumber));
    }

    private int enemyCount = 0;

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsOfType<Ghost>().Length;

        if (enemyCount == 0 && !GameManager.gameManager.inWave)
        {
            GameManager.gameManager.NextWave();
            StartCoroutine(SpawnEnemyWave(GameManager.gameManager.waveNumber));
        }
    }

    IEnumerator SpawnEnemyWave(int enemiesToSpawn)
    {
        GameManager.gameManager.inWave = true;
        countdownUI.gameObject.SetActive(true);
        float timeBetweenWavesDisplay = interval;
        TextMeshProUGUI countdownUIText = countdownUI.GetComponentInChildren<TextMeshProUGUI>(true);
        countdownUIText.text = "0:0" + timeBetweenWavesDisplay.ToString();
        while(timeBetweenWavesDisplay > 0)
        {
            yield return new WaitForSeconds(1); //We wait here to pause between wave spawning
            timeBetweenWavesDisplay-=1;
            countdownUIText.text = "0:0" + timeBetweenWavesDisplay.ToString();
        }
        countdownUI.gameObject.SetActive(false);
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].transform.position, enemyPrefab.transform.rotation);
            yield return new WaitForSeconds(interval/3); //We wait here to give a bit of time between each enemy spawn
        }
        GameManager.gameManager.inWave = false;
    }

}
