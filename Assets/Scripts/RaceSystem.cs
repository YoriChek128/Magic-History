using UnityEngine;
using System;

/// <summary>
/// Типы рас в игре
/// </summary>
public enum RaceType
{
    // Основные расы
    Human,            // Человек - физическое существо с Человеческой энергией
    Amalgam,          // Амальгама - смежное существо с Амальгамной энергией
    Anomaly,          // Аномалия - нефизические существа с Аномальной энергией
    Puppet,       // Марионетка - существо на нитях кукловода
    
    // Расы хаоса (обратные)
    PrisonerHuman,          // Заключенный человек
    SeparatedAmalgam,       // Раздельная амальгама
    FormedAnomaly,          // Сформированная аномалия
    
    // Прочие расы хаоса
    ChaosDistorted,   // Существо хаоса искаженной энергии
    ChaosNoEnergy,          // Существо хаоса лишенное энергии
    
    // Существ высшего порядка
    FirstOrderGodChild,     // Дети Богов Первого порядка (Системные люди)
    SecondOrderGodChild,    // Дети Богов Второго порядка
    Deity                   // Божество
}

/// <summary>
/// Типы душ
/// </summary>
public enum SoulType
{
    Normal,         // Нормальная душа
    Damaged,        // Поврежденная оболочка души
    Broken,         // Полностью поврежденная душа (хаос)
    Divine,         // Божественная душа
    Empty           // Нет души (марионетки)
}

/// <summary>
/// Данные о расе персонажа
/// </summary>
[System.Serializable]
public class RaceData
{
    public RaceType raceType;
    public string raceName;
    public string description;
    public EnergyType primaryEnergy;
    public EnergyType[] additionalEnergies;
    public SoulType soulType;
    public bool hasSpecialAbility;
    public string specialAbilityName;
    public string specialAbilityDescription;
    
    public RaceData(RaceType type)
    {
        raceType = type;
        InitializeRaceData();
    }
    
