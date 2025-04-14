using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 5f;
    [SerializeField] private float difficultyIncreaseRate = 0.98f;
    [SerializeField] private int maxEnemiesPerWave = 5;
    
    [SerializeField] private Transform leftSpawnPoint;
    [SerializeField] private Transform rightSpawnPoint;
    
    private float spawnTimer;
    private int waveNumber = 1;
    
    void Start()
    {
        // Spawn noktaları yoksa, varsayılan noktalara ayarla
        if (leftSpawnPoint == null)
        {
            GameObject left = new GameObject("LeftSpawnPoint");
            left.transform.position = new Vector3(-10, 0, 0);
            left.transform.parent = transform;
            leftSpawnPoint = left.transform;
        }
        
        if (rightSpawnPoint == null)
        {
            GameObject right = new GameObject("RightSpawnPoint");
            right.transform.position = new Vector3(10, 0, 0);
            right.transform.parent = transform;
            rightSpawnPoint = right.transform;
        }
    }
    
    void Update()
    {
        spawnTimer += Time.deltaTime;
        
        if (spawnTimer >= spawnRate)
        {
            SpawnWave();
            spawnTimer = 0;
            
            // Zorluk artışı
            spawnRate *= difficultyIncreaseRate;
        }
    }
    
    private void SpawnWave()
    {
        int enemyCount = Mathf.Min(waveNumber, maxEnemiesPerWave);
        
        for (int i = 0; i < enemyCount; i++)
        {
            // Rastgele sol veya sağ taraftan spawn
            bool spawnLeft = Random.value < 0.5f;
            Vector2 spawnPosition = spawnLeft ? 
                leftSpawnPoint.position : rightSpawnPoint.position;
            
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            
            // Doğrultuyu spawn pozisyonuna göre ayarla (soldan geliyorsa sağa, sağdan geliyorsa sola)
            enemy.GetComponent<Enemy>().SetDirection(!spawnLeft);
        }
        
        waveNumber++;
        Debug.Log("Dalga " + waveNumber + " başladı!");
    }
}