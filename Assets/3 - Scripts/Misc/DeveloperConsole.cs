using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class DeveloperConsole
{
    private readonly string prefix;
    private readonly IEnumerable<ConsoleCommand> commands;

    public DeveloperConsole(string prefix, IEnumerable<ConsoleCommand> commands)
    {
        this.prefix = prefix;
        this.commands = commands;
    }

    public void ProcessCommand(string inputValue)
    {
        if (!inputValue.StartsWith(prefix)) { return; }

        inputValue = inputValue.Remove(0, prefix.Length);

        string[] inputSplit = inputValue.Split(' ');

        string commandInput = inputSplit[0];
        string[] args = inputSplit.Skip(1).ToArray();

        ProcessCommand(commandInput, args);
    }

    public void ProcessCommand(string commandInput, string[] args)
    {
        var processCommand = false;
        foreach (var command in commands)
        {
            foreach (var commandWord in command.CommandWord)
            {
                if (commandInput.Equals(commandWord, System.StringComparison.OrdinalIgnoreCase))
                {
                    processCommand = true;
                    break;
                }
                if (processCommand)
                {
                    break;
                }
            }

            if (processCommand)
            {
                if (command.Process(args))
                {
                    return;
                }
            }
        }
    }
}
