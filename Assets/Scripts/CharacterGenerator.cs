using UnityEngine;
using System.Collections.Generic;

public class CharacterGenerator : MonoBehaviour
{
    [System.Serializable]
    public class MagicData
    {
        public string name;
        public MagicTheme theme;
        public string description;
    }

    private static readonly List<MagicData> allMagics = new List<MagicData>
    {
        // Стихия
        new MagicData { name = "Огонь", theme = MagicTheme.Element, description = "Управление огнем" },
        new MagicData { name = "Вода", theme = MagicTheme.Element, description = "Управление водой" },
        new MagicData { name = "Лед", theme = MagicTheme.Element, description = "Управление льдом" },
        new MagicData { name = "Молния", theme = MagicTheme.Element, description = "Управление молнией" },
        new MagicData { name = "Земля", theme = MagicTheme.Element, description = "Управление землей" },
        new MagicData { name = "Ветер", theme = MagicTheme.Element, description = "Управление ветром" },
        new MagicData { name = "Тьма", theme = MagicTheme.Element, description = "Управление тьмой" },
        new MagicData { name = "Свет", theme = MagicTheme.Element, description = "Управление светом" },
        new MagicData { name = "Кислота", theme = MagicTheme.Element, description = "Управление кислотой" },
        new MagicData { name = "Природа", theme = MagicTheme.Element, description = "Управление растениями" },
        
        // Техническая
        new MagicData { name = "Хакер Реальности", theme = MagicTheme.Technical, description = "Взлом реальности" },
        new MagicData { name = "Хирургия", theme = MagicTheme.Technical, description = "Биологические манипуляции" },
        new MagicData { name = "Робототехника", theme = MagicTheme.Technical, description = "Управление механизмами" },
        new MagicData { name = "Лазер", theme = MagicTheme.Technical, description = "Энергетические лучи" },
        new MagicData { name = "Кибернетика", theme = MagicTheme.Technical, description = "Технологические улучшения" },
        
        // Пространство
        new MagicData { name = "Гравитация", theme = MagicTheme.Space, description = "Контроль гравитации" },
        new MagicData { name = "Телекинез", theme = MagicTheme.Space, description = "Перемещение объектов" },
        new MagicData { name = "Телепортация", theme = MagicTheme.Space, description = "Мгновенное перемещение" },
        new MagicData { name = "Порталы", theme = MagicTheme.Space, description = "Создание проходов" },
        
        // Время
        new MagicData { name = "Ускорение", theme = MagicTheme.Time, description = "Ускорение времени" },
        new MagicData { name = "Замедление", theme = MagicTheme.Time, description = "Замедление времени" },
        new MagicData { name = "Пауза", theme = MagicTheme.Time, description = "Остановка времени" },
        new MagicData { name = "Возврат", theme = MagicTheme.Time, description = "Возврат во времени" },
        
        // Особая
        new MagicData { name = "Исцеление", theme = MagicTheme.Special, description = "Восстановление здоровья" },
        new MagicData { name = "Вампиризм", theme = MagicTheme.Special, description = "Кража жизненной силы" },
        new MagicData { name = "Взрыв", theme = MagicTheme.Special, description = "Создание взрывов" },
        new MagicData { name = "Разрез", theme = MagicTheme.Special, description = "Создание разрезов" },
        new MagicData { name = "Барьер", theme = MagicTheme.Special, description = "Создание защитных полей" },
        
        // Божественная
        new MagicData { name = "Созидание", theme = MagicTheme.Divine, description = "Создание материи" },
        new MagicData { name = "Разрушение", theme = MagicTheme.Divine, description = "Разрушение материи" },
        new MagicData { name = "Контроль", theme = MagicTheme.Divine, description = "Контроль разума" },
        new MagicData { name = "Отрицание", theme = MagicTheme.Divine, description = "Отмена магии" }
    };

    private static readonly List<EnergyType> distortedEnergies = new List<EnergyType>
    {
        EnergyType.Chaos
    };

    public void Start()
    {
        GenerateCharacter();
    }

    public void GenerateCharacter()
    {
        RaceType race = RollRace();
        EnergyType primaryEnergy = GetPrimaryEnergy(race);
        EnergyType secondaryEnergy = GetSecondaryEnergy(race);
        EnergyType tertiaryEnergy = GetTertiaryEnergy(race);
        MagicData magic = RollMagic(race, primaryEnergy);
        bool hasSpecialAbility = CanUseSpecialAbility(race, primaryEnergy);

        Debug.Log($"Сгенерирован персонаж:");
        Debug.Log($"Раса: {race}");
        Debug.Log($"Основная энергия: {primaryEnergy}");
        if (secondaryEnergy != EnergyType.None)
            Debug.Log($"Вторичная энергия: {secondaryEnergy}");
        if (tertiaryEnergy != EnergyType.None)
            Debug.Log($"Третичная энергия: {tertiaryEnergy}");
        Debug.Log($"Магия: {magic.name} ({magic.theme})");
        Debug.Log($"Особый прием доступен: {hasSpecialAbility}");

        PlayerController player = GetComponent<PlayerController>();
        if (player != null)
        {
            player.Initialize(race, primaryEnergy, secondaryEnergy, tertiaryEnergy, magic.name, hasSpecialAbility);
        }
    }

