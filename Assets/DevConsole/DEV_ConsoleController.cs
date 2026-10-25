using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DEV_ConsoleController : MonoBehaviour
{
    [Header("Console")]
    [SerializeField] private TMP_Text outputText;
    [SerializeField] private TMP_InputField inputBox;

    [Header("Commands")]
    [SerializeField] private GameObject commandsObject;

    private readonly Dictionary<string, DEV_ConsoleCommand> registeredCommands = new Dictionary<string, DEV_ConsoleCommand>(System.StringComparer.OrdinalIgnoreCase);

    void Start()
    {
        if(outputText == null || inputBox == null || commandsObject == null)
        {
            Debug.LogError("DEV_ConsoleController: Missing references", this);

            enabled = false;
            return;
        }

        RegisterCommands();
        inputBox.onEndEdit.AddListener(OnEndEdit);
    }

    void OnEndEdit(string command)
    {
        Keyboard keyboard = Keyboard.current;

        if(keyboard == null)
        {
            return;
        }

        if (keyboard.enterKey.wasPressedThisFrame || keyboard.numpadEnterKey.wasPressedThisFrame)
        {
            SubmitCommand();
        }
    }

    private void RegisterCommands()
    {
        registeredCommands.Clear();

        DEV_ConsoleCommand[] commands = commandsObject.GetComponents<DEV_ConsoleCommand>();

        foreach(DEV_ConsoleCommand command in commands)
        {
            if (!command.enabled)
            {
                continue;
            }

            string commandName = command.CommandName;

            if (string.IsNullOrWhiteSpace(commandName))
            {
                Debug.LogError("DEV_ConsoleController: A command has an empty name", command);

                continue;
            }

            bool containsWhitespace = false;

            foreach(char character in commandName)
            {
                if (char.IsWhiteSpace(character))
                {
                    containsWhitespace = true;

                    break;
                }
            }

            if (containsWhitespace)
            {
                Debug.LogError("DEV_ConsoleController: A command cant contain empty space: " + commandName, command);

                continue;
            }

            if (registeredCommands.ContainsKey(commandName))
            {
                Debug.LogError("DEV_ConsoleController: Duplicate command name " + commandName, command);

                continue;
            }

            registeredCommands.Add(commandName, command);
        }
    }

    public IEnumerable<DEV_ConsoleCommand> RegisteredCommands => registeredCommands.Values;

    public void WriteLine(string message)
    {
        outputText.text += message + "\n";
    }

    public void ClearOutput()
    {
        outputText.text = "";
    }

    void SubmitCommand()
    {
        //Trim extra spaces
        string input = inputBox.text.Trim();
        inputBox.text = "";

        inputBox.Select();
        inputBox.ActivateInputField();

        //If nothing is entered, ignore
        if (string.IsNullOrWhiteSpace(input))
        {
            return;
        }

        ClearOutput();
        WriteLine("> " + input);

        // Split when on whitespace, ignore repeated spaces
        string[] parts = input.Split((char[])null, System.StringSplitOptions.RemoveEmptyEntries);
        string commandName = parts[0];

        // Argument after the command
        string[] arguments = new string[parts.Length - 1];

        System.Array.Copy(parts, 1, arguments, 0, arguments.Length);

        if(!registeredCommands.TryGetValue(commandName, out DEV_ConsoleCommand command))
        {
            WriteLine("Unknown command: " + commandName);
            return;
        }

        if(command == null || !command.isActiveAndEnabled)
        {
            WriteLine("Command unavailable: " + commandName);

            return;
        }

        command.Execute(arguments, this);
    }

    private void OnDestroy()
    {
        if(inputBox != null)
        {
            inputBox.onEndEdit.RemoveListener(OnEndEdit);
        }
    }
}
