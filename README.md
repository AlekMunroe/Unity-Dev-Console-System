# Unity Dev Console System

A dev console built for Unity. Developers can create commands and enter them during gameplay.

This was originally developed for The Story of Kate Webster to help with testing. This project is seperated from that game into a standalone Unity project with a demo scene.

## Features

- Configurable combination to open the console
- Escape to close the console
- TMP input and output
- Seperate command components
- Automatic command detection
- Help command
- Clear command
- Command creation menu and a script template
- Arguments
- Console opening for editor and development builds

## Requirements

The project uses Unity `6000.3.19f1`

**Dependencies**
- Unity Input System
- TextMeshPro

The required packages are saved in `Packages/manifest.json`. Unity will restore them when opening the project.

## Installation

**Clone the full project**

```
git clone https://github.com/AlekMunroe/Unity-Dev-Console-System.git
```

1. Open Unity Hub
2. Add the cloned folder
3. Open it in Unity `6000.3.19f1`.
4. Allow Unity to import assets and restore packages
1. Check the console for errors

**Use in existing project**

1. Download the `.unitypackage` file
1. In Unity go to `Assets > Import Package > Custom Package`
1. Select the downloaded `.unitypackage` file
1. Click **Import** and keep the files in `Assets/DevConsole`
1. Drag the prefab in `Assets/DevConsole/Prefabs` into your scene

Try the demo in `Assets/DevConsole/Demo` and enter Play Mode.

The command creation script expects a template at
```
Assets/DevConsole/Editor/Templates/DEV_CommandTemplate.txt
```

See [Developer guide](Documentation/DeveloperGuide.md) for how to create custom commands.

## Setting up the console

The scene requires:
- A console panel with TextMeshPro output and an input field
- An active GameObject with Dev_ConsoleController and Dev_ToggleConsole
- A Commands GameObject with the command components
- An EventSystem using the Input System UI Input Module

on **DEV_ConsoleController** assign:

| Field | Assign |
| - | - |
| Output Text | TextMeshPro Text component to show the output |
| Input Box | TextMeshPro Input to type commands |
| Commands Object | GameObject with command components attached |

On **DEV_ToggleConsole** assign:

| Field | Assign |
| - | - |
| Dev Console | Panel to show/hide |
| Input Box | The same input assigned to DEV_ConsoleController |
| Required Keys | Keys which are held together to open the console |

Attach ClearCommand and HelpCommand to the Commands GameObject

## Using the console

1. Enter Play Mode
1. Hold all the keys set in **Required Keys**
1. Type a command
1. Press enter to send the command
1. Press Esc to close the console

## Existing commands

| Command | What it does |
| - | - |
| clear | Clears the console output |
| help | List the commands and their descriptions |
| help <command> | Shows a specific command's description and its usage |

Example:
```
help clear
```

Output:
```
clear
Description: Clears the console
Usage: clear
```

Commands are not case sensitive.

## How the system works

The system uses three parts
- **DEV_ToggleConsole**: Uses the set keys to show the console panel
- **DEV_ConsoleController**: Runs commands, reads inputs and runs the correct commands
- **DEV_ConsoleCommand**: Handles the shared structure which is inherited by commands

The command flow:
`Input > Command > Registered command > Execute`

The controller finds each command component which is attached directly on the Commands GameObject on start.

Each command contains:
- CommandName
- Description
- Usage
- Execute

The command handles its own data and behaviour.

## Project structure

```
Assets/
└── DevConsole/
    ├── Commands/
    │   ├── ClearCommand.cs
    │   └── HelpCommand.cs
    ├── Editor/
    │   ├── DEV_CommandCreator.cs
    │   ├── Templates/
    │   └── DEV_CommandTemplate.txt
    ├── DEV_ConsoleCommand.cs
    ├── DEV_ConsoleController.cs
    └── DEV_ToggleConsole.cs
```

## Creating custom commands

1. In the project window, right click in the Commands folder
1. Select `Create > DevConsole > New Command`
1. Name the script
1. Customise CommandName, Description and Usage
1. Add custom behaviour inside Execute()
1. Drag the script into the Commands GameObject

The command script must be on the Commands as a component.

See [Developer guide](Documentation/DeveloperGuide.md) for the hello world command example.

## Troubleshooting

### The console doesn't open
- Check the opening script is on a GameObject outside of the hidden panel
- Check the references and Required Keys are set.

### The input field doesnt respong
- Check the scene has an EventSystem using the Input System UI Input Module.
- Check Active Input Handling supports the Input System.

### A command is unknown
- Check the script is on the Commands GameObject and it is enabled.
- Check the CommandName and look for errors in Unity's console.

### A duplicate command error
- Two commands have the same CommandName.
- Give each command a unique name.

### The command creation menu is missing
- Check DEV_CommandCreator is in the Editor folder.

### The template cant be found
- Check the template is in `Assets/DevConsole/Editor/Templates/DEV_CommandTemplate.txt`

## Documentation

- [Developer guide](Documentation/DeveloperGuide.md)

## Author
Alek Paul Munroe

[GitHub](https://github.com/AlekMunroe/)