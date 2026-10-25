# Unity Dev Console System Developer Guide

## Purpose

This guide explains how to configure the console, use commands and create custom commands.

The system uses seperate command scripts. Developers can add their own commands without changing the console controller directly.

## System overview

| Script | Responsibility |
| - | - |
| DEV_ConsoleController.cs | Reads the input, registers commands and runs them |
| DEV_ToggleConsole.cs | Opens the console using the configured keys. Closes the console with Esc |
| DEV_ConsoleCommand.cs | The shared structure for command scripts |
| Commands/ClearCommand.cs | Clears the console |
| Commands/HelpCommand.cs | Lists the commands and shows their help information |
| Editor/DEV_CommandCreator.cs | Adds the command creation menu |
| Editor/Templates/DEV_CommandTemplate.txt | The new command script template |

These paths are all under `Assets/DevConsole/` as the root.

## Console Flow

The console follows this structure:
`Input > Command > Registered command > Execute`

The controller checks the first word with the available command names. Any words after are the arguments.

Example:

```
help clear
```

The command is `help`. The argument is `clear`.

Commands are not case sensitive. `help`, `Help` and `HELP` are all the same command.

## Connecting the console to a scene

The console needs:
- A canvas with a console panel
- A TMP text for the output
- A TMP input
- A Commands GameObject with the command scripts
- An EventSystem with the Input System.

Example hierarchy:

```
DevConsole
├── Commands
└── Canvas
    └── ConsolePanel
        ├── OutputText
        └── InputField
```

Add `DEV_ConsoleController` and `DEV_ToggleConsole` to the `DevConsole` GameObject.

### Configure the controller

Add these fields onto `DEV_ConsoleController`

| Field | Assign |
| - | - |
| Output Text | TextMeshPro text to show the command output |
| Input Box | TextMeshPro input field to input commands |
| Commands Object | A GameObject with all the command scripts |

Missing references give errors in Unity's console and disables the controller.

### Configure opening controls

Add these fields onto `DEV_ToggleConsole`

| Field | Assign |
| - | - |
| Dev Console | Console panel to show/hide |
| Input Box | The same as the input used by the controller |
| Required Keys | Keys which are held together to open the console |

Example of keys:

| Array | Key |
| - | - |
| 0 | Digit1 |
| 1 | Digit2 |
| 2 | Digit3 |
| 3 | Enter |

Hold 1 + 2 + 3 + enter together to open the console.

Pressing escape closes the console.

Don't leave RequiredKeys empty or set to None.

## UI Input

The console uses Unity's Input System for keyboard input and TextMeshPro for text.

When creating a scene:
1. Check the Input System package is installed
1. Check **Active Input Handling** is enabled for the Input System
1. Check the scene has an **EventSystem**
1. Check it uses **Input System UI Input Module**
1. Check the Canvas has a **Graphic Raycaster**
1. Set the input field's **Line Type** to **Single Line**

## Using the console
1. Enter Play Mode
1. Hold the keys to open the console
1. Type a command
1. Press enter to input the command
1. Press Esc to close the console

The input field is selected automatically when the console panel opens.

## Existing commands

### clear
Clears the console output.

```
clear
```

This command doesnt have any arguments. Adding arguments will show an error and its usage.

### help

Lists the avaialble commands and their descriptions.

```
help
```

To show the description and usage of a command:

```
help [command]
```

## Command registration

When adding a component script to the Commands GameObject the controller finds all available scripts in start.

Putting a script in the Comamnds folder doesnt register it. The script must be on the Commands GameObject.

The controller doesnt search child objects.

Commands are registered on startup. To add new commands exit Play Mode.

### Command names

Command names must:
- Have at least 1 character
- Have no whitespae
- Be unique

If two command scripts use the same command name, the first is registered and the second is skipped.

Disabed components are skipped when registering.

## Creating custom commands

Creating a custom command has three parts:
1. Create the comamnd script
1. Customise the information and function
1. Attach it to the Commands GameObject

### Create the script
1. In the project window go to the Commands folder.
1. Right click and select `Create > DevConsole > New Command`
1. Name the script. This example uses `HelloWorldCommand`.
1. Open the script

The template will create a class which inherits from DEV_ConsoleCommand.

### Customise the command script

Example:

```
using UnityEngine;

public class HelloWorldCommand : DEV_ConsoleCommand
{
    public override string CommandName => "hello";
    public override string Description => "Outputs 'Hello world!'";
    public override string Usage => "hello";

    public override void Execute(string[] arguments, DEV_ConsoleController console)
    {
        if(arguments.Length > 0)
        {
            console.WriteLine("This command doesn't support arguments");
            console.WriteLine("Usage: " + Usage);

            return;
        }

        // Add implementation here

        console.WriteLine("Hello world!");
    }
}
```

The command doesnt need to match the class name.

### Command information

| Property | Purpose |
| - | - |
| CommandName | The command to input in the console |
| Description | Explaination of the script shown in help |
| Usage | Example how to enter the command |

### Execute

Execute is the command's behaviour.

The controller passes:
- **arguments**: words after the command name.
- **console**: The controller running commands.

## Attach the command
1. Select the Commands GameObject
1. Drag the HelloWorldCommand script into the inspector
1. Check the script is enabled
1. Enter Play Mode

Open the console and type:

```
hello
```

Output:

```
Hello world!
```

You can view the help:

Input:
```
help hello
```

The console will output the description and usage.

## Command arguments

Arguments are passed as strings.

Example:

```
givecoints 50
```

The controller selects the command `givecoins` and passes `"50"` as `argument[0]`.

When adding an argument:
1. Check the number of arguments
1. Check each value eists
1. Perform the action after validation

The template rejects all arguments by default. Change that check when creating a command that has arguments.

## Writing console outputs

Commands can write output by using:
```
console.WriteLine("Output to show");
```

WriteLine adds a new line after the message.

To clear the output you can use:
```
console.ClearOutput();
```

Commands dont need access to the components directly.

Use console.WriteLine for messages the developer can see in the game console. Use Unity's built in debug when the error belongs in the Unity console.

## Project specific commands

Custom commands can have serialize fields for project references.

Add these on the command script in the inspector.

Keep project specific bejaviour in the command instead of adding to DEV_ConsoleController.

## Editor files

Keep DEV_CommandCreator in the Editor folder.

The creation menu uses this path for the template:
```
Assets/DevConsole/Editor/Templates/Dev_CommandTemplate.txt
```

If the template is moved, update the TemplatePath in DEV_CommandCreator.

Keep `#SCRIPTNAME#` in the template. Unity replaces it with the new name when generating the file./

The generated command script should be outside the editor folder.

## Development builds

DEV_ToggleConsole only opens the console in the editor and in development builds.

In other builds, it hides the panel and disables itself.

It doesnt remove the controller or command code from the build.

## Troubleshooting

| Problem | Check |
| - | - |
| The console does not open | Game view focus, RequiredKeys, references, if the script is active |
| Missing references error | Controller and opening script inspector fields |
| Unknown command | CommandName spelling, is the component attached to the Controllers |
| Command script exists but isnt found | Attach it to the Commands GameObject, check it is active |
| Command missing from help | Registraiton error, is the script enabled, is the Command GameObject referenced |
| Duplicate command error | Each commang script has a unique CommandName |
| Whitespace error | Remove spaces or tabs from the CommandName |
| New command doesnt show in Play Mode | Add the command script to the Commands GameObject |
| The creation menu doesnt show | Editor script location, console errors |
| Template not found error | The template at the correct TemplatePath |
| Scripts cannot be attached | Class and filenames matching. It inherits DEV_ConsoleCommand |