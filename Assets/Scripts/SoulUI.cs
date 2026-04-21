using UnityEngine;
using System;

/// <summary>
/// Типы кнопок Soul UI
/// </summary>
public enum SoulUIButtonType
{
    // Базовые кнопки (есть у всех существ)
    Action,     // [Действие] - любое взаимодействие с миром
    Attack,     // [Атака] - физическая атака
    Defend,     // [Защита] - блокировка опасности
    Mercy,      // [Пощада] - отказ от насилия
    Magic,      // [Магия] - использование магии
    Cheats,     // [Читы] - нелегальная копия [Перезапись]
    
    // Кнопки ТОЛЬКО у Богов
    Console,    // [Консоль] - отображение данных
    Rewrite     // [Перезапись] - глобальное редактирование Вселенной
}

/// <summary>
/// Данные кнопки Soul UI
/// </summary>
[System.Serializable]
public class SoulUIButton
{
    public SoulUIButtonType buttonType;
    public string displayName;
    public Color buttonColor;
    public bool isAvailable;
    public bool isGodOnly;
    public string description;
    
    public SoulUIButton(SoulUIButtonType type)
    {
        buttonType = type;
        InitializeButtonData();
    }
    
    private void InitializeButtonData()
    {
        switch (buttonType)
        {
            case SoulUIButtonType.Action:
                displayName = "[Действие]";
                buttonColor = Color.white;
                isAvailable = true;
                isGodOnly = false;
                description = "Любое взаимодействие существа с миром. Пока существо живет, кнопка всегда будет невредима.";
                break;
                
            case SoulUIButtonType.Attack:
                displayName = "[Атака]";
                buttonColor = new Color(1f, 0.3f, 0.3f);
                isAvailable = true;
                isGodOnly = false;
                description = "Любая физическая атака: удар кулаком, мечом и т.д. Может усиливаться магией.";
                break;
                
            case SoulUIButtonType.Defend:
                displayName = "[Защита]";
                buttonColor = new Color(0.3f, 0.6f, 1f);
                isAvailable = true;
                isGodOnly = false;
                description = "Любая блокировка опасности. Защищать можно не только себя.";
                break;
                
            case SoulUIButtonType.Mercy:
                displayName = "[Пощада]";
                buttonColor = new Color(0.3f, 1f, 0.3f);
                isAvailable = true;
                isGodOnly = false;
                description = "Отказ от насилия вовсе, путь переговора.";
                break;
                
            case SoulUIButtonType.Magic:
                displayName = "[Магия]";
                buttonColor = new Color(0.8f, 0.3f, 1f);
                isAvailable = false; // Есть только у тех, кто владеет магией
                isGodOnly = false;
                description = "Использование магии. Кнопка есть у тех, кто обладает мастерством управления энергии.";
                break;
                
            case SoulUIButtonType.Cheats:
                displayName = "[Читы]";
                buttonColor = new Color(1f, 0f, 1f); // Искаженный цвет
                isAvailable = false;
                isGodOnly = false;
                description = "Нелегальная слабая копия кнопки [Перезапись].";
                break;
                
            case SoulUIButtonType.Console:
                displayName = "[Консоль]";
                buttonColor = new Color(0f, 1f, 1f);
                isAvailable = false;
                isGodOnly = true;
                description = "Отображение всех данных о мире, объекте и т.п. Не позволяет редактировать данные, только вывод.";
                break;
                
            case SoulUIButtonType.Rewrite:
                displayName = "[Перезапись]";
                buttonColor = new Color(1f, 0.5f, 0f);
                isAvailable = false;
                isGodOnly = true;
                description = "Кнопка глобального редактирования Вселенной. Любая команда. Работает только внутри определенной Вселенной.";
                break;
        }
    }
}

/// <summary>
/// Компонент управления Soul UI
/// </summary>
public class SoulUI : MonoBehaviour
{
    [Header("Soul UI Buttons")]
    [SerializeField] private SoulUIButton[] availableButtons;
    
    [Header("God Settings")]
    [SerializeField] private bool isDeity = false;
    [SerializeField] private int universeID = 8; // Для ограничения [Перезапись]
    
    [Header("References")]
    [SerializeField] private EnergySystem energySystem;
    [SerializeField] private MagicSystem magicSystem;
    [SerializeField] private RaceSystem raceSystem;
    
    [Header("Events")]
    public event Action<SoulUIButtonType> OnButtonPressed;
    public event Action<string> OnConsoleCommand;
    
