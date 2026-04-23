using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class MeleeHitbox : MonoBehaviour
{
    [Header("Settings")]
    public float damage = 10f;
    public bool isActive = false;
    
    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public Color activeColor = Color.red;
    public Color inactiveColor = Color.clear;
    
    private BoxCollider2D boxCollider;
    
    void Awake()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        boxCollider.isTrigger = true;
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
            
        SetActive(false);
    }
    
    public void Activate(float duration = 0.2f)
    {
        isActive = true;
        SetActive(true);
        
        Invoke(nameof(Deactivate), duration);
    }
    
    void Deactivate()
    {
        isActive = false;
        SetActive(false);
    }
    
    void SetActive(bool active)
    {
        boxCollider.enabled = active;
        
        if (spriteRenderer != null)
        {
            Color color = active ? activeColor : inactiveColor;
            spriteRenderer.color = color;
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;
        
        IDamageable damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(damage);
            Debug.Log($"Хитбокс нанес {damage} урона врагу: {other.name}");
        }
    }
}
