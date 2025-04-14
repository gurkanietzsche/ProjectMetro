using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Düşman Özellikleri")]
    public int maxHealth = 3;
    private int currentHealth;
    public int damage = 1;
    public float moveSpeed = 2f;
    
    [Header("Saldırı Ayarları")]
    public float attackRate = 1f;
    public float attackRange = 1f;
    private float nextAttackTime = 0f;
    
    // Bileşenler
    private Rigidbody2D rb;
    private Transform target;
    private SpriteRenderer spriteRenderer;
    
    // Hareket yönü
    private bool movingRight = true;
    
    void Start()
    {
        // Bileşenleri al
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
        // Değişkenleri başlat
        currentHealth = maxHealth;
        
        // Oyuncuyu hedef olarak al
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }
    
    void Update()
    {
        if (target == null)
            return;
            
        // Oyuncuya doğru hareket et
        MoveTowardsPlayer();
        
        // Oyuncuya saldır
        if (Time.time >= nextAttackTime && IsPlayerInRange())
        {
            Attack();
            nextAttackTime = Time.time + attackRate;
        }
    }
    
    void MoveTowardsPlayer()
    {
        // Oyuncuya olan mesafeyi hesapla
        float distanceToPlayer = Vector2.Distance(transform.position, target.position);
        
        // Saldırı menzilinden uzaktaysa hareket et
        if (distanceToPlayer > attackRange)
        {
            // Hareket yönünü belirle
            float directionToPlayer = target.position.x - transform.position.x;
            
            // Hareket yönünü değiştir
            if (directionToPlayer > 0 && !movingRight)
            {
                Flip();
            }
            else if (directionToPlayer < 0 && movingRight)
            {
                Flip();
            }
            
            // Hareket et
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            // Saldırı menzilindeyse dur
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }
    
    bool IsPlayerInRange()
    {
        if (target == null)
            return false;
            
        float distance = Vector2.Distance(transform.position, target.position);
        return distance <= attackRange;
    }
    
    void Attack()
    {
        // Oyuncuya zarar ver
        PlayerController player = target.GetComponent<PlayerController>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
    
    void Flip()
    {
        movingRight = !movingRight;
        
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // Hasar aldığında kırmızı yanıp sön
        StartCoroutine(FlashRed());
        
        // Öldü mü kontrol et
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    IEnumerator FlashRed()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }
    
    void Die()
    {
        // Ölüm efekti oluştur veya animasyon oynat
        
        // GameManager'a hurda/kaynak ekle (eğer varsa)
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.AddScrap(1); // Ölen düşmandan 1 hurda kazan
        }
        
        // Düşmanı yok et
        Destroy(gameObject);
    }
    
    // Debug için saldırı menzilini görselleştir
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}