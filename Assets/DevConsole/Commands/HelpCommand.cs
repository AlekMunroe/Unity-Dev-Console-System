public class HelpCommand : DEV_ConsoleCommand
{
    public override string CommandName => "help";
    public override string Description => "Lists the available commands";
    public override string Usage => "help [command]";

    public override void Execute(string[] arguments, DEV_ConsoleController console)
    {
        if(arguments.Length > 1)
        {
            console.WriteLine("Usage: " + Usage);
            return;
        }

        // Add implementation here

        if(arguments.Length == 1)
        {
            string requestedCommand = arguments[0];

            foreach(DEV_ConsoleCommand command in console.RegisteredCommands)
            {
                if(command == null || !command.isActiveAndEnabled)
                {
                    continue;
                }

                if(string.Equals(command.CommandName, requestedCommand, System.StringComparison.OrdinalIgnoreCase))
                {
                    console.WriteLine(command.CommandName);
                    console.WriteLine("Description: " + command.Description);
                    console.WriteLine("Usage: " + command.Usage);

                    return;
                }
            }

            console.WriteLine("Unknown command: " + requestedCommand);
            console.WriteLine("Enter 'help' to list all available commands");

            return;
        }

        // No arguments
        console.WriteLine("Available commands:");

        foreach(DEV_ConsoleCommand command in console.RegisteredCommands)
        {
            if(command == null || !command.isActiveAndEnabled)
            {
                continue;
            }

            console.WriteLine(command.CommandName + " - " + command.Description);
        }

        console.WriteLine("");
        console.WriteLine("Enter 'help <command>' for it's description and usage");
    }
}