    private void Start()
    {
        InitializeButtons();
        
        if (energySystem == null) energySystem = GetComponent<EnergySystem>();
        if (magicSystem == null) magicSystem = GetComponent<MagicSystem>();
        if (raceSystem == null) raceSystem = GetComponent<RaceSystem>();
    }
    
    /// <summary>
    /// Инициализация кнопок
    /// </summary>
    private void InitializeButtons()
    {
        // Базовые кнопки всегда доступны
        availableButtons = new SoulUIButton[]
        {
            new SoulUIButton(SoulUIButtonType.Action),
            new SoulUIButton(SoulUIButtonType.Attack),
            new SoulUIButton(SoulUIButtonType.Defend),
            new SoulUIButton(SoulUIButtonType.Mercy)
        };
        
        // Проверка на наличие магии
        if (magicSystem != null && magicSystem.CanUseMagic())
        {
            AddButton(new SoulUIButton(SoulUIButtonType.Magic));
        }
        
        // Кнопка [Читы] - редкая, только для особых случаев
        // AddButton(new SoulUIButton(SoulUIButtonType.Cheats));
        
        // Божественные кнопки
        if (isDeity)
        {
            AddButton(new SoulUIButton(SoulUIButtonType.Console));
            AddButton(new SoulUIButton(SoulUIButtonType.Rewrite));
        }
    }
    
    /// <summary>
    /// Добавить кнопку
    /// </summary>
    private void AddButton(SoulUIButton button)
    {
        var newList = new SoulUIButton[availableButtons.Length + 1];
        availableButtons.CopyTo(newList, 0);
        newList[newList.Length - 1] = button;
        availableButtons = newList;
    }
    
    /// <summary>
    /// Нажать кнопку
    /// </summary>
    public bool PressButton(SoulUIButtonType buttonType, params object[] parameters)
    {
        SoulUIButton button = GetButton(buttonType);
        
        if (button == null || !button.isAvailable)
        {
            Debug.LogWarning($"Кнопка {buttonType} недоступна!");
            return false;
        }
        
        Debug.Log($"Нажата кнопка: {button.displayName}");
        Debug.Log(button.description);
        
        OnButtonPressed?.Invoke(buttonType);
        
        return ExecuteButtonAction(buttonType, parameters);
    }
    
    /// <summary>
    /// Получить кнопку по типу
    /// </summary>
    public SoulUIButton GetButton(SoulUIButtonType buttonType)
    {
        foreach (var button in availableButtons)
        {
            if (button.buttonType == buttonType)
                return button;
        }
        return null;
    }
    
    /// <summary>
    /// Выполнить действие кнопки
    /// </summary>
    private bool ExecuteButtonAction(SoulUIButtonType buttonType, object[] parameters)
    {
        switch (buttonType)
        {
            case SoulUIButtonType.Action:
                return ExecuteAction(parameters);
                
            case SoulUIButtonType.Attack:
                return ExecuteAttack(parameters);
                
            case SoulUIButtonType.Defend:
                return ExecuteDefend(parameters);
                
            case SoulUIButtonType.Mercy:
                return ExecuteMercy(parameters);
                
            case SoulUIButtonType.Magic:
                return ExecuteMagic(parameters);
                
            case SoulUIButtonType.Cheats:
                return ExecuteCheats(parameters);
                
            case SoulUIButtonType.Console:
                return ExecuteConsole(parameters);
                
            case SoulUIButtonType.Rewrite:
                return ExecuteRewrite(parameters);
                
            default:
                return false;
        }
    }
    
    private bool ExecuteAction(object[] parameters)
    {
        Debug.Log("[Действие] выполнено - взаимодействие с миром");
        // Логика взаимодействия с миром
        return true;
    }
    
    private bool ExecuteAttack(object[] parameters)
    {
        Debug.Log("[Атака] выполнена - физическая атака");
        
        // Параметры атаки
        Vector3 target = parameters.Length > 0 && parameters[0] is Vector3 v ? v : Vector3.zero;
        int damage = parameters.Length > 1 && parameters[1] is int d ? d : 10;
        
        Debug.Log($"Цель: {target}, Урон: {damage}");
        
        // Здесь должна быть логика нанесения урона
        return true;
    }
    
    private bool ExecuteDefend(object[] parameters)
    {
        Debug.Log("[Защита] активирована - блокировка опасности");
        
        GameObject target = parameters.Length > 0 && parameters[0] is GameObject g ? g : gameObject;
        int defenseValue = parameters.Length > 1 && parameters[1] is int d ? d : 5;
        
        Debug.Log($"Защищаем: {target.name}, Защита: {defenseValue}");
        
        return true;
    }
    
