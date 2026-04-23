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
        // === СТИХИЯ (23 магии) ===
        new MagicData { name = "Огонь", theme = MagicTheme.Element, description = "Управление огнем" },
        new MagicData { name = "Вода", theme = MagicTheme.Element, description = "Управление водой" },
        new MagicData { name = "Лёд", theme = MagicTheme.Element, description = "Управление льдом" },
        new MagicData { name = "Земля", theme = MagicTheme.Element, description = "Управление землей" },
        new MagicData { name = "Воздух", theme = MagicTheme.Element, description = "Управление воздухом" },
        new MagicData { name = "Молния", theme = MagicTheme.Element, description = "Управление молнией" },
        new MagicData { name = "Тьма", theme = MagicTheme.Element, description = "Управление тьмой" },
        new MagicData { name = "Свет", theme = MagicTheme.Element, description = "Управление светом" },
        new MagicData { name = "Кровь", theme = MagicTheme.Element, description = "Управление кровью" },
        new MagicData { name = "Космос", theme = MagicTheme.Element, description = "Управление космосом" },
        new MagicData { name = "Песок", theme = MagicTheme.Element, description = "Управление песком" },
        new MagicData { name = "Магма", theme = MagicTheme.Element, description = "Управление магмой" },
        new MagicData { name = "Чернила", theme = MagicTheme.Element, description = "Управление чернилами" },
        new MagicData { name = "Кислота", theme = MagicTheme.Element, description = "Управление кислотой" },
        new MagicData { name = "Железо", theme = MagicTheme.Element, description = "Управление железом" },
        new MagicData { name = "Тень", theme = MagicTheme.Element, description = "Управление тенью" },
        new MagicData { name = "Нити", theme = MagicTheme.Element, description = "Управление нитями" },
        new MagicData { name = "Природа", theme = MagicTheme.Element, description = "Управление природой" },
        new MagicData { name = "Яд", theme = MagicTheme.Element, description = "Управление ядом" },
        new MagicData { name = "Газ", theme = MagicTheme.Element, description = "Управление газом" },
        new MagicData { name = "Пыль", theme = MagicTheme.Element, description = "Управление пылью" },
        new MagicData { name = "Стекло", theme = MagicTheme.Element, description = "Управление стеклом" },
        
        // === ТЕХНИЧЕСКАЯ (13 магий) ===
        new MagicData { name = "Контроль роботов", theme = MagicTheme.Technical, description = "Управление роботами" },
        new MagicData { name = "Хирургия", theme = MagicTheme.Technical, description = "Биологические манипуляции" },
        new MagicData { name = "Физическая сила", theme = MagicTheme.Technical, description = "Усиление физической силы" },
        new MagicData { name = "Помехи и сбои", theme = MagicTheme.Technical, description = "Создание помех" },
        new MagicData { name = "Взлом реальности", theme = MagicTheme.Technical, description = "Взлом реальности" },
        new MagicData { name = "Магнетизм", theme = MagicTheme.Technical, description = "Управление магнетизмом" },
        new MagicData { name = "Лазер", theme = MagicTheme.Technical, description = "Энергетические лучи" },
        new MagicData { name = "Энерго пластины", theme = MagicTheme.Technical, description = "Создание энергопластин" },
        new MagicData { name = "Радиация", theme = MagicTheme.Technical, description = "Управление радиацией" },
        new MagicData { name = "Аналитизм", theme = MagicTheme.Technical, description = "Анализ и вычисления" },
        new MagicData { name = "Коллизия", theme = MagicTheme.Technical, description = "Управление столкновениями" },
        new MagicData { name = "Интерфейс", theme = MagicTheme.Technical, description = "Создание интерфейсов" },
        new MagicData { name = "Манипуляция телом", theme = MagicTheme.Technical, description = "Изменение тела" },
        
        // === ПРОСТРАНСТВО (9 магий) ===
        new MagicData { name = "Телекинез", theme = MagicTheme.Space, description = "Перемещение объектов" },
        new MagicData { name = "Барьеры", theme = MagicTheme.Space, description = "Создание барьеров" },
        new MagicData { name = "Порталы", theme = MagicTheme.Space, description = "Создание проходов" },
        new MagicData { name = "Клинок пространства", theme = MagicTheme.Space, description = "Разрезы пространства" },
        new MagicData { name = "Гравитация", theme = MagicTheme.Space, description = "Контроль гравитации" },
        new MagicData { name = "Движение", theme = MagicTheme.Space, description = "Управление движением" },
        new MagicData { name = "Иллюзии", theme = MagicTheme.Space, description = "Создание иллюзий" },
        new MagicData { name = "Зеркальная реальность", theme = MagicTheme.Space, description = "Отражение реальности" },
        new MagicData { name = "Пространство", theme = MagicTheme.Space, description = "Искажение пространства" },
        
        // === ВРЕМЯ (9 магий) ===
        new MagicData { name = "Предсказание", theme = MagicTheme.Time, description = "Видение будущего" },
        new MagicData { name = "Хронос", theme = MagicTheme.Time, description = "Контроль времени" },
        new MagicData { name = "Остановка времени", theme = MagicTheme.Time, description = "Пауза времени" },
        new MagicData { name = "Откат времени", theme = MagicTheme.Time, description = "Возврат во времени" },
        new MagicData { name = "Пропуск времени", theme = MagicTheme.Time, description = "Пропуск моментов" },
        new MagicData { name = "Цикл времени", theme = MagicTheme.Time, description = "Временные петли" },
        new MagicData { name = "Ускорение времени", theme = MagicTheme.Time, description = "Ускорение времени" },
        new MagicData { name = "Зеркала времен", theme = MagicTheme.Time, description = "Отражение временных линий" },
        new MagicData { name = "Время", theme = MagicTheme.Time, description = "Абсолютное время" },
        
        // === ОСОБАЯ (16 магий) ===
        new MagicData { name = "Лезвие жизни", theme = MagicTheme.Special, description = "Управление жизненной энергией" },
        new MagicData { name = "Порча", theme = MagicTheme.Special, description = "Накладывание порчи" },
        new MagicData { name = "Взрыв", theme = MagicTheme.Special, description = "Создание взрывов" },
        new MagicData { name = "Рассечение", theme = MagicTheme.Special, description = "Создание разрезов" },
        new MagicData { name = "Импульс", theme = MagicTheme.Special, description = "Энергетические импульсы" },
        new MagicData { name = "Болезнь", theme = MagicTheme.Special, description = "Распространение болезней" },
        new MagicData { name = "Созидание кукол", theme = MagicTheme.Special, description = "Создание марионеток" },
        new MagicData { name = "Управление призраками", theme = MagicTheme.Special, description = "Контроль духов" },
        new MagicData { name = "Плавление энергий", theme = MagicTheme.Special, description = "Расплавление магии" },
        new MagicData { name = "Магия цепей", theme = MagicTheme.Special, description = "Создание цепей" },
        new MagicData { name = "Управление погодой", theme = MagicTheme.Special, description = "Контроль погоды" },
        new MagicData { name = "Исцеление", theme = MagicTheme.Special, description = "Восстановление здоровья" },
        new MagicData { name = "Вампиризм", theme = MagicTheme.Special, description = "Кража жизненной силы" },
        new MagicData { name = "Магия красок", theme = MagicTheme.Special, description = "Манипуляция красками" },
        new MagicData { name = "Копирование техник", theme = MagicTheme.Special, description = "Копирование способностей" },
        new MagicData { name = "Небесная кара", theme = MagicTheme.Special, description = "Божественное наказание" },
        
        // === БОЖЕСТВЕННАЯ (12 магий) ===
        new MagicData { name = "Созидание", theme = MagicTheme.Divine, description = "Создание материи" },
        new MagicData { name = "Разрушение", theme = MagicTheme.Divine, description = "Разрушение материи" },
        new MagicData { name = "Хаос", theme = MagicTheme.Divine, description = "Управление хаосом" },
        new MagicData { name = "Возрождение", theme = MagicTheme.Divine, description = "Воскрешение" },
        new MagicData { name = "Аннигиляция", theme = MagicTheme.Divine, description = "Полное уничтожение" },
        new MagicData { name = "Разум", theme = MagicTheme.Divine, description = "Контроль разума" },
        new MagicData { name = "Душа", theme = MagicTheme.Divine, description = "Манипуляция душами" },
        new MagicData { name = "Отрицание", theme = MagicTheme.Divine, description = "Отмена магии" },
        new MagicData { name = "Эмоции", theme = MagicTheme.Divine, description = "Управление эмоциями" },
        new MagicData { name = "Код", theme = MagicTheme.Divine, description = "Редактирование кода реальности" },
        new MagicData { name = "Контроль", theme = MagicTheme.Divine, description = "Абсолютный контроль" },
        new MagicData { name = "Режиссура Главного Героя", theme = MagicTheme.Divine, description = "Управление сюжетом" }
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
        return (RaceType)Random.Range((int)RaceType.FirstOrderGodChild, (int)RaceType.Deity + 1);
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
        // Магия НЕ зависит от расы - любое существо может получить любую магию
        // Включая человека с Божественной магией, Амальгаму с магией Времени/Пространства и т.д.
        int randomIndex = Random.Range(0, allMagics.Count);
        return allMagics[randomIndex];
    }

    private bool CanUseSpecialAbility(RaceType race, EnergyType primaryEnergy)
    {
        // Если существо не может пользоваться энергией, то он не может использовать особый навык
        if (primaryEnergy == EnergyType.None)
            return false;

        switch (race)
        {
            case RaceType.PrisonerHuman:
            case RaceType.SeparatedAmalgam:
            case RaceType.FormedAnomaly:
                return true;
            default:
                return true; // Любое существо с энергией может использовать особый прием
        }
    }

    private T RandomEnumValue<T>() where T : System.Enum
    {
        var values = System.Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Range(0, values.Length));
    }
}
