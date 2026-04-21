using UnityEngine;
using UnityEngine.Events;
/// <summary>
/// Типы магических техник
/// </summary>
public enum MagicType
{
    None,
    
    // Стихия
    Fire,
    Water,
    Ice,
    Cosmos,
    Acid,
    Nature,
    Sand,
    Air,
    Lightning,
    Darkness,
    
    // Техническая
    RealityHacker,
    Surgery,
    RobotControl,
    Laser,
    
    // Пространство
    Gravity,
    Telekinesis,
    Teleport,
    
    // Время
    TimeReturn,
    Chronos,
    Skip,
    Acceleration,
    
    // Особая
    Healing,
    Vampirism,
    EnergyMelting,
    Explosions,
    Slashes,
    
    // Божественная
    Creation,
    Destruction,
    Control,
    Mind,
    Negation,
    
    // Специальные (гибриды и уникальные)
    Special
}

/// <summary>
/// Тема магической техники
/// </summary>
public enum MagicTheme
{
    Element,        // Стихия
    Technical,      // Техническая
    Space,          // Пространство
    Time,           // Время
    Special,        // Особая
    Divine          // Божественная
}

/// <summary>
/// Типы Аномальных Зон
/// </summary>
public enum AnomalyZoneType
{
    Empty,      // Пустая АЗ - нейтрализует техники
    Simple,     // Простая АЗ - пассивное действие магии
    Technical,  // Техническая АЗ - действие по правилам
    Absolute    // Абсолютная АЗ - полное искажение пространства
}

/// <summary>
/// Класс магической техники
/// </summary>
[System.Serializable]
public class MagicTechnique
{
    public string techniqueName;
    public MagicType magicType;
    public MagicTheme theme;
    public int manaCost;
    public float cooldown;
    public string description;
    
    // Для божественной магии - аспект мира
    public string divineAspect;
    
    public MagicTechnique(string name, MagicType type, MagicTheme theme, int cost)
    {
        techniqueName = name;
        magicType = type;
        this.theme = theme;
        manaCost = cost;
        cooldown = 1f;
        description = "";
        divineAspect = "";
    }
    
    /// <summary>
    /// Определить тему магии по типу
    /// </summary>
    public static MagicTheme GetThemeByType(MagicType type)
    {
        switch (type)
        {
            case MagicType.Fire:
            case MagicType.Water:
            case MagicType.Ice:
            case MagicType.Cosmos:
            case MagicType.Acid:
            case MagicType.Nature:
            case MagicType.Sand:
            case MagicType.Air:
            case MagicType.Lightning:
            case MagicType.Darkness:
                return MagicTheme.Element;
                
            case MagicType.RealityHacker:
            case MagicType.Surgery:
            case MagicType.RobotControl:
            case MagicType.Laser:
                return MagicTheme.Technical;
                
            case MagicType.Gravity:
            case MagicType.Telekinesis:
            case MagicType.Teleport:
                return MagicTheme.Space;
                
            case MagicType.TimeReturn:
            case MagicType.Chronos:
            case MagicType.Skip:
            case MagicType.Acceleration:
                return MagicTheme.Time;
                
            case MagicType.Healing:
            case MagicType.Vampirism:
            case MagicType.EnergyMelting:
            case MagicType.Explosions:
            case MagicType.Slashes:
                return MagicTheme.Special;
                
            case MagicType.Creation:
            case MagicType.Destruction:
            case MagicType.Control:
            case MagicType.Mind:
            case MagicType.Negation:
                return MagicTheme.Divine;
                
            default:
                return MagicTheme.Special;
        }
    }
}

/// <summary>
/// Компонент управления магией персонажа
/// </summary>
public class MagicSystem : MonoBehaviour
{
    [Header("Magic Settings")]
    [SerializeField] private bool hasMagicAbility = false;
    [SerializeField] private MagicType[] knownTechniques;
    [SerializeField] private int masteryLevel = 0;
    
    [Header("Anomaly Zone Settings")]
    [SerializeField] private AnomalyZoneType zoneMastery = AnomalyZoneType.Empty;
    [SerializeField] private int trainingMonths = 0;
    
    [Header("Absolute Zone Info")]
    [SerializeField] private string absoluteZoneName = "";
    [SerializeField] private int yearsToMaster = 0;
    
