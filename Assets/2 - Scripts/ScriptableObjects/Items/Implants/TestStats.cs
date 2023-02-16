[System.Serializable]
public struct TestStats
{
    public int agility, strength;
    public TestStats(int agility, int strength)
    {
        this.agility = agility;
        this.strength = strength;
    }

    public TestStats(TestStats testStats)
    {
        this.agility = testStats.agility;
        this.strength = testStats.strength;
    }

    public static TestStats operator+ (TestStats a, TestStats b)
    {
        return new TestStats(a.agility + b.agility, a.strength + b.strength);
    }

    public static bool operator>= (TestStats a, TestStats b)
    {
        if (a.agility >= b.agility && a.strength >= b.strength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator<= (TestStats a, TestStats b)
    {
        if (a.agility <= b.agility || a.strength <= b.strength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public static bool operator> (TestStats a, TestStats b)
    {
        if (a.agility > b.agility && a.strength > b.strength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static bool operator< (TestStats a, TestStats b)
    {
        if (a.agility < b.agility || a.strength < b.strength)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
