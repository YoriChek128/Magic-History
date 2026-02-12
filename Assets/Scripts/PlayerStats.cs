using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatsChangedEvent : UnityEvent<float, float> { }

public class PlayerStats : MonoBehaviour
{
    [Header("Level Setting")]
    [SerializeField] private int physicalLevel = 0;
    [SerializeField] private int magicLevel = 0;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth;

    [Header("Stamina Settings")]
    [SerializeField] private int maxStamina;
    [SerializeField] private int currentStamina;
    [SerializeField] private int staminaDrainPerSecond;
    [SerializeField] private int staminaRegenPerSecond;

    [Header("Mana Settings")]
    [SerializeField] private int maxMana;
    [SerializeField] private int currentMana;
    [SerializeField] private int manaRegenPerSecond;

    [Header("Events")]
    public StatsChangedEvent OnHealthChanged;
    public StatsChangedEvent OnStaminaChanged;
    public StatsChangedEvent OnManaChanged;

    public StatsChangedEvent OnLevelChanged;

    // Свойства для доступа к значениям
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float MaxStamina => maxStamina;
    public float CurrentStamina => currentStamina;
    public float MaxMana => maxMana;
    public float CurrentMana => currentMana;

    private void Start()
    {
        InitializeStats(physicalLevel, magicLevel);
        StartCoroutine(ManaRegeneration());
    }

    private void InitializeStats(int physical, int magic)
    {
        maxHealth = 50 + 5*physical;
        maxStamina = 20 + 10*physical;
        maxMana = 10 + 10*magic;

        staminaDrainPerSecond = 2;
        staminaRegenPerSecond = 1 + physical / 10;
        manaRegenPerSecond = 1 + magic / 10;

        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentMana = maxMana;
    }

    private System.Collections.IEnumerator ManaRegeneration()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (currentMana < maxMana)
            {
                ChangeMana(manaRegenPerSecond);
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ChangeStamina(int amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, maxStamina);
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    public void ChangeMana(int amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    public bool TryUseMana(int amount)
    {
        if (currentMana >= amount)
        {
            ChangeMana(-amount);
            return true;
        }
        return false;
    }

    private void Die()
    {
        Debug.Log("Игрок погиб!");
        // Здесь можно добавить логику смерти
        Time.timeScale = 0; // Остановка игры
    }

    public int GetHealthAmount()
    {
        return currentHealth;
    }

    public int GetStaminaAmount()
    {
        return currentStamina;
    }

    public int GetManaAmount()
    {
        return currentMana;
    }

    public int GetStaminaDrainAmount()
    {
        return staminaDrainPerSecond;
    }

    public int GetStaminaRegenAmount()
    {
        return staminaRegenPerSecond;
    }
}
