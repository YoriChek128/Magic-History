using System;
using UnityEngine;

/// <summary>
/// Типы магических техник (полный список, не зависит от расы)
/// </summary>
public enum MagicType
{
    None,
    
    // === СТИХИЯ (23 магии) ===
    Fire, Water, Ice, Earth, Air, Lightning, Darkness, Light, Blood, Cosmos, 
    Sand, Magma, Ink, Acid, Iron, Shadow, Threads, Nature, Poison, Gas, Dust, Glass,
    
    // === ТЕХНИЧЕСКАЯ (13 магий) ===
    RobotControl, Surgery, PhysicalForce, Glitches, RealityHack, Magnetism, 
    Laser, EnergyPlates, Radiation, Analytism, Collision, Interface, BodyManipulation,
    
    // === ПРОСТРАНСТВО (9 магий) ===
    Telekinesis, Barriers, Portals, SpaceBlade, Gravity, Movement, Illusions, 
    MirrorReality, Space,
    
    // === ВРЕМЯ (9 магий) ===
    Prediction, Chronos, TimeStop, TimeRewind, TimeSkip, TimeCycle, 
    TimeAcceleration, TimeMirrors, Time,
    
    // === ОСОБАЯ (16 магий) ===
    LifeBlade, Corruption, Explosion, Slash, Impulse, Disease, PuppetCreation, 
    GhostControl, EnergyMelting, ChainMagic, WeatherControl, Healing, Vampire, 
    PaintMagic, TechniqueCopy, HeavenlyPunishment,
    
    // === БОЖЕСТВЕННАЯ (12 магий) ===
    Creation, Destruction, Chaos, Rebirth, Annihilation, Mind, Soul, 
    Denial, Emotions, Code, Control, MainHeroDirector
}

/// <summary>
/// Тема магической техники
/// </summary>
public enum MagicTheme
{
    Element,        // Стихия (23 магии)
    Technical,      // Техническая (13 магий)
    Space,          // Пространство (9 магий)
    Time,           // Время (9 магий)
    Special,        // Особая (16 магий)
    Divine          // Божественная (12 магий)
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
    /// Определить тему магии по типу (полный список из 82 магий)
    /// </summary>
    public static MagicTheme GetThemeByType(MagicType type)
    {
        // === СТИХИЯ (23 магии) ===
        switch (type)
        {
            case MagicType.Fire:
            case MagicType.Water:
            case MagicType.Ice:
            case MagicType.Earth:
            case MagicType.Air:
            case MagicType.Lightning:
            case MagicType.Darkness:
            case MagicType.Light:
            case MagicType.Blood:
            case MagicType.Cosmos:
            case MagicType.Sand:
            case MagicType.Magma:
            case MagicType.Ink:
            case MagicType.Acid:
            case MagicType.Iron:
            case MagicType.Shadow:
            case MagicType.Threads:
            case MagicType.Nature:
            case MagicType.Poison:
            case MagicType.Gas:
            case MagicType.Dust:
            case MagicType.Glass:
                return MagicTheme.Element;
                
            // === ТЕХНИЧЕСКАЯ (13 магий) ===
            case MagicType.RobotControl:
            case MagicType.Surgery:
            case MagicType.PhysicalForce:
            case MagicType.Glitches:
            case MagicType.RealityHack:
            case MagicType.Magnetism:
            case MagicType.Laser:
            case MagicType.EnergyPlates:
            case MagicType.Radiation:
            case MagicType.Analytism:
            case MagicType.Collision:
            case MagicType.Interface:
            case MagicType.BodyManipulation:
                return MagicTheme.Technical;
                
            // === ПРОСТРАНСТВО (9 магий) ===
            case MagicType.Telekinesis:
            case MagicType.Barriers:
            case MagicType.Portals:
            case MagicType.SpaceBlade:
            case MagicType.Gravity:
            case MagicType.Movement:
            case MagicType.Illusions:
            case MagicType.MirrorReality:
            case MagicType.Space:
                return MagicTheme.Space;
                
            // === ВРЕМЯ (9 магий) ===
            case MagicType.Prediction:
            case MagicType.Chronos:
            case MagicType.TimeStop:
            case MagicType.TimeRewind:
            case MagicType.TimeSkip:
            case MagicType.TimeCycle:
            case MagicType.TimeAcceleration:
            case MagicType.TimeMirrors:
            case MagicType.Time:
                return MagicTheme.Time;
                
            // === ОСОБАЯ (16 магий) ===
            case MagicType.LifeBlade:
            case MagicType.Corruption:
            case MagicType.Explosion:
            case MagicType.Slash:
            case MagicType.Impulse:
            case MagicType.Disease:
            case MagicType.PuppetCreation:
            case MagicType.GhostControl:
            case MagicType.EnergyMelting:
            case MagicType.ChainMagic:
            case MagicType.WeatherControl:
            case MagicType.Healing:
            case MagicType.Vampire:
            case MagicType.PaintMagic:
            case MagicType.TechniqueCopy:
            case MagicType.HeavenlyPunishment:
                return MagicTheme.Special;
                
            // === БОЖЕСТВЕННАЯ (12 магий) ===
            case MagicType.Creation:
            case MagicType.Destruction:
            case MagicType.Chaos:
            case MagicType.Rebirth:
            case MagicType.Annihilation:
            case MagicType.Mind:
            case MagicType.Soul:
            case MagicType.Denial:
            case MagicType.Emotions:
            case MagicType.Code:
            case MagicType.Control:
            case MagicType.MainHeroDirector:
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
    
    public event Action<MagicType> OnTechniqueUsed;
    public event Action<AnomalyZoneType> OnZoneOpened;
    
    private EnergySystem energySystem;
    
    // Известные абсолютные зоны
    private static readonly string[] knownAbsoluteZones = new string[]
    {
        "Божественная чистка",    // Коджекс
        "Танец лезвий",          // Джек
        "Гниющий сад воспоминаний" // Росток
    };
    
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
    
    /// <summary>
    /// Получить список известных Абсолютных зон
    /// </summary>
    public static string[] GetKnownAbsoluteZones()
    {
        return knownAbsoluteZones;
    }
    
    #endregion
    
    #region Race-Specific Abilities
    
    /// <summary>
    /// [Магическая перезапись] - особый прием Заключенного человека
    /// </summary>
    public bool MagicalRewrite(object target)
    {
        // Объединение Аномальной и Амальгамной энергий
        if (energySystem.PrimaryEnergyType != EnergyType.Chaos)
        {
            Debug.LogWarning("Только Заключенный человек может использовать Магическую перезапись!");
            return false;
        }
        
        Debug.Log($"[Магическая перезапись] активирована!");
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
    public GameObject ForgeTechniqueItem(string itemName)
    {
        Debug.Log($"[Кузница техники] создается: {itemName}");
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
