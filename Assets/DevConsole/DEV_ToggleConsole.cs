using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class DEV_ToggleConsole : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject devConsole;
    [SerializeField] private TMP_InputField inputBox;

    [Header("Toggle Controls")]
    [SerializeField] private Key[] requiredKeys;
    private Key toggleKey;

    void Start()
    {
#if !DEVELOPMENT_BUILD && !UNITY_EDITOR
      if (devConsole != null)
        {
            devConsole.SetActive(false);
        }

        enabled = false;
        return;  
#endif

        if(devConsole == null)
        {
            Debug.LogError("DEV_ToggleConsole: MIssing devConsole reference.", this);

            enabled = false;
            return;
        }

        if(inputBox == null)
        {
            Debug.LogError("Dev_ToggleConsole: Missing inputBox reference.", this);

            enabled = false;
            return;
        }

        devConsole.SetActive(false);
    }

    void Update()
    {
        ToggleDevConsole();
    }

    public void ToggleDevConsole()
    {
        Keyboard keyboard = Keyboard.current;

        if(keyboard == null)
        {
            Debug.LogError("DEV_ToggleConsole: Cannot find keyboard. Please check a keyboard is connected", this);

            enabled = false;
            return;
        }

        // Close the devConsole with escape
        if (devConsole.activeSelf)
        {
            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                devConsole.SetActive(false);
            }

            return;
        }

        if(requiredKeys == null || requiredKeys.Length == 0)
        {
            Debug.LogWarning("DEV_ToggleConsole: No keys are set in requiredKeys", this);

            return;
        }

        // Wait for all keys held down
        foreach(Key requiredKey in requiredKeys)
        {
            if(requiredKey == Key.None || !keyboard[requiredKey].isPressed)
            {
                return;
            }
        }

        devConsole.SetActive(true);

        if(inputBox != null)
        {
            inputBox.Select();
            inputBox.ActivateInputField();
        }
    }
}