    private void InitializeRaceData()
    {
        switch (raceType)
        {
            case RaceType.Human:
                raceName = "Человек";
                description = "Физическое существо с Человеческой энергией. Энергия ведет себя как огонь.";
                primaryEnergy = EnergyType.Human;
                additionalEnergies = new EnergyType[0];
                soulType = SoulType.Normal;
                hasSpecialAbility = true;
                specialAbilityName = "Человеческая молния / Человеческий взрыв";
                specialAbilityDescription = "При перепадах температур вылетают искры, преобразующиеся в молнию. Взрыв - теоретическое состояние.";
                break;
                
            case RaceType.Amalgam:
                raceName = "Амальгама";
                description = "Смежное существо с Амальгамной энергией (пластилин). Обычно 2 магические техники, могут создавать гибриды.";
                primaryEnergy = EnergyType.Amalgam;
                additionalEnergies = new EnergyType[0];
                soulType = SoulType.Normal;
                hasSpecialAbility = true;
                specialAbilityName = "Гибридизация техник";
                specialAbilityDescription = "Создание гибридов из двух магических техник. Изменение структуры тела.";
                break;
                
            case RaceType.Anomaly:
                raceName = "Аномалия";
                description = "Нефизическое существо с Аномальной энергией (вода). Техника растворена в энергии.";
                primaryEnergy = EnergyType.Anomaly;
                additionalEnergies = new EnergyType[0];
                soulType = SoulType.Normal;
                hasSpecialAbility = true;
                specialAbilityName = "Аномальная зона";
                specialAbilityDescription = "Владение АЗ: Пустая, Простая, Техническая, Абсолютная.";
                break;
                
            case RaceType.Puppet:
                raceName = "Марионетка";
                description = "Существо на нитях кукловода. Энергия поступает через нити.";
                primaryEnergy = EnergyType.None;
                additionalEnergies = new EnergyType[0];
                soulType = SoulType.Empty;
                hasSpecialAbility = false;
                specialAbilityName = "";
                specialAbilityDescription = "Жизненная энергия через нити. Срыв с нитей = смерть. Урон по душе идет кукловоду.";
                break;
                
            case RaceType.PrisonerHuman:
                raceName = "Заключенный человек";
                description = "Человек, зажавший самооценку и человеческую энергию. Обладает Амальгамной и Аномальной энергиями.";
                primaryEnergy = EnergyType.Anomaly;
                additionalEnergies = new EnergyType[] { EnergyType.Amalgam };
                soulType = SoulType.Damaged;
                hasSpecialAbility = true;
                specialAbilityName = "Магическая перезапись";
                specialAbilityDescription = "Объединение Аномальной и Амальгамной энергий для изменения реальности под правило магии.";
                break;
                
            case RaceType.SeparatedAmalgam:
                raceName = "Раздельная амальгама";
                description = "Нестабильное существо из 5-10 составляющих. Только двое обладают энергиями (Человек + Аномалия).";
                primaryEnergy = EnergyType.Human;
                additionalEnergies = new EnergyType[] { EnergyType.Anomaly };
                soulType = SoulType.Damaged;
                hasSpecialAbility = true;
                specialAbilityName = "Королевская зона";
                specialAbilityDescription = "Человек распространяет волю, Аномалия открывает зону. Подавление всех энергий + Человеческая молния.";
                break;
                
            case RaceType.FormedAnomaly:
                raceName = "Сформированная аномалия";
                description = "Аномалия, не способная отделяться в свою реальность. Доступны Человеческая и Амальгамная энергии.";
                primaryEnergy = EnergyType.Amalgam;
                additionalEnergies = new EnergyType[] { EnergyType.Human };
                soulType = SoulType.Damaged;
                hasSpecialAbility = true;
                specialAbilityName = "Кузница техники";
                specialAbilityDescription = "Создание любого оружия, предмета, артефакта, брони в рамках своей техники.";
                break;
                
            case RaceType.ChaosDistorted:
                raceName = "Существо хаоса искаженной энергии";
                description = "Душа искаверкана. Обладает уникальной энергией (Ци, Проклятая, Котики, Нитки и т.д.).";
                primaryEnergy = EnergyType.Chaos;
                additionalEnergies = new EnergyType[0];
                soulType = SoulType.Broken;
                hasSpecialAbility = true;
                specialAbilityName = "Уникальная энергия";
                specialAbilityDescription = "Индивидуальная энергия со своими правилами.";
                break;
                
            case RaceType.ChaosNoEnergy:
                raceName = "Существо хаоса лишенное энергии";
                description = "Душа состоит лишь из оболочки. После пробуждения - безлимитная техника на основе Пранической энергии.";
                primaryEnergy = EnergyType.None;
                additionalEnergies = new EnergyType[] { EnergyType.Pranic };
                soulType = SoulType.Broken;
                hasSpecialAbility = true;
                specialAbilityName = "Пробуждение";
                specialAbilityDescription = "После пробуждения - безлимитное использование техники (Полный контроль).";
                break;
                
            case RaceType.FirstOrderGodChild:
                raceName = "Дети Богов Первого порядка";
                description = "Системные люди. 6 существ с двумя видами энергий. Содержат деталь от артефакта 'Устройство'.";
                primaryEnergy = EnergyType.Pranic;
                additionalEnergies = new EnergyType[] { EnergyType.Human };
                soulType = SoulType.Divine;
                hasSpecialAbility = true;
                specialAbilityName = "Системный доступ";
                specialAbilityDescription = "Доступ к системным функциям Вселенной.";
                break;
                
            case RaceType.SecondOrderGodChild:
                raceName = "Дети Богов Второго порядка";
                description = "Созданы Богами напрямую. Способны на Созидание и Разрушение в прямом смысле.";
                primaryEnergy = EnergyType.Pranic;
                additionalEnergies = new EnergyType[0];
                soulType = SoulType.Divine;
                hasSpecialAbility = true;
                specialAbilityName = "Созидание / Разрушение";
                specialAbilityDescription = "Прямое создание и уничтожение материи.";
                break;
                
            case RaceType.Deity:
                raceName = "Божество";
                description = "Неубиваемые существа с Пранической энергией.";
                primaryEnergy = EnergyType.Pranic;
                additionalEnergies = new EnergyType[0];
                soulType = SoulType.Divine;
                hasSpecialAbility = true;
                specialAbilityName = "Божественные кнопки";
                specialAbilityDescription = "Доступ к [Консоль] и [Перезапись]. Полный контроль над Вселенной.";
                break;
        }
    }
}

/// <summary>
/// Компонент управления расой персонажа
/// </summary>
public class RaceSystem : MonoBehaviour
{
    [Header("Race Settings")]
    [SerializeField] private RaceData currentRace;
    [SerializeField] private bool isAwakened = false;
    
    [Header("Marionette Settings")]
    [SerializeField] private GameObject puppetMaster;
    [SerializeField] private bool isConnectedToThreads = true;
    
    [Header("Chaos Settings")]
    [SerializeField] private bool isSystemBroken = false;
    [SerializeField] private int componentCount = 0; // Для раздельной амальгамы
    
