using UnityEngine;
using UnityEditor;
using System.IO;

public static class DEV_CommandCreator
{
    private const string TemplatePath = "Assets/DevConsole/Editor/Templates/DEV_CommandTemplate.txt";

    [MenuItem("Assets/Create/DevConsole/New Command")]
    private static void CreateCommand()
    {
        if (!File.Exists(TemplatePath))
        {
            Debug.LogError("DEV_CommandCreator: Command template does not exist at " + TemplatePath);

            return;
        }

        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(TemplatePath, "NewConsoleCommand.cs");
    }
}
