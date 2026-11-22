using System;
using UnityEngine;

//Created with claude AI help

// Accessibility options shared by every script. The values are saved with PlayerPrefs.

public static class AccessibilitySettings
{
    private const string AutoFireKey = "AutoFire";

    private static bool loaded;
    private static bool autoFire;

    //Raised when the option changes (the UI listens to it)
    public static event Action<bool> OnAutoFireChanged;

    //When true, Aqua shoots without holding the mouse button.
    public static bool AutoFire
    {
        get
        {
            if (!loaded)
            {
                autoFire = PlayerPrefs.GetInt(AutoFireKey, 0) == 1;// off by default
                loaded = true;
            }
            return autoFire;
        }
        set
        {
            autoFire = value;
            loaded = true;
            PlayerPrefs.SetInt(AutoFireKey, value ? 1 : 0);
            PlayerPrefs.Save();
            OnAutoFireChanged?.Invoke(value);
        }
    }
}