using UnityEngine;

/// <summary>
/// Компонент Аномальной Зоны
/// </summary>
public class AnomalyZone : MonoBehaviour
{
    [Header("Zone Settings")]
    [SerializeField] private AnomalyZoneType zoneType;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float energyCost = 10f; // Затраты энергии вместо длительности
    
    [Header("Zone Effects")]
    [SerializeField] private bool isActive = true;
    [SerializeField] private MagicType zoneMagicType;
    [SerializeField] private string technicalRules = "";
    
    [Header("Absolute Zone Info")]
    [SerializeField] private string absoluteZoneName = "";
    [SerializeField] private bool isAbsoluteZone = false;
    
    private EnergySystem ownerEnergySystem;
    private CircleCollider2D zoneCollider;
    
    /// <summary>
    /// Инициализация зоны
    /// </summary>
    public void Initialize(AnomalyZoneType type, EnergySystem owner)
    {
        zoneType = type;
        ownerEnergySystem = owner;
        
        zoneCollider = GetComponent<CircleCollider2D>();
        if (zoneCollider != null)
        {
            radius = zoneCollider.radius;
        }
        
        ApplyZoneEffects();
    }
    
    /// <summary>
    /// Установить название для Абсолютной зоны
    /// </summary>
    public void SetAbsoluteZoneName(string name)
    {
        absoluteZoneName = name;
        isAbsoluteZone = true;
        Debug.Log($"Абсолютная АЗ \"{name}\" активирована!");
    }
    
    /// <summary>
    /// Применить эффекты зоны в зависимости от типа
    /// </summary>
    private void ApplyZoneEffects()
    {
        switch (zoneType)
        {
            case AnomalyZoneType.Empty:
                ApplyEmptyZone();
                break;
                
            case AnomalyZoneType.Simple:
                ApplySimpleZone();
                break;
                
            case AnomalyZoneType.Technical:
                ApplyTechnicalZone();
                break;
                
            case AnomalyZoneType.Absolute:
                ApplyAbsoluteZone();
                break;
        }
    }
    
    /// <summary>
    /// Пустая АЗ - нейтрализует техники
    /// </summary>
    private void ApplyEmptyZone()
    {
        Debug.Log("Пустая АЗ: Нейтрализация техник внутри зоны");
        // Нейтрализация Аномальной и Амальгамной энергии
        // Плохо работает против техник Человека (имеют форму)
    }
    
    /// <summary>
    /// Простая АЗ - пассивное действие магии
    /// </summary>
    private void ApplySimpleZone()
    {
        Debug.Log($"Простая АЗ: Пассивное действие магии {zoneMagicType}");
        // Пространство меняет цвет под магию
    }
    
    /// <summary>
    /// Техническая АЗ - действие по правилам
    /// </summary>
    private void ApplyTechnicalZone()
    {
        Debug.Log($"Техническая АЗ: Действие по правилам - {technicalRules}");
        // Пример: остановка времени только для магии
    }
    
    /// <summary>
    /// Абсолютная АЗ - полное искажение пространства
    /// </summary>
    private void ApplyAbsoluteZone()
    {
        Debug.Log($"Абсолютная АЗ \"{absoluteZoneName}\": Полное искажение пространства");
        // Забивает пассивным применением и техническими особенностями
        
        // Известные Абсолютные АЗ:
        // - "Божественная чистка" Коджекса
        // - "Танец лезвий" Джека
        // - "Гниющий сад воспоминаний" Ростка
        // - "Бесконечная сплавка" Барреро (не завершена)
    }
    
    private void Update()
    {
        if (!isActive) return;
        
        // Зоны не имеют длительности - они активны пока не будут деактивированы
        // Энергия тратится при создании зоны, не каждый кадр
    }
    
    /// <summary>
    /// Обработка попадания объекта в зону
    /// </summary>
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log($"Объект {other.name} вошел в зону {zoneType}");
        
