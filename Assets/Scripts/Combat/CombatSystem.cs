using UnityEngine;

public class CombatSystem : MonoBehaviour
{
    [Header("Melee Attack Settings")]
    public LayerMask enemyLayers;
    public Transform attackPoint;
    public float meleeRange = 2f;
    public float meleeDamage = 10f;
    
    [Header("Ranged Attack Settings")]
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float projectileSpeed = 15f;
    public float rangedDamage = 15f;
    
    [Header("Defense Settings")]
    public float defenseMultiplier = 0.5f;
    private bool isDefending = false;
    
    [Header("Visual Effects")]
    public GameObject hitEffectPrefab;
    public GameObject meleeEffectPrefab;
    
    void Start()
    {
        if (attackPoint == null)
            attackPoint = transform;
            
        if (firePoint == null)
            firePoint = transform;
    }
    
    public void PerformMeleeAttack(float damage, float range)
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, range, enemyLayers);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
                Debug.Log($"Нанесено {damage} урона врагу: {enemy.name}");
                
                if (hitEffectPrefab != null)
                {
                    Instantiate(hitEffectPrefab, enemy.transform.position, Quaternion.identity);
                }
            }
        }
        
        if (meleeEffectPrefab != null)
        {
            Instantiate(meleeEffectPrefab, attackPoint.position, Quaternion.identity);
        }
    }
    
    public void PerformRangedAttack(GameObject projectile, Vector3 targetPosition, float speed, float damage)
    {
        if (projectile == null)
        {
            Debug.LogError("Projectile prefab not assigned!");
            return;
        }
        
        Vector3 direction = (targetPosition - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        GameObject newProjectile = Instantiate(projectile, firePoint.position, Quaternion.Euler(0, 0, angle));
        
        Projectile projComponent = newProjectile.GetComponent<Projectile>();
        if (projComponent != null)
        {
            projComponent.Initialize(direction, speed, damage);
        }
        else
        {
            Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * speed;
            }
        }
        
        Debug.Log($"Снаряд выпущен в направлении: {direction}");
    }
    
    public void ActivateDefense()
    {
        isDefending = true;
        Debug.Log("Активирована защита! Получаемый урон снижен.");
        
        Invoke(nameof(DeactivateDefense), 1f);
    }
    
    void DeactivateDefense()
    {
        isDefending = false;
    }
    
    public float CalculateDamage(float incomingDamage)
    {
        if (isDefending)
        {
            return incomingDamage * defenseMultiplier;
        }
        return incomingDamage;
    }
    
    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            attackPoint = transform;
            
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint.position, meleeRange);
    }
}

public interface IDamageable
{
    void TakeDamage(float damage);
}
