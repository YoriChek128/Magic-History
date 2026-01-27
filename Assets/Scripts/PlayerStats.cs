using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StatsChangedEvent : UnityEvent<float, float> { }

public class PlayerStats : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth;

    [Header("Stamina Settings")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float currentStamina;

    [Header("Mana Settings")]
    [SerializeField] private float maxMana = 100f;
    [SerializeField] private float currentMana;
    [SerializeField] private float manaRegenPerSecond = 2f;

    [Header("Events")]
    public StatsChangedEvent OnHealthChanged;
    public StatsChangedEvent OnStaminaChanged;
    public StatsChangedEvent OnManaChanged;

    // Свойства для доступа к значениям
    public float MaxHealth => maxHealth;
    public float CurrentHealth => currentHealth;
    public float MaxStamina => maxStamina;
    public float CurrentStamina => currentStamina;
    public float MaxMana => maxMana;
    public float CurrentMana => currentMana;

    private void Start()
    {
        InitializeStats();
        StartCoroutine(ManaRegeneration());
    }

    private void InitializeStats()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentMana = maxMana;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
        OnManaChanged?.Invoke(currentMana, maxMana);
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

    public void ChangeHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void ChangeStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, maxStamina);
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    public void ChangeMana(float amount)
    {
        currentMana = Mathf.Clamp(currentMana + amount, 0, maxMana);
        OnManaChanged?.Invoke(currentMana, maxMana);
    }

    public bool TryUseMana(float amount)
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

    // Методы для увеличения максимальных значений (можно использовать для улучшений)
    public void IncreaseMaxHealth(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void IncreaseMaxStamina(float amount)
    {
        maxStamina += amount;
        currentStamina += amount;
        OnStaminaChanged?.Invoke(currentStamina, maxStamina);
    }

    public void IncreaseMaxMana(float amount)
    {
        maxMana += amount;
        currentMana += amount;
        OnManaChanged?.Invoke(currentMana, maxMana);
    }
}