    public event UnityAction<MagicType> OnTechniqueUsed;
    public event UnityAction<AnomalyZoneType> OnZoneOpened;
    
    private EnergySystem energySystem;
    
    private void Start()
    {
        energySystem = GetComponent<EnergySystem>();
    }
    
    /// <summary>
    /// Проверить возможность использования магии
    /// </summary>
    public bool CanUseMagic()
    {
        return hasMagicAbility && energySystem != null && energySystem.CurrentEnergy > 0;
    }
    
    /// <summary>
    /// Использовать технику
    /// </summary>
    public bool UseTechnique(MagicType techniqueType, Vector3 targetPosition)
    {
        if (!CanUseMagic())
        {
            Debug.LogWarning("Невозможно использовать магию!");
            return false;
        }
        
        if (!IsTechniqueKnown(techniqueType))
        {
            Debug.LogWarning($"Техника {techniqueType} не изучена!");
            return false;
        }
        
        MagicTechnique technique = GetTechniqueInfo(techniqueType);
        
        if (!energySystem.TryConsumeEnergy(technique.manaCost))
        {
            Debug.LogWarning("Недостаточно энергии!");
            return false;
        }
        
        ExecuteTechnique(technique, targetPosition);
        OnTechniqueUsed?.Invoke(techniqueType);
        
        return true;
    }
    
    /// <summary>
    /// Проверить, известна ли техника
    /// </summary>
    public bool IsTechniqueKnown(MagicType techniqueType)
    {
        foreach (var technique in knownTechniques)
        {
            if (technique == techniqueType)
                return true;
        }
        return false;
    }
    
    /// <summary>
    /// Получить информацию о технике
    /// </summary>
    public MagicTechnique GetTechniqueInfo(MagicType techniqueType)
    {
        MagicTheme theme = MagicTechnique.GetThemeByType(techniqueType);
        return new MagicTechnique(techniqueType.ToString(), techniqueType, theme, 10);
    }
    
    /// <summary>
    /// Выполнить технику (заготовка для реализации)
    /// </summary>
    private void ExecuteTechnique(MagicTechnique technique, Vector3 targetPosition)
    {
        Debug.Log($"Использована техника: {technique.techniqueName} ({technique.theme})");
        
        // Здесь должна быть логика выполнения техники в зависимости от темы
        switch (technique.theme)
        {
            case MagicTheme.Element:
                ExecuteElementTechnique(technique.magicType, targetPosition);
                break;
            case MagicTheme.Technical:
                ExecuteTechnicalTechnique(technique.magicType, targetPosition);
                break;
            case MagicTheme.Space:
                ExecuteSpaceTechnique(technique.magicType, targetPosition);
                break;
            case MagicTheme.Time:
                ExecuteTimeTechnique(technique.magicType, targetPosition);
                break;
            case MagicTheme.Special:
                ExecuteSpecialTechnique(technique.magicType, targetPosition);
                break;
            case MagicTheme.Divine:
                ExecuteDivineTechnique(technique.magicType, targetPosition);
                break;
        }
    }
    
    private void ExecuteElementTechnique(MagicType type, Vector3 pos)
    {
        Debug.Log($"Стихийная техника: {type} в точке {pos}");
        // Реализация стихийных техник
    }
    
    private void ExecuteTechnicalTechnique(MagicType type, Vector3 pos)
    {
        Debug.Log($"Техническая техника: {type} в точке {pos}");
        // Реализация технических техник
    }
    
    private void ExecuteSpaceTechnique(MagicType type, Vector3 pos)
    {
        Debug.Log($"Пространственная техника: {type} в точке {pos}");
        // Реализация пространственных техник
    }
    
    private void ExecuteTimeTechnique(MagicType type, Vector3 pos)
    {
        Debug.Log($"Временная техника: {type} в точке {pos}");
        // Реализация временных техник
    }
    
    private void ExecuteSpecialTechnique(MagicType type, Vector3 pos)
    {
        Debug.Log($"Особая техника: {type} в точке {pos}");
        // Реализация особых техник
    }
    
