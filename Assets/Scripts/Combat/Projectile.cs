using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 direction;
    private float speed;
    private float damage;
    private float lifetime = 5f;
    
    [Header("Visual Settings")]
    public SpriteRenderer spriteRenderer;
    public TrailRenderer trailRenderer;
    
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
    
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
    
    public void Initialize(Vector3 dir, float spd, float dmg)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
        
        if (spriteRenderer != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Debug.Log($"Снаряд попал в {other.name}, нанесено {damage} урона");
        }
        
        Destroy(gameObject);
    }
}
