using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsoleCommand : ScriptableObject
{
    [SerializeField] private List<string> commandWord = new List<string>();

    public List<string> CommandWord => commandWord;

    public abstract bool Process(string[] args);
}
