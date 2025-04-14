using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private int health = 3;
    [SerializeField] private int coins = 100;
    
    // UI için property'ler
    public int Health { get { return health; } }
    public int Coins { get { return coins; } }
    
    void Update()
    {
        // Yatay hareket için input al
        float horizontalInput = Input.GetAxis("Horizontal");
        
        // Transform ile hareket
        transform.Translate(new Vector3(horizontalInput * moveSpeed * Time.deltaTime, 0, 0));
        
        // Karakter yönünü ayarla
        if (horizontalInput != 0)
        {
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
        }
    }
    
    public void TakeDamage()
    {
        health--;
        Debug.Log("Mevcut Can: " + health);
        
        if (health <= 0)
        {
            GameOver();
        }
    }
    
    private void GameOver()
    {
        Debug.Log("Oyun Bitti!");
    }
    
    public void AddCoins(int amount)
    {
        coins += amount;
        Debug.Log("Mevcut Para: " + coins);
    }
    
    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            Debug.Log("Para Harcandı. Kalan: " + coins);
            return true;
        }
        Debug.Log("Yeterli para yok!");
        return false;
    }
}