using TMPro;
using UnityEngine;
using UnityEngine.UI;


// Shows the player's health on the HUD life bar (a Slider from 0 to 1) and as a percentage.

public class LifebarUI : MonoBehaviour
{
    [SerializeField] private Slider healthBar;
    [SerializeField] private TMP_Text healthValueText;

    // Subscribe/unsubscribe with the object so no stale listener stays after a scene reload
    private void OnEnable()
    {
        PlayerHealth.onHealthPercentageChanged += UpdateBar;
    }

    private void OnDisable()
    {
        PlayerHealth.onHealthPercentageChanged -= UpdateBar;
    }

    private void UpdateBar(float percentage)
    {
        float clamped = Mathf.Clamp01(percentage);

        healthBar.value = clamped;
        healthValueText.text = Mathf.RoundToInt(clamped * 100f) + "%";
    }
}