using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

// created with claude help
// Turns the auto fire on and off with a key and/or a UI Toggle.
// Put it on an object that is always active in the scene.

public class AutoFireToggle : MonoBehaviour
{
    [Tooltip("Optional. A UI Toggle that shows and changes the option.")]
    [SerializeField] private Toggle toggle;
    [Tooltip("Optional. Text that shows 'Auto-disparo (F): ON/OFF'.")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Key shortcutKey = Key.F;

    private void OnEnable()
    {
        AccessibilitySettings.OnAutoFireChanged += Refresh;

        if (toggle != null)
        {
            toggle.onValueChanged.AddListener(OnToggleClicked);
        }

        Refresh(AccessibilitySettings.AutoFire);// show the saved value at the start
    }

    private void OnDisable()
    {
        AccessibilitySettings.OnAutoFireChanged -= Refresh;

        if (toggle != null)
        {
            toggle.onValueChanged.RemoveListener(OnToggleClicked);
        }
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current[shortcutKey].wasPressedThisFrame)
        {
            AccessibilitySettings.AutoFire = !AccessibilitySettings.AutoFire;
        }
    }

    private void OnToggleClicked(bool isOn)
    {
        AccessibilitySettings.AutoFire = isOn;
    }

    private void Refresh(bool isOn)
    {
        if (toggle != null)
        {
            toggle.SetIsOnWithoutNotify(isOn);// update the checkbox without triggering OnToggleClicked
        }

        if (statusText != null)
        {
            statusText.text = "Auto-disparo (F): " + (isOn ? "ON" : "OFF");
        }
    }
}