using UnityEngine;

public class ClearCommand : DEV_ConsoleCommand
{
    public override string CommandName => "clear";
    public override string Description => "Clears the console";
    public override string Usage => "clear";

    public override void Execute(string[] arguments, DEV_ConsoleController console)
    {
        if(arguments.Length > 0)
        {
            console.WriteLine("This command doesn't support arguments");
            console.WriteLine("Usage: " + Usage);

            return;
        }

        console.ClearOutput();
    }
}
