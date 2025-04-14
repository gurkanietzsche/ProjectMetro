using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    public float moveSpeed = 5f;
    
    [Header("Sağlık Sistemi")]
    public int maxHealth = 3;
    private int currentHealth;
    
    [Header("Silah Sistemi")]
    public int maxAmmo = 30;
    private int currentAmmo;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public float bulletForce = 20f;
    public float fireRate = 0.2f;
    private float nextFireTime = 0f;
    
    // Bileşenler
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    
    // Karakter yönü
    private bool isFacingRight = true;
    
    // Animator parametreleri
    private readonly string speedParameter = "Speed";
    private readonly string shootParameter = "Shoot";
    private readonly string hurtParameter = "Hurt";
    private readonly string dieParameter = "Die";
    
    void Start()
    {
        // Bileşenleri al
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        // Değişkenleri başlat
        currentHealth = maxHealth;
        currentAmmo = maxAmmo;
    }
    
    void Update()
    {
        // Yatay hareket
        float moveHorizontal = Input.GetAxis("Horizontal");
        
        // Animasyon parametresini güncelle
        if (animator != null)
        {
            animator.SetFloat(speedParameter, Mathf.Abs(moveHorizontal));
        }
        
        // Karakter yönünü belirle
        if (moveHorizontal > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (moveHorizontal < 0 && isFacingRight)
        {
            Flip();
        }
        
        // Ateş etme
        if (Input.GetButtonDown("Fire1") && Time.time >= nextFireTime && currentAmmo > 0)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
        
        // Hareketi uygula
        rb.linearVelocity = new Vector2(moveHorizontal * moveSpeed, rb.linearVelocity.y);
    }
    
    void Flip()
    {
        isFacingRight = !isFacingRight;
        
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        
        // FirePoint'in yönünü de ayarla (eğer varsa)
        if (firePoint != null)
        {
            Vector3 firePointPos = firePoint.localPosition;
            firePointPos.x = Mathf.Abs(firePointPos.x) * (isFacingRight ? 1 : -1);
            firePoint.localPosition = firePointPos;
        }
    }
    
    void Shoot()
    {
        // Mermi sayısını azalt
        currentAmmo--;
        
        // Ateş etme animasyonunu tetikle
        if (animator != null)
        {
            animator.SetTrigger(shootParameter);
        }
        
        // Mermi oluştur (bulletPrefab daha sonra oluşturulacak)
        if (bulletPrefab != null && firePoint != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody2D rbBullet = bullet.GetComponent<Rigidbody2D>();
            
            if (rbBullet != null)
            {
                rbBullet.AddForce(firePoint.right * bulletForce * (isFacingRight ? 1 : -1), ForceMode2D.Impulse);
            }
            
            // Mermiyi birkaç saniye sonra yok et
            Destroy(bullet, 2f);
        }
    }
    
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        
        // Hasar alma animasyonunu tetikle
        if (animator != null)
        {
            animator.SetTrigger(hurtParameter);
        }
        
        // Öldü mü kontrol et
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        // Ölüm animasyonunu tetikle
        if (animator != null)
        {
            animator.SetTrigger(dieParameter);
        }
        
        // Oyuncuyu devre dışı bırak
        enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.bodyType = RigidbodyType2D.Static;
        GetComponent<Collider2D>().enabled = false;
        
        // GameManager'a ölüm bilgisini gönder (güvenli bir şekilde)
        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.GameOver();
        }
        else
        {
            Debug.LogWarning("GameManager bulunamadı!");
        }
    }
    
    public void AddAmmo(int amount)
    {
        currentAmmo = Mathf.Min(currentAmmo + amount, maxAmmo);
    }
    
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
    }
    
    // Getter metodları
    public int GetCurrentHealth() { return currentHealth; }
    public int GetMaxHealth() { return maxHealth; }
    public int GetCurrentAmmo() { return currentAmmo; }
    public int GetMaxAmmo() { return maxAmmo; }
}