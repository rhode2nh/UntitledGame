using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemStats 
{
    public int agility { get; set; }
    public int stamina { get; set; }
    public int strength { get; set; }
    public int intelligence { get; set; }

    public ItemStats()
    {
        agility = 0;
        stamina = 0;
        strength = 0;
        intelligence = 0;
    }
}
