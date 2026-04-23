using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public RaceType race;
    public EnergyType primaryEnergy;
    public EnergyType secondaryEnergy;
    public EnergyType tertiaryEnergy;
    public string magicName;
    public bool hasSpecialAbility;
    
    private CharacterGenerator characterGenerator;
    private CombatSystem combatSystem;
    
    [Header("Combat Settings")]
    public float meleeRange = 2f;
    public float rangedSpeed = 10f;
    public float rangedDamage = 10f;
    public float meleeDamage = 5f;
    
    [Header("References")]
    public Transform attackPoint;
    public GameObject rangedProjectilePrefab;
    public LayerMask enemyLayers;
    
    void Start()
    {
        characterGenerator = GetComponent<CharacterGenerator>();
        combatSystem = GetComponent<CombatSystem>();
        
        if (attackPoint == null)
        {
            attackPoint = transform;
        }
    }
    
    void Update()
    {
        HandleCombatInput();
        HandleSpecialInput();
    }
    
    void HandleCombatInput()
    {
        // ЛКМ - Атака ближнего боя
        if (Input.GetMouseButtonDown(0))
        {
            PerformMeleeAttack();
        }
        
        // ПКМ - Атака дальнего боя
        if (Input.GetMouseButtonDown(1))
        {
            PerformRangedAttack();
        }
        
        // Пробел - Защита
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PerformDefense();
        }
    }
    
    void HandleSpecialInput()
    {
        // E - Магия
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseMagic();
        }
        
        // Q - Особый прием (только если есть энергия)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            UseSpecialAbility();
        }
        
        // F - Действие
        if (Input.GetKeyDown(KeyCode.F))
        {
            PerformAction();
        }
        
        // T - Пощада
        if (Input.GetKeyDown(KeyCode.T))
        {
            ShowMercy();
        }
        
        // O - Перекрутить энергию (только для DistortedChaos)
        if (Input.GetKeyDown(KeyCode.O))
        {
            RerollDistortedEnergy();
        }
    }
    
    void PerformMeleeAttack()
    {
        Debug.Log("Атака ближнего боя!");
        
        if (combatSystem != null)
        {
            combatSystem.PerformMeleeAttack(meleeDamage, meleeRange);
        }
        
        // Визуальный эффект атаки
        PerformMeleeVisualEffect();
    }
    
    void PerformRangedAttack()
    {
        Debug.Log("Атака дальнего боя!");
        
        if (rangedProjectilePrefab != null && combatSystem != null)
        {
            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPosition.z = 0;
            
            combatSystem.PerformRangedAttack(rangedProjectilePrefab, targetPosition, rangedSpeed, rangedDamage);
        }
        else
        {
            // Если нет префаба, создаем простой снаряд
            CreateSimpleProjectile();
        }
    }
    
    void CreateSimpleProjectile()
    {
        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0;
        
        Vector3 direction = (targetPosition - transform.position).normalized;
        
        Debug.DrawRay(transform.position, direction * 10f, Color.red, 2f);
        Debug.Log($"Снаряд летит в направлении: {direction}");
    }
    
    void PerformDefense()
    {
        Debug.Log("Защита!");
        
        if (combatSystem != null)
        {
            combatSystem.ActivateDefense();
        }
    }
    
    void UseMagic()
    {
        if (primaryEnergy == EnergyType.None)
        {
            Debug.Log("Недостаточно энергии для использования магии!");
            return;
        }
        
        Debug.Log($"Использование магии: {magicName}");
        // Здесь будет логика использования магии
    }
    
    void UseSpecialAbility()
    {
        if (!hasSpecialAbility)
        {
            Debug.Log("Особый прием недоступен!");
            return;
        }
        
        if (primaryEnergy == EnergyType.None)
        {
            Debug.Log("Недостаточно энергии для особого приема!");
            return;
        }
        
        Debug.Log("Использование особого приема!");
        
        switch (race)
        {
            case RaceType.SealedHuman:
                Debug.Log("[Магическая перезапись] - изменение реальности!");
                break;
            case RaceType.SeparatedAmalgama:
                Debug.Log("[Королевская зона] - подавление энергий!");
                break;
            case RaceType.FormedAnomaly:
                Debug.Log("[Кузница техники] - создание предмета!");
                break;
            default:
                Debug.Log("Стандартный особый прием!");
                break;
        }
    }
    
    void PerformAction()
    {
        Debug.Log("Действие!");
        // Взаимодействие с миром
    }
    
    void ShowMercy()
    {
        Debug.Log("Пощада - отказ от насилия!");
        // Логика переговоров
    }
    
    void RerollDistortedEnergy()
    {
        if (race == RaceType.DistortedChaos)
        {
            if (characterGenerator != null)
            {
                characterGenerator.RerollDistortedEnergy();
            }
        }
        else
        {
            Debug.Log("Только существо хаоса искаженной энергии может менять энергию!");
        }
    }
    
    void PerformMeleeVisualEffect()
    {
        // Создаем эффект атаки ближнего боя
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, meleeRange, enemyLayers);
        
        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.DrawLine(attackPoint.position, enemy.transform.position, Color.red, 0.5f);
        }
        
        // Визуализация радиуса атаки
        Debug.DrawCircle(attackPoint.position, meleeRange, Color.yellow, 0.2f);
    }
    
    public void Initialize(RaceType newRace, EnergyType newPrimaryEnergy, 
                          EnergyType newSecondaryEnergy, EnergyType newTertiaryEnergy,
                          string newMagicName, bool newHasSpecialAbility)
    {
        race = newRace;
        primaryEnergy = newPrimaryEnergy;
        secondaryEnergy = newSecondaryEnergy;
        tertiaryEnergy = newTertiaryEnergy;
        magicName = newMagicName;
        hasSpecialAbility = newHasSpecialAbility;
    }
    
    void OnDrawGizmosSelected()
    {
        // Отрисовка радиуса атаки в редакторе
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPoint != null ? attackPoint.position : transform.position, meleeRange);
    }
}