    private void ExecuteDivineTechnique(MagicType type, Vector3 pos)
    {
        Debug.Log($"Божественная техника: {type} в точке {pos}");
        // Реализация божественных техник
    }
    
    #region Anomaly Zone Methods
    
    /// <summary>
    /// Открыть Аномальную зону с проверкой мастерства
    /// </summary>
    public bool OpenZone(AnomalyZoneType zoneType, Vector3 center, float radius)
    {
        if (!CanUseMagic()) return false;
        
        // Проверка мастерства для разных типов зон
        if (!CanOpenZone(zoneType))
        {
            Debug.LogWarning("Недостаточно мастерства для открытия этой зоны!");
            return false;
        }
        
        bool result = energySystem.OpenAnomalyZone(zoneType, center, radius);
        
        if (result)
        {
            OnZoneOpened?.Invoke(zoneType);
        }
        
        return result;
    }
    
    /// <summary>
    /// Проверить возможность открытия зоны
    /// </summary>
    public bool CanOpenZone(AnomalyZoneType zoneType)
    {
        switch (zoneType)
        {
            case AnomalyZoneType.Empty:
                return true; // Не требует особого мастерства
                
            case AnomalyZoneType.Simple:
                return true; // Не требует особого мастерства
                
            case AnomalyZoneType.Technical:
                return trainingMonths >= 3; // ~3 месяца тренировок
                
            case AnomalyZoneType.Absolute:
                return yearsToMaster >= 10; // Несколько десятков лет
        }
        
        return false;
    }
    
    /// <summary>
    /// Установить имя Абсолютной зоны
    /// </summary>
    public void NameAbsoluteZone(string name)
    {
        if (zoneMastery == AnomalyZoneType.Absolute)
        {
            absoluteZoneName = name;
            Debug.Log($"Абсолютная АЗ названа: {name}");
        }
    }
    
    #endregion
    
    #region Race-Specific Abilities
    
    /// <summary>
    /// [Магическая перезапись] - особый прием Заключенного человека
    /// </summary>
    public bool MagicalRewrite(MagicType technique, object target)
    {
        // Объединение Аномальной и Амальгамной энергий
        if (energySystem.PrimaryEnergyType != EnergyType.Chaos)
        {
            Debug.LogWarning("Только Заключенный человек может использовать Магическую перезапись!");
            return false;
        }
        
        Debug.Log($"[Магическая перезапись] активирована: {technique}");
        // Изменение реальности под правило магии
        return true;
    }
    
    /// <summary>
    /// [Королевская зона] - особый прием Раздельной амальгамы
    /// </summary>
    public bool RoyalZone(Vector3 area, float duration)
    {
        // Человек распространяет волю + Аномалия открывает зону
        Debug.Log("[Королевская зона] активирована!");
        Debug.Log("Человеческая молния + Аномальная зона = подавление всех энергий");
        return true;
    }
    
    /// <summary>
    /// [Кузница техники] - особый прием Сформированной аномалии
    /// </summary>
    public GameObject ForgeTechniqueItem(MagicType technique, string itemName)
    {
        Debug.Log($"[Кузница техники] создается: {itemName} на основе {technique}");
        // Создание оружия, предмета, артефакта, брони в рамках техники
        return new GameObject(itemName);
    }
    
    #endregion
    
    /// <summary>
    /// Добавить технику в список известных
    /// </summary>
    public void LearnTechnique(MagicType techniqueType)
    {
        if (!IsTechniqueKnown(techniqueType))
        {
            var newList = new MagicType[knownTechniques.Length + 1];
            knownTechniques.CopyTo(newList, 0);
            newList[newList.Length - 1] = techniqueType;
            knownTechniques = newList;
            
            Debug.Log($"Изучена новая техника: {techniqueType}");
        }
    }
    
    /// <summary>
    /// Получить информацию о магии для консоли
    /// </summary>
    public string GetMagicInfo()
    {
        string info = $"Has Magic: {hasMagicAbility}\n";
        info += $"Mastery Level: {masteryLevel}\n";
        info += $"Known Techniques: {knownTechniques.Length}\n";
        info += $"Zone Mastery: {zoneMastery}\n";
        info += $"Training Months: {trainingMonths}\n";
        info += $"Absolute Zone: {absoluteZoneName}\n";
        
        return info;
    }
}