        // Применение эффектов зоны к объекту
        ApplyZoneEffectToObject(other);
    }
    
    /// <summary>
    /// Обработка выхода объекта из зоны
    /// </summary>
    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log($"Объект {other.name} вышел из зоны {zoneType}");
    }
    
    /// <summary>
    /// Применить эффект зоны к объекту
    /// </summary>
    private void ApplyZoneEffectToObject(Collider2D target)
    {
        var targetEnergy = target.GetComponent<EnergySystem>();
        var targetMagic = target.GetComponent<MagicSystem>();
        var targetRace = target.GetComponent<RaceSystem>();
        
        switch (zoneType)
        {
            case AnomalyZoneType.Empty:
                HandleEmptyZoneEffect(targetEnergy, targetMagic);
                break;
                
            case AnomalyZoneType.Simple:
                HandleSimpleZoneEffect(targetEnergy, targetMagic);
                break;
                
            case AnomalyZoneType.Technical:
                HandleTechnicalZoneEffect(targetEnergy, targetMagic);
                break;
                
            case AnomalyZoneType.Absolute:
                HandleAbsoluteZoneEffect(targetEnergy, targetMagic, targetRace);
                break;
        }
    }
    
    /// <summary>
    /// Эффект Пустой АЗ
    /// </summary>
    private void HandleEmptyZoneEffect(EnergySystem energy, MagicSystem magic)
    {
        if (energy == null) return;
        
        // Размывает Амальгамную и Аномальную энергию
        if (energy.PrimaryEnergyType == EnergyType.Amalgam || 
            energy.PrimaryEnergyType == EnergyType.Anomaly)
        {
            Debug.Log("Пустая АЗ размывает энергию!");
            // energy.DrainEnergy(10f);
        }
        
        // Техники Человека имеют форму - работают плохо
        if (energy.PrimaryEnergyType == EnergyType.Human)
        {
            Debug.Log("Пустая АЗ слабо влияет на Человеческую энергию (техники имеют форму)");
        }
    }
    
    /// <summary>
    /// Эффект Простой АЗ
    /// </summary>
    private void HandleSimpleZoneEffect(EnergySystem energy, MagicSystem magic)
    {
        // Пассивное применение магии в зоне
        if (magic != null)
        {
            Debug.Log("Простая АЗ применяет пассивный эффект магии");
        }
    }
    
    /// <summary>
    /// Эффект Технической АЗ
    /// </summary>
    private void HandleTechnicalZoneEffect(EnergySystem energy, MagicSystem magic)
    {
        // Действие по определенным правилам
        Debug.Log($"Техническая АЗ применяет правило: {technicalRules}");
        
        // Пример: остановка времени только для магии (Глава 16: Парадоксер)
    }
    
    /// <summary>
    /// Эффект Абсолютной АЗ
    /// </summary>
    private void HandleAbsoluteZoneEffect(EnergySystem energy, MagicSystem magic, RaceSystem race)
    {
        Debug.Log($"Абсолютная АЗ \"{absoluteZoneName}\" воздействует на цель!");
        
        // Полное искажение пространства
        // Комбинация пассивного применения и технических особенностей
    }
    
    /// <summary>
    /// Столкновение с другой Аномальной зоной
    /// </summary>
    private void HandleZoneCollision(AnomalyZone otherZone)
    {
        Debug.Log($"Столкновение зон: {zoneType} vs {otherZone.zoneType}");
        
        if (zoneType == AnomalyZoneType.Empty)
        {
            // Пустая АЗ имеет 2 пути:
            // 1. Держится, блокируя действие чужой АЗ (Глава 17)
            // 2. Полностью подавляет чужую АЗ (Глава 16)
            
            Debug.Log("Пустая АЗ пытается заблокировать/подавить чужую АЗ");
        }
    }
    
    /// <summary>
    /// Деактивировать зону
    /// </summary>
    public void DeactivateZone()
    {
        isActive = false;
        Debug.Log($"Зона {zoneType} деактивирована");
        
        // Удаление объекта зоны
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Получить информацию о зоне
    /// </summary>
    public string GetZoneInfo()
    {
        string info = $"Тип зоны: {zoneType}\n";
        info += $"Радиус: {radius}\n";
        info += $"Затраты энергии: {energyCost}\n";
        info += $"Активна: {isActive}\n";
        
        if (isAbsoluteZone)
        {
            info += $"Название АЗ: {absoluteZoneName}\n";
        }
        
        return info;
    }
}