    [Header("Divine Settings")]
    [SerializeField] private bool hasConsoleAccess = false;
    [SerializeField] private bool hasRewriteAccess = false;
    [SerializeField] private int universeID = 8; // Для Системной пары #8
    
    public event Action<RaceType> OnRaceChanged;
    public event Action OnAwakening;
    public event Action OnPuppetThreadCut;
    
    private EnergySystem energySystem;
    private MagicSystem magicSystem;
    
    public RaceType CurrentRaceType => currentRace.raceType;
    public bool IsAwakened => isAwakened;
    public bool HasSoul => currentRace.soulType != SoulType.Empty;
    
    private void Start()
    {
        energySystem = GetComponent<EnergySystem>();
        magicSystem = GetComponent<MagicSystem>();
        
        if (currentRace == null)
        {
            SetRace(RaceType.Human);
        }
    }
    
    private void Update()
    {
        CheckMarionetteStatus();
    }
    
    /// <summary>
    /// Установить расу персонажа
    /// </summary>
    public void SetRace(RaceType newRace)
    {
        currentRace = new RaceData(newRace);
        ApplyRaceEffects();
        OnRaceChanged?.Invoke(newRace);
        
        Debug.Log($"Раса установлена: {currentRace.raceName}");
    }
    
    /// <summary>
    /// Применить эффекты расы
    /// </summary>
    private void ApplyRaceEffects()
    {
        // Настройка энергии
        if (energySystem != null)
        {
            energySystem.SetEnergyType(currentRace.primaryEnergy);
        }
        
        // Особые настройки для разных рас
        switch (currentRace.raceType)
        {
            case RaceType.Amalgam:
                if (magicSystem != null)
                {
                    // Амальгамы имеют 2 техники
                    magicSystem.LearnTechnique(MagicType.Fire);
                    magicSystem.LearnTechnique(MagicType.Water);
                }
                break;
                
            case RaceType.Anomaly:
                if (magicSystem != null)
                {
                    // Аномалии начинают с Пустой АЗ
                    // magicSystem.SetZoneMastery(AnomalyZoneType.Empty);
                }
                break;
                
            case RaceType.Puppet:
                isConnectedToThreads = true;
                break;
                
            case RaceType.Deity:
                hasConsoleAccess = true;
                hasRewriteAccess = true;
                break;
        }
    }
    
    /// <summary>
    /// Пробуждение существа
    /// </summary>
    public void Awaken()
    {
        if (isAwakened)
        {
            Debug.Log("Существо уже пробуждено!");
            return;
        }
        
        isAwakened = true;
        
        // Эффекты пробуждения для разных рас
        switch (currentRace.raceType)
        {
            case RaceType.ChaosNoEnergy:
                // Получение безлимитной Пранической энергии
                if (energySystem != null)
                {
                    energySystem.SetEnergyType(EnergyType.Pranic);
                }
                Debug.Log("Пробуждение! Получен безлимитный доступ к технике!");
                break;
                
            case RaceType.SeparatedAmalgam:
                // Разделение на компоненты
                SeparateComponents();
                break;
        }
        
        OnAwakening?.Invoke();
        Debug.Log($"{currentRace.raceName} пробудился!");
    }
    
    /// <summary>
    /// Разделение Раздельной амальгамы на компоненты
    /// </summary>
    private void SeparateComponents()
    {
        if (currentRace.raceType != RaceType.SeparatedAmalgam) return;
        
        Debug.Log("Раздельная амальгама разделяется на компоненты!");
        Debug.Log($"Всего компонентов: {componentCount}");
        Debug.Log("Только Человек и Аномалия выживают после разделения!");
        
        // Остальные существа без энергии умирают
    }
    
    /// <summary>
    /// Проверка состояния марионетки
    /// </summary>
    private void CheckMarionetteStatus()
    {
        if (currentRace.raceType != RaceType.Puppet) return;
        
        if (!isConnectedToThreads)
        {
            Die();
            OnPuppetThreadCut?.Invoke();
        }
    }
    
    /// <summary>
    /// Отрезать нити марионетки
    /// </summary>
    public void CutPuppetThreads()
    {
        if (currentRace.raceType != RaceType.Puppet) return;
        
        isConnectedToThreads = false;
        Debug.LogWarning("Нити марионетки перерезаны!");
    }
    
