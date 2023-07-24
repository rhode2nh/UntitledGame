[System.Serializable]
public struct TestStats
{
    public int agility, strength, jumpHeight;
    public TestStats(int agility, int strength, int jumpHeight)
    {
        this.agility = agility;
        this.strength = strength;
        this.jumpHeight = jumpHeight;
    }

    public TestStats(TestStats testStats)
    {
        this.agility = testStats.agility;
        this.strength = testStats.strength;
        this.jumpHeight = testStats.jumpHeight;
    }

    public static TestStats operator+ (TestStats a, TestStats b)
    {
        return new TestStats(a.agility + b.agility, a.strength + b.strength, a.jumpHeight + b.jumpHeight);
    }

    public static bool operator>= (TestStats a, TestStats b)
    {
        if (a.agility >= b.agility && a.strength >= b.strength && a.jumpHeight >= b.jumpHeight)
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
        if (a.agility <= b.agility || a.strength <= b.strength || a.jumpHeight < b.jumpHeight)
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
        if (a.agility > b.agility && a.strength > b.strength && a.jumpHeight > b.jumpHeight)
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
        if (a.agility < b.agility || a.strength < b.strength || a.jumpHeight < b.jumpHeight)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
