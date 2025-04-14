using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private int health = 20;
    [SerializeField] private int damage = 1;
    [SerializeField] private int coinValue = 10;
    
    private bool movingRight = false;
    private Rigidbody2D rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        // Hareket
        float direction = movingRight ? 1 : -1;
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
        
        // Yön gösterimi için scale'i ayarla
        transform.localScale = new Vector3(direction, 1, 1);
    }
    
    public void SetDirection(bool goRight)
    {
        movingRight = goRight;
    }
    
    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        
        if (health <= 0)
        {
            Die();
        }
    }
    
    private void Die()
    {
        // Oyuncuya para ver
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
        {
            player.AddCoins(coinValue);
        }
        
        // Düşmanı yok et
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Oyuncuya çarpma kontrolü
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                player.TakeDamage();
            }
            
            // Düşman çarpma sonrası kaybolsun
            Destroy(gameObject);
        }
    }
}