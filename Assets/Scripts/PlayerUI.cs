using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text healthText;

    [Header("Stamina")]
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private Text staminaText;

    [Header("Mana")]
    [SerializeField] private Slider manaSlider;
    [SerializeField] private Text manaText;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.OnHealthChanged.AddListener(UpdateHealthUI);
            playerStats.OnStaminaChanged.AddListener(UpdateStaminaUI);
            playerStats.OnManaChanged.AddListener(UpdateManaUI);

            // Инициализация UI
            UpdateHealthUI(playerStats.CurrentHealth, playerStats.MaxHealth);
            UpdateStaminaUI(playerStats.CurrentStamina, playerStats.MaxStamina);
            UpdateManaUI(playerStats.CurrentMana, playerStats.MaxMana);
        }
    }

    private void UpdateHealthUI(float current, float max)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.value = current;
        }

        if (healthText != null)
        {
            healthText.text = $"{Mathf.RoundToInt(current)}/{Mathf.RoundToInt(max)}";
        }
    }

    private void UpdateStaminaUI(float current, float max)
    {
        if (staminaSlider != null)
        {
            staminaSlider.maxValue = max;
            staminaSlider.value = current;
        }

        if (staminaText != null)
        {
            staminaText.text = $"{Mathf.RoundToInt(current)}/{Mathf.RoundToInt(max)}";
        }
    }

    private void UpdateManaUI(float current, float max)
    {
        if (manaSlider != null)
        {
            manaSlider.maxValue = max;
            manaSlider.value = current;
        }

        if (manaText != null)
        {
            manaText.text = $"{Mathf.RoundToInt(current)}/{Mathf.RoundToInt(max)}";
        }
    }
}