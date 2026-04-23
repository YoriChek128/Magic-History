using UnityEngine;
using System;

/// <summary>
/// Типы энергий в игре
/// </summary>
public enum EnergyType
{
    None,
    Human,        // Человеческая энергия (огонь)
    Amalgam,      // Амальгамная энергия (пластилин)
    Anomaly,      // Аномальная энергия (вода)
    Pranic,       // Праническая энергия (Божественная)
    Chaos         // Искаженная энергия хаоса
}

/// <summary>
/// Состояния человеческой энергии
/// </summary>
public enum HumanEnergyState
{
    Normal,       // Обычное состояние (огонь)
    Sparks,       // Искры (перепад температур)
    Lightning,    // Человеческая молния
    Explosion     // Человеческий взрыв (теоретический)
}

/// <summary>
/// Система управления энергией персонажа
/// </summary>
public class EnergySystem : MonoBehaviour
{
    [Header("Base Energy Settings")]
    [SerializeField] private EnergyType primaryEnergyType = EnergyType.None;
    [SerializeField] private float maxEnergy = 100f;
    [SerializeField] private float currentEnergy = 100f;
    [SerializeField] private float energyRegenPerSecond = 1f;

    [Header("Human Energy Specific")]
    [SerializeField] private HumanEnergyState humanEnergyState = HumanEnergyState.Normal;
    [SerializeField] private float heatLevel = 0f; // Уровень накала энергии

    [Header("Amalgam Energy Specific")]
    [SerializeField] private MagicType firstMagicType;
    [SerializeField] private MagicType secondMagicType;
    [SerializeField] private bool canCreateHybrids = true;

    [Header("Anomaly Energy Specific")]
    [SerializeField] private AnomalyZoneType activeZoneType = AnomalyZoneType.Empty;
    [SerializeField] private string absoluteZoneName = "";
    [SerializeField] private int masteryYears = 0;

    public event Action<EnergyType, float> OnEnergyChanged;
    public event Action<HumanEnergyState> OnHumanEnergyStateChanged;

    // Свойства
    public EnergyType PrimaryEnergyType => primaryEnergyType;
    public float MaxEnergy => maxEnergy;
    public float CurrentEnergy => currentEnergy;
    public float EnergyPercentage => currentEnergy / maxEnergy;
    public HumanEnergyState HumanEnergyState => humanEnergyState;
    public float HeatLevel => heatLevel;
    public MagicType FirstMagicType => firstMagicType;
    public MagicType SecondMagicType => secondMagicType;
    public AnomalyZoneType ActiveZoneType => activeZoneType;
    public string AbsoluteZoneName => absoluteZoneName;

    private void Start()
    {
        InitializeEnergy();
    }

    private void Update()
    {
        RegenerateEnergy();
    }

    private void InitializeEnergy()
    {
        currentEnergy = maxEnergy;
        
        // Настройка в зависимости от типа энергии
        switch (primaryEnergyType)
        {
            case EnergyType.Human:
                maxEnergy = 100f;
                humanEnergyState = HumanEnergyState.Normal;
                break;
            case EnergyType.Amalgam:
                maxEnergy = 150f;
                canCreateHybrids = true;
                break;
            case EnergyType.Anomaly:
                maxEnergy = 200f;
                activeZoneType = AnomalyZoneType.Empty;
                break;
            case EnergyType.Pranic:
                maxEnergy = float.MaxValue; // Божественная энергия безлимитна
                currentEnergy = float.MaxValue;
                break;
            case EnergyType.Chaos:
                maxEnergy = 100f;
                break;
        }
    }

    private void RegenerateEnergy()
    {
        if (primaryEnergyType == EnergyType.Pranic) return; // Божественная не регенерирует, она безлимитна

        if (currentEnergy < maxEnergy)
        {
            currentEnergy = Mathf.Min(currentEnergy + energyRegenPerSecond * Time.deltaTime, maxEnergy);
            OnEnergyChanged?.Invoke(primaryEnergyType, currentEnergy);
        }
    }