    /// <summary>
    /// Нанести урон по душе (для марионеток - урон кукловоду)
    /// </summary>
    public void DealSoulDamage(int damage)
    {
        if (currentRace.raceType == RaceType.Puppet && puppetMaster != null)
        {
            Debug.Log($"Урон по душе марионетки нанесен кукловоду: {damage}");
            // Нанести урон кукловоду
            var masterStats = puppetMaster.GetComponent<PlayerStats>();
            if (masterStats != null)
            {
                masterStats.ChangeHealth(-damage);
            }
        }
        else if (HasSoul)
        {
            Debug.Log($"Урон по душе: {damage}");
            // Обычный урон по душе
        }
        else
        {
            Debug.Log("У существа нет души - урон по душе невозможен!");
        }
    }
    
    /// <summary>
    /// Использовать особый прием расы
    /// </summary>
    public bool UseSpecialAbility(params object[] parameters)
    {
        if (!currentRace.hasSpecialAbility)
        {
            Debug.LogWarning("У этой расы нет особого приема!");
            return false;
        }
        
        Debug.Log($"Использован особый прием: {currentRace.specialAbilityName}");
        
        switch (currentRace.raceType)
        {
            case RaceType.Human:
                return UseHumanSpecialAbility(parameters);
                
            case RaceType.PrisonerHuman:
                return UsePrisonerHumanAbility(parameters);
                
            case RaceType.SeparatedAmalgam:
                return UseSeparatedAmalgamAbility(parameters);
                
            case RaceType.FormedAnomaly:
                return UseFormedAnomalyAbility(parameters);
                
            case RaceType.Deity:
                return UseDeityAbility(parameters);
        }
        
        return true;
    }
    
    private bool UseHumanSpecialAbility(object[] parameters)
    {
        // Человеческая молния или взрыв
        if (energySystem != null)
        {
            return energySystem.TryUseHumanLightning(20f);
        }
        return false;
    }
    
    private bool UsePrisonerHumanAbility(object[] parameters)
    {
        // Магическая перезапись
        if (magicSystem != null && parameters.Length > 0)
        {
            return magicSystem.MagicalRewrite(parameters[0]);
        }
        return false;
    }
    
    private bool UseSeparatedAmalgamAbility(object[] parameters)
    {
        // Королевская зона
        if (magicSystem != null && parameters.Length >= 2)
        {
            Vector3 area = parameters[0] is Vector3 v ? v : Vector3.zero;
            float duration = parameters[1] is float f ? f : 5f;
            return magicSystem.RoyalZone(area, duration);
        }
        return false;
    }
    
    private bool UseFormedAnomalyAbility(object[] parameters)
    {
        // Кузница техники
        if (magicSystem != null && parameters.Length >= 1)
        {
            string itemName = parameters[0]?.ToString() ?? "Item";
            GameObject item = magicSystem.ForgeTechniqueItem(itemName);
            return item != null;
        }
        return false;
    }
    
    private bool UseDeityAbility(object[] parameters)
    {
        // Божественные кнопки
        if (parameters.Length > 0 && parameters[0] is string command)
        {
            if (command == "Console" && hasConsoleAccess)
            {
                Debug.Log("[Консоль] активирована - отображение данных о мире");
                return true;
            }
            else if (command == "Rewrite" && hasRewriteAccess)
            {
                Debug.Log($"[Перезапись] активирована - редактирование Вселенной #{universeID}");
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Смерть существа
    /// </summary>
    public void Die()
    {
        Debug.Log($"{currentRace.raceName} погиб!");
        
        // Логика смерти в зависимости от расы
        switch (currentRace.raceType)
        {
            case RaceType.Puppet:
                Debug.Log("Марионетка разрушена без возможности восстановления.");
                break;
                
            case RaceType.SeparatedAmalgam:
                if (isAwakened)
                {
                    Debug.Log("После пробуждения остались только Человек и Аномалия.");
                }
                break;
        }
        
        // Вызов смерти в PlayerStats
        var stats = GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.ChangeHealth(-stats.GetHealthAmount());
        }
    }
    
    /// <summary>
    /// Получить информацию о расе
    /// </summary>
    public string GetRaceInfo()
    {
        string info = $"Раса: {currentRace.raceName}\n";
        info += $"Описание: {currentRace.description}\n";
        info += $"Энергия: {currentRace.primaryEnergy}\n";
        info += $"Душа: {currentRace.soulType}\n";
        info += $"Особый прием: {currentRace.specialAbilityName}\n";
        info += $"Пробужден: {isAwakened}\n";
        
        if (currentRace.additionalEnergies.Length > 0)
        {
            info += "Доп. энергии: ";
            foreach (var energy in currentRace.additionalEnergies)
            {
                info += $"{energy} ";
            }
            info += "\n";
        }
        
        return info;
    }
}