    private bool ExecuteMercy(object[] parameters)
    {
        Debug.Log("[Пощада] - отказ от насилия, попытка переговоров");
        // Логика дипломатии/переговоров
        return true;
    }
    
    private bool ExecuteMagic(object[] parameters)
    {
        if (magicSystem == null || !magicSystem.CanUseMagic())
        {
            Debug.LogWarning("Магия недоступна!");
            return false;
        }
        
        Debug.Log("[Магия] - использование магической техники");
        
        if (parameters.Length > 0 && parameters[0] is MagicType magicType)
        {
            Vector3 target = parameters.Length > 1 && parameters[1] is Vector3 v ? v : Vector3.zero;
            return magicSystem.UseTechnique(magicType, target);
        }
        
        return true;
    }
    
    private bool ExecuteCheats(object[] parameters)
    {
        Debug.LogWarning("[Читы] - нелегальное действие! Слабая копия [Перезапись]");
        
        // Очень ограниченные возможности по сравнению с настоящей [Перезапись]
        if (parameters.Length > 0 && parameters[0] is string cheatCode)
        {
            Debug.Log($"Чит-код: {cheatCode}");
            // Минимальные изменения
        }
        
        return false; // Чаще всего не работает
    }
    
    private bool ExecuteConsole(object[] parameters)
    {
        if (!isDeity)
        {
            Debug.LogWarning("[Консоль] доступна только Богам!");
            return false;
        }
        
        Debug.Log("[Консоль] активирована");
        
        // Сбор информации о мире
        string worldInfo = "";
        
        if (energySystem != null)
        {
            worldInfo += energySystem.GetEnergyInfo() + "\n";
        }
        
        if (magicSystem != null)
        {
            worldInfo += magicSystem.GetMagicInfo() + "\n";
        }
        
        if (raceSystem != null)
        {
            worldInfo += raceSystem.GetRaceInfo() + "\n";
        }
        
        Debug.Log("=== КОНСОЛЬ ===\n" + worldInfo);
        
        OnConsoleCommand?.Invoke(worldInfo);
        
        return true;
    }
    
    private bool ExecuteRewrite(object[] parameters)
    {
        if (!isDeity)
        {
            Debug.LogWarning("[Перезапись] доступна только Богам!");
            return false;
        }
        
        Debug.Log($"[Перезапись] - редактирование Вселенной #{universeID}");
        
        if (parameters.Length > 0 && parameters[0] is string command)
        {
            Debug.Log($"Команда перезаписи: {command}");
            
            // Глобальное редактирование Вселенной
            // Это самая мощная кнопка в игре
            
            OnConsoleCommand?.Invoke($"[ПЕРЕЗАПИСЬ] Команда: {command}");
            
            return true;
        }
        
        Debug.LogWarning("Необходимо указать команду для [Перезапись]!");
        return false;
    }
    
    /// <summary>
    /// Проверить доступность кнопки Магии
    /// </summary>
    public void CheckMagicAvailability()
    {
        if (magicSystem != null && magicSystem.CanUseMagic())
        {
            SoulUIButton magicButton = GetButton(SoulUIButtonType.Magic);
            if (magicButton != null)
            {
                magicButton.isAvailable = true;
            }
            else
            {
                AddButton(new SoulUIButton(SoulUIButtonType.Magic));
            }
        }
    }
    
    /// <summary>
    /// Сделать существо Богом (добавить божественные кнопки)
    /// </summary>
    public void AscendToDeity(int universeID)
    {
        isDeity = true;
        this.universeID = universeID;
        
        Debug.Log("Существо возвысилось до Бога!");
        
        AddButton(new SoulUIButton(SoulUIButtonType.Console));
        AddButton(new SoulUIButton(SoulUIButtonType.Rewrite));
    }
    
    /// <summary>
    /// Получить список всех доступных кнопок
    /// </summary>
    public string GetAvailableButtonsInfo()
    {
        string info = "=== SOUL UI КНОПКИ ===\n";
        
        foreach (var button in availableButtons)
        {
            info += $"{button.displayName} - {(button.isAvailable ? "Доступно" : "Недоступно")}\n";
            if (button.isGodOnly)
            {
                info += "  [ТОЛЬКО ДЛЯ БОГОВ]\n";
            }
        }
        
        return info;
    }
}
