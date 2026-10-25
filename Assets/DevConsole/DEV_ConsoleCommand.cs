using UnityEngine;

public abstract class DEV_ConsoleCommand : MonoBehaviour
{
    public abstract string CommandName { get; }
    public abstract string Description { get; }
    public abstract string Usage { get; }

    public abstract void Execute(string[] arguments, DEV_ConsoleController console);
}