    /// <summary>
    /// Потратить энергию
    /// </summary>
    public bool TryConsumeEnergy(float amount)
    {
        if (primaryEnergyType == EnergyType.Pranic) return true; // Божественная энергия всегда доступна

        if (currentEnergy >= amount)
        {
            currentEnergy -= amount;
            OnEnergyChanged?.Invoke(primaryEnergyType, currentEnergy);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Добавить энергию
    /// </summary>
    public void AddEnergy(float amount)
    {
        if (primaryEnergyType == EnergyType.Pranic) return;

        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy);
        OnEnergyChanged?.Invoke(primaryEnergyType, currentEnergy);
    }

    #region Human Energy Methods

    /// <summary>
    /// Накалить человеческую энергию (повышает тепло)
    /// </summary>
    public void HeatUpHumanEnergy(float amount)
    {
        if (primaryEnergyType != EnergyType.Human) return;

        heatLevel += amount;
        UpdateHumanEnergyState();
    }

    /// <summary>
    /// Охладить человеческую энергию
    /// </summary>
    public void CoolDownHumanEnergy(float amount)
    {
        if (primaryEnergyType != EnergyType.Human) return;

        heatLevel = Mathf.Max(heatLevel - amount, 0f);
        UpdateHumanEnergyState();
    }

    private void UpdateHumanEnergyState()
    {
        HumanEnergyState previousState = humanEnergyState;

        if (heatLevel >= 100f)
        {
            humanEnergyState = HumanEnergyState.Explosion;
            Debug.LogWarning("Человеческий взрыв! Это состояние пока недостижимо для использования.");
        }
        else if (heatLevel >= 70f)
        {
            humanEnergyState = HumanEnergyState.Lightning;
        }
        else if (heatLevel >= 30f)
        {
            humanEnergyState = HumanEnergyState.Sparks;
        }
        else
        {
            humanEnergyState = HumanEnergyState.Normal;
        }

        if (previousState != humanEnergyState)
        {
            OnHumanEnergyStateChanged?.Invoke(humanEnergyState);
        }
    }

    /// <summary>
    /// Использовать человеческую молнию (только сильные существа)
    /// </summary>
    public bool TryUseHumanLightning(float energyCost)
    {
        if (humanEnergyState != HumanEnergyState.Lightning && 
            humanEnergyState != HumanEnergyState.Explosion)
            return false;

        return TryConsumeEnergy(energyCost);
    }

    /// <summary>
    /// Усилить ткани организма человеческой энергией
    /// </summary>
    public void EnhanceBodyWithHumanEnergy(float multiplier, float duration)
    {
        if (primaryEnergyType != EnergyType.Human) return;
        
        StartCoroutine(EnhanceBodyCoroutine(multiplier, duration));
    }

    private System.Collections.IEnumerator EnhanceBodyCoroutine(float multiplier, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            // Здесь должна быть логика усиления характеристик
            yield return null;
            elapsed += Time.deltaTime;
        }
    }

    #endregion

    #region Amalgam Energy Methods

    /// <summary>
    /// Создать гибридную технику из двух магических типов
    /// </summary>
    public MagicType CreateHybridMagic()
    {
        if (primaryEnergyType != EnergyType.Amalgam || !canCreateHybrids)
            return MagicType.None;

        // Логика создания гибрида (упрощенная)
        return MagicType.Special; // Возвращаем особую технику как гибрид
    }

    /// <summary>
    /// Изменить структуру тела (защита от физических атак без магии)
    /// </summary>
    public void ReshapeBody()
    {
        if (primaryEnergyType != EnergyType.Amalgam) return;
        
        // Логика изменения структуры тела
        Debug.Log("Амальгама изменила структуру тела - физические атаки бесполезны!");
    }

    #endregion

    #region Anomaly Energy Methods

    /// <summary>
    /// Открыть Аномальную зону
    /// </summary>
    public bool OpenAnomalyZone(AnomalyZoneType zoneType, Vector3 center, float radius)
    {
        if (primaryEnergyType != EnergyType.Anomaly) return false;

        float energyCost = GetZoneEnergyCost(zoneType);
        
        if (!TryConsumeEnergy(energyCost))
            return false;

        activeZoneType = zoneType;
        
        // Создание зоны
        GameObject zoneObject = new GameObject($"AnomalyZone_{zoneType}");
        zoneObject.transform.position = center;
        
        CircleCollider2D zoneCollider = zoneObject.AddComponent<CircleCollider2D>();
        zoneCollider.radius = radius;
        zoneCollider.isTrigger = true;
        
        AnomalyZone zoneComponent = zoneObject.AddComponent<AnomalyZone>();
        zoneComponent.Initialize(zoneType, this);

        Debug.Log($"Открыта Аномальная зона: {zoneType}");
        return true;
    }

    private float GetZoneEnergyCost(AnomalyZoneType zoneType)
    {
        switch (zoneType)
        {
            case AnomalyZoneType.Empty:
                return 10f;
            case AnomalyZoneType.Simple:
                return 20f;
            case AnomalyZoneType.Technical:
                return 50f;
            case AnomalyZoneType.Absolute:
                return 200f;
            default:
                return 10f;
        }
    }

    /// <summary>
    /// Проверка на возможность использования Абсолютной АЗ
    /// </summary>
    public bool CanUseAbsoluteZone()
    {
        return masteryYears >= 10 && primaryEnergyType == EnergyType.Anomaly;
    }

    /// <summary>
    /// Установить название Абсолютной АЗ
    /// </summary>
    public void SetAbsoluteZoneName(string name)
    {
        absoluteZoneName = name;
    }

    #endregion

    /// <summary>
    /// Установить тип энергии (для смены расы или пробуждения)
    /// </summary>
    public void SetEnergyType(EnergyType newType)
    {
        primaryEnergyType = newType;
        InitializeEnergy();
    }

    /// <summary>
    /// Получить информацию об энергии для консоли
    /// </summary>
    public string GetEnergyInfo()
    {
        return $"Energy: {primaryEnergyType} | {currentEnergy}/{maxEnergy}\n" +
               $"Human State: {humanEnergyState} | Heat: {heatLevel}\n" +
               $"Anomaly Zone: {activeZoneType} | Absolute: {absoluteZoneName}";
    }
}
