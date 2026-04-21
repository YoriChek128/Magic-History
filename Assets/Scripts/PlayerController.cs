using UnityEngine;

/// <summary>
/// Пример управления персонажем в игре
/// Этот скрипт демонстрирует, как использовать все системы вместе
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("References")]
    private EnergySystem energySystem;
    private MagicSystem magicSystem;
    private RaceSystem raceSystem;
    private SoulUI soulUI;
    private CharacterGenerator characterGenerator;
    
    [Header("Input Settings")]
    [SerializeField] private KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode defendKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode magicKey = KeyCode.E;
    [SerializeField] private KeyCode specialAbilityKey = KeyCode.Q;
    [SerializeField] private KeyCode interactKey = KeyCode.F;
    [SerializeField] private KeyCode mercyKey = KeyCode.T;
    
    [Header("Camera")]
    [SerializeField] private Camera mainCamera;
    
    private void Start()
    {
        // Получаем компоненты
        energySystem = GetComponent<EnergySystem>();
        magicSystem = GetComponent<MagicSystem>();
        raceSystem = GetComponent<RaceSystem>();
        soulUI = GetComponent<SoulUI>();
        characterGenerator = GetComponent<CharacterGenerator>();
        
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        Debug.Log("=== Игрок готов ===");
        Debug.Log("Нажмите G для генерации случайного персонажа");
        Debug.Log("Нажмите R для перегенерации");
    }
    
    private void Update()
    {
        HandleGenerationInput();
        HandleCombatInput();
        HandleInteractionInput();
    }
    
    /// <summary>
    /// Управление генерацией персонажа
    /// </summary>
    private void HandleGenerationInput()
    {
        // Генерация случайного персонажа
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (characterGenerator != null)
            {
                characterGenerator.GenerateRandomCharacter();
                Debug.Log(characterGenerator.GetCharacterStats());
            }
        }
        
        // Перегенерация
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (characterGenerator != null)
            {
                characterGenerator.RerollCharacter();
                Debug.Log(characterGenerator.GetCharacterStats());
            }
        }
    }
    
    /// <summary>
    /// Боевое управление
    /// </summary>
    private void HandleCombatInput()
    {
        // Атака [Атака]
        if (Input.GetKeyDown(attackKey))
        {
            PerformAttack();
        }
        
        // Защита [Защита]
        if (Input.GetKeyDown(defendKey))
        {
            PerformDefend();
        }
        
        // Магия [Магия]
        if (Input.GetKeyDown(magicKey))
        {
            UseMagic();
        }
        
        // Особый прием расы
        if (Input.GetKeyDown(specialAbilityKey))
        {
            UseSpecialAbility();
        }
    }
    
    /// <summary>
    /// Взаимодействие с миром
    /// </summary>
    private void HandleInteractionInput()
    {
        // Действие [Действие]
        if (Input.GetKeyDown(interactKey))
        {
            PerformAction();
        }
        
        // Пощада [Пощада]
        if (Input.GetKeyDown(mercyKey))
        {
            ShowMercy();
        }
    }
    
    /// <summary>
    /// Выполнить атаку
    /// </summary>
    private void PerformAttack()
    {
        Vector3 targetPosition = GetTargetPosition();
        
        // Используем кнопку [Атака] из Soul UI
        if (soulUI != null)
        {
            soulUI.PressButton(SoulUIButtonType.Attack, targetPosition, 10);
        }
        
        // Здесь можно добавить анимацию атаки, рейкасты и т.д.
        Debug.Log($"Атака в точку: {targetPosition}");
    }
    
    /// <summary>
    /// Выполнить защиту
    /// </summary>
    private void PerformDefend()
    {
        if (soulUI != null)
        {
            soulUI.PressButton(SoulUIButtonType.Defend, gameObject, 5);
        }
        
        Debug.Log("Активирована защита!");
    }
    
    /// <summary>
    /// Использовать магию
    /// </summary>
    private void UseMagic()
    {
        if (magicSystem == null || !magicSystem.CanUseMagic())
        {
            Debug.LogWarning("Магия недоступна!");
            return;
        }
        
        Vector3 targetPosition = GetTargetPosition();
        
        // Используем первую известную технику для примера
        // В реальной игре здесь должен быть выбор техники из меню
        MagicType techniqueToUse = GetFirstKnownTechnique();
        
        if (techniqueToUse != MagicType.None)
        {
            magicSystem.UseTechnique(techniqueToUse, targetPosition);
            
            // Также можно вызвать через Soul UI
            if (soulUI != null)
            {
                soulUI.PressButton(SoulUIButtonType.Magic, techniqueToUse, targetPosition);
            }
        }
    }
    
    /// <summary>
    /// Использовать особый прием расы
    /// </summary>
    private void UseSpecialAbility()
    {
        if (raceSystem == null)
        {
            Debug.LogWarning("RaceSystem не найден!");
            return;
        }
        
        // Пример параметров для разных рас
        object[] parameters = new object[] { GetTargetPosition(), 5f };
        
        bool success = raceSystem.UseSpecialAbility(parameters);
        
        if (success)
        {
            Debug.Log($"Особый прием расы {raceSystem.CurrentRaceType} использован!");
        }
    }
    
    /// <summary>
    /// Выполнить действие
    /// </summary>
    private void PerformAction()
    {
        if (soulUI != null)
        {
            soulUI.PressButton(SoulUIButtonType.Action);
        }
        
        Debug.Log("Взаимодействие с миром...");
        
        // Здесь может быть логика взаимодействия с объектами
        // Raycast, диалоги, поднятие предметов и т.д.
    }
    
    /// <summary>
    /// Показать пощаду
    /// </summary>
    private void ShowMercy()
    {
        if (soulUI != null)
        {
            soulUI.PressButton(SoulUIButtonType.Mercy);
        }
        
        Debug.Log("Попытка переговоров...");
        
        // Логика дипломатии/переговоров с врагами
    }
    
    /// <summary>
    /// Получить позицию цели (курсор мыши в мире)
    /// </summary>
    private Vector3 GetTargetPosition()
    {
        if (mainCamera == null) return Vector3.zero;
        
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            return hit.point;
        }
        
        // Если не попали в объект, используем точку перед камерой
        return ray.GetPoint(10f);
    }
    
    /// <summary>
    /// Получить первую известную технику
    /// </summary>
    private MagicType GetFirstKnownTechnique()
    {
        // В реальной игре здесь должен быть UI выбора техники
        // Для примера просто возвращаем Огонь если доступен
        if (magicSystem != null && magicSystem.IsTechniqueKnown(MagicType.Fire))
        {
            return MagicType.Fire;
        }
        
        // Или любую другую доступную технику
        return MagicType.Special;
    }
    
    /// <summary>
    /// Открыть Аномальную зону (для Аномалий)
    /// </summary>
    public void OpenAnomalyZone(AnomalyZoneType zoneType)
    {
        if (magicSystem == null)
        {
            Debug.LogWarning("MagicSystem не найден!");
            return;
        }
        
        Vector3 centerPosition = transform.position + transform.forward * 5f;
        float radius = 10f;
        
        bool success = magicSystem.OpenZone(zoneType, centerPosition, radius);
        
        if (success)
        {
            Debug.Log($"Открыта Аномальная зона: {zoneType}");
        }
        else
        {
            Debug.LogWarning("Не удалось открыть зону!");
        }
    }
    
    /// <summary>
    /// Пробудить персонажа (если возможно)
    /// </summary>
    public void AwakenCharacter()
    {
        if (raceSystem == null)
        {
            Debug.LogWarning("RaceSystem не найден!");
            return;
        }
        
        raceSystem.Awaken();
    }
    
    /// <summary>
    /// Показать полную статистику персонажа
    /// </summary>
    public void ShowFullStats()
    {
        string stats = "=== ПОЛНАЯ СТАТИСТИКА ===\n\n";
        
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
        
        Debug.Log(stats);
    }
}
