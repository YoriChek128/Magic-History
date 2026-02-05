using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private Image health;
    [SerializeField] private TMP_Text healthText;

    [Header("Stamina")]
    [SerializeField] private Image stamina;
    [SerializeField] private TMP_Text staminaText;

    [Header("Mana")]
    [SerializeField] private Image mana;
    [SerializeField] private TMP_Text manaText;

    private PlayerStats playerStats;

    private void Start()
    {
        playerStats = FindFirstObjectByType<PlayerStats>();
    }

    private void Update()
    {
        if (playerStats != null)
        {
            // Инициализация UI
            UpdateHealthUI(playerStats.CurrentHealth, playerStats.MaxHealth);
            UpdateStaminaUI(playerStats.CurrentStamina, playerStats.MaxStamina);
            UpdateManaUI(playerStats.CurrentMana, playerStats.MaxMana);
        }
    }

    private void UpdateHealthUI(float current, float max)
    {
        if (health != null)
        {
            health.fillAmount = current / max;
        }

        if (healthText != null)
        {
            healthText.text = $"{Mathf.RoundToInt(current)}/{Mathf.RoundToInt(max)}";
        }
    }

    private void UpdateStaminaUI(float current, float max)
    {
        if (stamina != null)
        {
            stamina.fillAmount = current / max;
        }

        if (staminaText != null)
        {
            staminaText.text = $"{Mathf.RoundToInt(current)}/{Mathf.RoundToInt(max)}";
        }
    }

    private void UpdateManaUI(float current, float max)
    {
        if (mana != null)
        {
            mana.fillAmount = current / max;
        }

        if (manaText != null)
        {
            manaText.text = $"{Mathf.RoundToInt(current)}/{Mathf.RoundToInt(max)}";
        }
    }
}