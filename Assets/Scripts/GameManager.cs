using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    [Header("Oyun Durumu")]
    public bool isGameOver = false;
    
    [Header("Kaynaklar")]
    public int scrap = 0; // Hurda/para birimi
    
    [Header("Gün/Gece Döngüsü")]
    public float dayDuration = 60f; // Saniye cinsinden gün süresi
    public float nightDuration = 30f; // Saniye cinsinden gece süresi
    private bool isNight = false;
    private float timeUntilNextPhase;
    
    void Awake()
    {
        // Singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Oyun başlangıcı ayarları
        isGameOver = false;
        
        // Gün/gece döngüsünü başlat
        timeUntilNextPhase = dayDuration;
        isNight = false;
    }
    
    void Update()
    {
        // Gün/gece döngüsünü yönet
        if (!isGameOver)
        {
            timeUntilNextPhase -= Time.deltaTime;
            
            if (timeUntilNextPhase <= 0)
            {
                if (isNight)
                {
                    // Geceden gündüze geçiş
                    isNight = false;
                    timeUntilNextPhase = dayDuration;
                    OnDayStart();
                }
                else
                {
                    // Gündüzden geceye geçiş
                    isNight = true;
                    timeUntilNextPhase = nightDuration;
                    OnNightStart();
                }
            }
        }
    }
    
    void OnDayStart()
    {
        Debug.Log("Gün başladı!");
    }
    
    void OnNightStart()
    {
        Debug.Log("Gece başladı! Zombi saldırısı!");
    }
    
    public void AddScrap(int amount)
    {
        scrap += amount;
    }
    
    public bool SpendScrap(int amount)
    {
        if (scrap >= amount)
        {
            scrap -= amount;
            return true;
        }
        return false;
    }
    
    public void GameOver()
    {
        isGameOver = true;
        Debug.Log("Oyun Bitti!");
        
        // Birkaç saniye sonra yeniden başlat
        Invoke("RestartGame", 3f);
    }
    
    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public bool IsNight()
    {
        return isNight;
    }
}