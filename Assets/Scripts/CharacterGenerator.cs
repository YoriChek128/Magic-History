using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Система генерации и кастомизации персонажей
/// Позволяет создавать любых персонажей со случайной магией из огромного ассортимента
/// </summary>
public class CharacterGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private bool useRandomSeed = true;
    [SerializeField] private int randomSeed = -1;
    
    [Header("Race Probabilities")]
    [SerializeField] private float commonRaceChance = 70f; // Человек, Амальгама, Аномалия
    [SerializeField] private float rareRaceChance = 20f;   // Марионетка, Хаос расы
    [SerializeField] private float legendaryRaceChance = 9.9f; // Дети Богов
    [SerializeField] private float mythicRaceChance = 0.1f;    // Божество
    
    [Header("Magic Distribution")]
    [SerializeField] private int minTechniquesCount = 1;
    [SerializeField] private int maxTechniquesCount = 3;
    [SerializeField] private bool allowDivineMagic = false; // Только для высших рас
    
    [Header("References")]
    private EnergySystem energySystem;
    private MagicSystem magicSystem;
    private RaceSystem raceSystem;
    private SoulUI soulUI;
    
    // Все доступные техники по темам
    private static readonly Dictionary<MagicTheme, List<MagicType>> techniquesByTheme = 
        new Dictionary<MagicTheme, List<MagicType>>
    {
        { MagicTheme.Element, new List<MagicType> 
            { MagicType.Fire, MagicType.Water, MagicType.Ice, MagicType.Cosmos, 
              MagicType.Acid, MagicType.Nature, MagicType.Sand, MagicType.Air, 
              MagicType.Lightning, MagicType.Darkness } },
        
        { MagicTheme.Technical, new List<MagicType> 
            { MagicType.RealityHacker, MagicType.Surgery, 
              MagicType.RobotControl, MagicType.Laser } },
        
        { MagicTheme.Space, new List<MagicType> 
            { MagicType.Gravity, MagicType.Telekinesis, MagicType.Teleport } },
        
        { MagicTheme.Time, new List<MagicType> 
            { MagicType.TimeReturn, MagicType.Chronos, 
              MagicType.Skip, MagicType.Acceleration } },
        
        { MagicTheme.Special, new List<MagicType> 
            { MagicType.Healing, MagicType.Vampirism, 
              MagicType.EnergyMelting, MagicType.Explosions, MagicType.Slashes } },
        
        { MagicTheme.Divine, new List<MagicType> 
            { MagicType.Creation, MagicType.Destruction, 
              MagicType.Control, MagicType.Mind, MagicType.Negation } }
    };
    
    private void Start()
    {
        energySystem = GetComponent<EnergySystem>();
        magicSystem = GetComponent<MagicSystem>();
        raceSystem = GetComponent<RaceSystem>();
        soulUI = GetComponent<SoulUI>();
        
        if (useRandomSeed && randomSeed == -1)
        {
            randomSeed = UnityEngine.Random.Range(0, int.MaxValue);
        }
        UnityEngine.Random.InitState(randomSeed);
    }
    
    /// <summary>
    /// Создать случайного персонажа
    /// </summary>
    public void GenerateRandomCharacter()
    {
        Debug.Log($"=== Генерация персонажа (Seed: {randomSeed}) ===");
        
        // 1. Выбираем расу
        RaceType selectedRace = SelectRandomRace();
        raceSystem.SetRace(selectedRace);
        Debug.Log($"Выбрана раса: {selectedRace}");
        
        // 2. Настраиваем энергию в зависимости от расы
        ConfigureEnergyForRace(selectedRace);
        
        // 3. Выбираем случайные техники
        int techniquesCount = UnityEngine.Random.Range(minTechniquesCount, maxTechniquesCount + 1);
        List<MagicType> selectedTechniques = SelectRandomTechniques(techniquesCount, selectedRace);
        
        foreach (var technique in selectedTechniques)
        {
            magicSystem.LearnTechnique(technique);
            Debug.Log($"Изучена техника: {technique}");
        }
        
        // 4. Проверяем возможность пробуждения
        CheckAwakeningPossibility(selectedRace);
        
        // 5. Обновляем UI
        UpdateSoulUI(selectedRace);
        
        Debug.Log("=== Генерация завершена ===");
    }
    
    /// <summary>
    /// Выбрать случайную расу на основе вероятностей
    /// </summary>
    private RaceType SelectRandomRace()
    {
        float roll = UnityEngine.Random.Range(0f, 100f);
        
        if (roll < commonRaceChance)
        {
            // Обычные расы: Человек, Амальгама, Аномалия
            return SelectFromCommonRaces();
        }
        else if (roll < commonRaceChance + rareRaceChance)
        {
            // Редкие расы: Марионетка, Расы хаоса
            return SelectFromRareRaces();
        }
        else if (roll < commonRaceChance + rareRaceChance + legendaryRaceChance)
        {
            // Легендарные: Дети Богов
            return SelectFromLegendaryRaces();
        }
        else
        {
            // Мифические: Божество
            return RaceType.Deity;
        }
    }
    
    private RaceType SelectFromCommonRaces()
    {
        float roll = UnityEngine.Random.Range(0f, 100f);
        if (roll < 40f) return RaceType.Human;
        if (roll < 70f) return RaceType.Amalgam;
        return RaceType.Anomaly;
    }
    
    private RaceType SelectFromRareRaces()
    {
        float roll = UnityEngine.Random.Range(0f, 100f);
        if (roll < 20f) return RaceType.Marionette;
        if (roll < 40f) return RaceType.PrisonerHuman;
        if (roll < 60f) return RaceType.SeparatedAmalgam;
        if (roll < 80f) return RaceType.FormedAnomaly;
        if (roll < 90f) return RaceType.ChaosDistortedEnergy;
        return RaceType.ChaosNoEnergy;
    }
    
    private RaceType SelectFromLegendaryRaces()
    {
        float roll = UnityEngine.Random.Range(0f, 100f);
        if (roll < 50f) return RaceType.FirstOrderGodChild;
        return RaceType.SecondOrderGodChild;
    }
    
    /// <summary>
    /// Настроить энергию для выбранной расы
    /// </summary>
    private void ConfigureEnergyForRace(RaceType race)
    {
        switch (race)
        {
            case RaceType.Human:
                energySystem.SetEnergyType(EnergyType.Human);
                break;
                
            case RaceType.Amalgam:
                energySystem.SetEnergyType(EnergyType.Amalgam);
                // Амальгамы получают 2 техники автоматически
                break;
                
            case RaceType.Anomaly:
                energySystem.SetEnergyType(EnergyType.Anomaly);
                break;
                
            case RaceType.Marionette:
                energySystem.SetEnergyType(EnergyType.None);
                break;
                
            case RaceType.PrisonerHuman:
            case RaceType.SeparatedAmalgam:
            case RaceType.FormedAnomaly:
            case RaceType.ChaosDistortedEnergy:
                energySystem.SetEnergyType(EnergyType.Chaos);
                break;
                
            case RaceType.ChaosNoEnergy:
                energySystem.SetEnergyType(EnergyType.None);
                break;
                
            case RaceType.FirstOrderGodChild:
            case RaceType.SecondOrderGodChild:
            case RaceType.Deity:
                energySystem.SetEnergyType(EnergyType.Pranic);
                allowDivineMagic = true;
                break;
        }
    }
    
    /// <summary>
    /// Выбрать случайные техники с учетом расы
    /// </summary>
    private List<MagicType> SelectRandomTechniques(int count, RaceType race)
    {
        List<MagicType> availableTechniques = new List<MagicType>();
        
        // Определяем доступные темы в зависимости от расы
        List<MagicTheme> availableThemes = GetAvailableThemesForRace(race);
        
        foreach (var theme in availableThemes)
        {
            if (techniquesByTheme.ContainsKey(theme))
            {
                availableTechniques.AddRange(techniquesByTheme[theme]);
            }
        }
        
        // Если раса позволяет божественную магию
        if (allowDivineMagic || race == RaceType.Deity)
        {
            availableTechniques.AddRange(techniquesByTheme[MagicTheme.Divine]);
        }
        
        // Случайный выбор техник
        List<MagicType> selected = new List<MagicType>();
        for (int i = 0; i < count && availableTechniques.Count > 0; i++)
        {
            int index = UnityEngine.Random.Range(0, availableTechniques.Count);
            MagicType selectedTechnique = availableTechniques[index];
            
            if (!selected.Contains(selectedTechnique))
            {
                selected.Add(selectedTechnique);
                availableTechniques.RemoveAt(index);
            }
        }
        
        // Особые правила для рас
        if (race == RaceType.Amalgam && selected.Count >= 2)
        {
            // Амальгамы могут создавать гибриды из первых двух техник
            Debug.Log($"Амальгама может создать гибрид: {selected[0]} + {selected[1]}");
        }
        
        return selected;
    }
    
    /// <summary>
    /// Получить доступные темы магии для расы
    /// </summary>
    private List<MagicTheme> GetAvailableThemesForRace(RaceType race)
    {
        List<MagicTheme> themes = new List<MagicTheme>();
        
        switch (race)
        {
            case RaceType.Human:
                // Люди могут использовать любые техники, кроме божественных
                themes.Add(MagicTheme.Element);
                themes.Add(MagicTheme.Technical);
                themes.Add(MagicTheme.Space);
                themes.Add(MagicTheme.Time);
                themes.Add(MagicTheme.Special);
                break;
                
            case RaceType.Amalgam:
                // Амальгамы предпочитают стихийные и технические
                themes.Add(MagicTheme.Element);
                themes.Add(MagicTheme.Technical);
                themes.Add(MagicTheme.Special);
                break;
                
            case RaceType.Anomaly:
                // Аномалии сильны в пространстве и времени
                themes.Add(MagicTheme.Space);
                themes.Add(MagicTheme.Time);
                themes.Add(MagicTheme.Special);
                break;
                
            case RaceType.Marionette:
                // Марионетки не имеют своей магии
                break;
                
            case RaceType.PrisonerHuman:
                // Заключенные люди имеют доступ ко всему
                themes.Add(MagicTheme.Element);
                themes.Add(MagicTheme.Technical);
                themes.Add(MagicTheme.Space);
                themes.Add(MagicTheme.Time);
                themes.Add(MagicTheme.Special);
                break;
                
            case RaceType.SeparatedAmalgam:
                // Раздельные амальгамы: человек + аномалия
                themes.Add(MagicTheme.Element);
                themes.Add(MagicTheme.Space);
                themes.Add(MagicTheme.Time);
                break;
                
            case RaceType.FormedAnomaly:
                // Сформированные аномалии могут создавать предметы
                themes.Add(MagicTheme.Element);
                themes.Add(MagicTheme.Technical);
                themes.Add(MagicTheme.Special);
                break;
                
            case RaceType.ChaosDistortedEnergy:
            case RaceType.ChaosNoEnergy:
                // Хаос расы имеют уникальные техники
                themes.Add(MagicTheme.Special);
                break;
                
            case RaceType.FirstOrderGodChild:
            case RaceType.SecondOrderGodChild:
            case RaceType.Deity:
                // Высшие расы имеют доступ ко всем темам
                themes.Add(MagicTheme.Element);
                themes.Add(MagicTheme.Technical);
                themes.Add(MagicTheme.Space);
                themes.Add(MagicTheme.Time);
                themes.Add(MagicTheme.Special);
                themes.Add(MagicTheme.Divine);
                break;
        }
        
        return themes;
    }
    
    /// <summary>
    /// Проверить возможность пробуждения
    /// </summary>
    private void CheckAwakeningPossibility(RaceType race)
    {
        // Некоторые расы могут быть пробуждены
        if (race == RaceType.ChaosNoEnergy || 
            race == RaceType.SeparatedAmalgam ||
            race == RaceType.Human ||
            race == RaceType.Amalgam ||
            race == RaceType.Anomaly)
        {
            Debug.Log($"Раса {race} способна к пробуждению!");
        }
    }
    
    /// <summary>
    /// Обновить Soul UI в зависимости от расы
    /// </summary>
    private void UpdateSoulUI(RaceType race)
    {
        // Проверка на наличие магии
        if (magicSystem != null && magicSystem.CanUseMagic())
        {
            soulUI.CheckMagicAvailability();
        }
        
        // Если это Божество, добавить божественные кнопки
        if (race == RaceType.Deity)
        {
            soulUI.AscendToDeity(8); // Вселенная #8
        }
    }
    
    /// <summary>
    /// Создать технику на основе типа энергии
    /// </summary>
    public MagicTechnique CreateSpellFromEnergy(EnergyType energyType, MagicTheme theme)
    {
        string spellName = "";
        int manaCost = 10;
        
        switch (energyType)
        {
            case EnergyType.Human:
                // Человеческая энергия создает техники "огнем"
                spellName = $"Огненная {theme} техника";
                manaCost = 15;
                break;
                
            case EnergyType.Amalgam:
                // Амальгамная энергия позволяет создавать гибриды
                spellName = $"Гибридная {theme} техника";
                manaCost = 20;
                break;
                
            case EnergyType.Anomaly:
                // Аномальная энергия "растворяет" технику
                spellName = $"Растворенная {theme} техника";
                manaCost = 25;
                break;
                
            case EnergyType.Pranic:
                // Праническая энергия - божественное творение
                spellName = $"Божественная {theme} техника";
                manaCost = 50;
                break;
                
            case EnergyType.Chaos:
                // Хаотическая энергия - непредсказуемый эффект
                spellName = $"Хаотическая {theme} техника";
                manaCost = 30;
                break;
        }
        
        return new MagicTechnique(spellName, MagicType.Special, theme, manaCost);
    }
    
    /// <summary>
    /// Получить статистику сгенерированного персонажа
    /// </summary>
    public string GetCharacterStats()
    {
        string stats = "=== СТАТИСТИКА ПЕРСОНАЖА ===\n\n";
        
        if (raceSystem != null)
        {
            stats += raceSystem.GetRaceInfo() + "\n";
        }
        
        if (energySystem != null)
        {
            stats += energySystem.GetEnergyInfo() + "\n";
        }
        
        if (magicSystem != null)
        {
            stats += magicSystem.GetMagicInfo() + "\n";
        }
        
        if (soulUI != null)
        {
            stats += soulUI.GetAvailableButtonsInfo() + "\n";
        }
        
        return stats;
    }
    
    /// <summary>
    /// Сбросить генератор и создать нового персонажа
    /// </summary>
    public void RerollCharacter()
    {
        randomSeed = UnityEngine.Random.Range(0, int.MaxValue);
        UnityEngine.Random.InitState(randomSeed);
        GenerateRandomCharacter();
    }
}