    private RaceType RollRace()
    {
        float roll = Random.value * 100f;

        if (roll < 0.1f)
            return RollGodRace();
        else if (roll < 10f)
            return RollRareRace();
        else if (roll < 30f)
            return RollUncommonRace();
        else
            return RollCommonRace();
    }

    private RaceType RollGodRace()
    {
        return (RaceType)(int)RaceType.Deity;
    }

    private RaceType RollRareRace()
    {
        return (RaceType)Random.Range((int)RaceType.PrisonerHuman, (int)RaceType.FormedAnomaly + 1);
    }

    private RaceType RollUncommonRace()
    {
        return (RaceType)Random.Range((int)RaceType.Amalgam, (int)RaceType.Puppet + 1);
    }

    private RaceType RollCommonRace()
    {
        return RaceType.Human;
    }

    private EnergyType GetPrimaryEnergy(RaceType race)
    {
        switch (race)
        {
            case RaceType.Human:
                return EnergyType.Human;
            case RaceType.Amalgam:
                return EnergyType.Amalgam;
            case RaceType.Anomaly:
                return EnergyType.Anomaly;
            case RaceType.Puppet:
                return RandomEnumValue<EnergyType>();
            case RaceType.PrisonerHuman:
                return EnergyType.Amalgam;
            case RaceType.SeparatedAmalgam:
                return EnergyType.Human;
            case RaceType.FormedAnomaly:
                return EnergyType.Human;
            case RaceType.ChaosDistorted:
                return EnergyType.Chaos;
            case RaceType.ChaosNoEnergy:
                return EnergyType.None;
            case RaceType.FirstOrderGodChild:
            case RaceType.SecondOrderGodChild:
                return EnergyType.Human;
            case RaceType.Deity:
                return EnergyType.Pranic;
            default:
                return EnergyType.Human;
        }
    }

    private EnergyType GetSecondaryEnergy(RaceType race)
    {
        switch (race)
        {
            case RaceType.PrisonerHuman:
                return EnergyType.Anomaly;
            case RaceType.SeparatedAmalgam:
                return EnergyType.Anomaly;
            case RaceType.FormedAnomaly:
                return Random.value > 0.5f ? EnergyType.Amalgam : EnergyType.Anomaly;
            default:
                return EnergyType.None;
        }
    }

    private EnergyType GetTertiaryEnergy(RaceType race)
    {
        if (race == RaceType.FormedAnomaly)
        {
            return Random.value > 0.5f ? EnergyType.Amalgam : EnergyType.Anomaly;
        }
        return EnergyType.None;
    }

    private MagicData RollMagic(RaceType race, EnergyType primaryEnergy)
    {
        if (primaryEnergy == EnergyType.None && race != RaceType.ChaosNoEnergy)
        {
            return allMagics[Random.Range(0, allMagics.Count)];
        }

        if (race == RaceType.ChaosDistorted)
        {
            return allMagics[Random.Range(0, allMagics.Count)];
        }

        List<MagicData> validMagics = new List<MagicData>();
        foreach (var magic in allMagics)
        {
            if (IsMagicCompatible(magic.theme, primaryEnergy))
            {
                validMagics.Add(magic);
            }
        }

        if (validMagics.Count == 0)
            return allMagics[Random.Range(0, allMagics.Count)];

        return validMagics[Random.Range(0, validMagics.Count)];
    }

    private bool IsMagicCompatible(MagicTheme theme, EnergyType energy)
    {
        return true;
    }

    private bool CanUseSpecialAbility(RaceType race, EnergyType primaryEnergy)
    {
        if (primaryEnergy == EnergyType.None)
            return false;

        switch (race)
        {
            case RaceType.PrisonerHuman:
            case RaceType.SeparatedAmalgam:
            case RaceType.FormedAnomaly:
                return true;
            default:
                return primaryEnergy != EnergyType.None;
        }
    }

    private T RandomEnumValue<T>() where T : System.Enum
    {
        var values = System.Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Range(0, values.Length));
    }

    public void RerollDistortedEnergy()
    {
        PlayerController player = GetComponent<PlayerController>();
        if (player != null && player.race == RaceType.ChaosDistorted)
        {
            player.primaryEnergy = distortedEnergies[Random.Range(0, distortedEnergies.Count)];
            Debug.Log($"Энергия хаоса изменена на: {player.primaryEnergy}");
        }
    }
}
