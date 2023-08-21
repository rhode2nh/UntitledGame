using System.Collections.Generic;
using UnityEngine;

public class TriggerList : MonoBehaviour
{
    public List<Output> triggerList;
    public bool spawnedFromTrigger = false;
    private Vector3 camDir;
    public float xSpread;
    public float ySpread;

    // Start is called before the first frame update
    void Awake()
    {
        triggerList = new List<Output>();
    }

    public void CalculateTriggerChildren() 
    {
        List<List<Output>> firstPass = CalculateFirstPass();
        //PrintOutput(firstPass);
        List<List<Output>> secondPass = CalculateSecondPass(firstPass);
        //PrintOutput(secondPass);
        // TODO: Evaluate the trigger list, then instantiate projectiles...I should reuse the algorithm I already created.
        if (secondPass.Count != 0)
        {
            for (int i = 0; i < secondPass[0].Count; i++)
            {
                float x = Random.Range(-xSpread * 0.5f, xSpread * 0.5f);
                float y = Random.Range(-ySpread * 0.5f, ySpread * 0.5f);
                var projectile = secondPass[0][i].slot.item as IProjectile;
                if (projectile == null)
                {
                    continue;
                }
                var instantiatedProjectile = Instantiate(projectile.ProjectilePrefab, transform.position, transform.rotation);
                var redirect = instantiatedProjectile.GetComponent<RaycastProjectile>();
                if (redirect != null)
                {
                    redirect.shouldRedirect = false;
                }
                var triggers = instantiatedProjectile.GetComponent<TriggerList>();
                if (triggers != null)
                {
                    triggers.spawnedFromTrigger = true;
                    triggers.triggerList = secondPass[0][i].postModifiers;
                }
                instantiatedProjectile.transform.rotation = Quaternion.AngleAxis(x, transform.up) * instantiatedProjectile.transform.rotation;
            }
        }
    }
    
    private List<List<Output>> CalculateFirstPass()
    {
        List<List<Output>> firstPass = new List<List<Output>>();
        List<Output> currentGroup = new List<Output>();
        List<int> potentialWrapModifiers = new List<int>();
        int projectilesToGroup = 1;

        for (int i = 0; i < triggerList.Count; i++)
        {
            var curSlot = triggerList[i].slot;
            var curModifier = curSlot.item as Modifier;
            if (curModifier is IProjectile)
            {
                if (curModifier is ITrigger)
                {
                    currentGroup.Add(new Output(triggerList[i].slot));
                    potentialWrapModifiers.Add(i);
                }
                else
                {
                    currentGroup.Add(new Output(curSlot));
                    projectilesToGroup--;
                }
            }

            else if (curModifier is ICastX)
            {
                var castX = curModifier as ICastX;
                projectilesToGroup += castX.ModifiersPerCast;
                currentGroup.Add(new Output(curSlot));
                potentialWrapModifiers.Add(i);
                projectilesToGroup--;
            }
            
            // TODO: Figure out what to do with the rest of modifiers
            else
            {
                firstPass.Add(new List<Output>());
            }

            if (i == triggerList.Count - 1)
            {
                firstPass.Add(new List<Output>(currentGroup));
                break;
            }

            if (projectilesToGroup == 0)
            {
                firstPass.Add(new List<Output>(currentGroup));
                currentGroup = new List<Output>();
                projectilesToGroup = 1;
            }
        }

        return firstPass;
    }

    private List<List<Output>> CalculateSecondPass(List<List<Output>> firstPass)
    {
        List<List<Output>> secondPass = new List<List<Output>>();

        for (int i = 0; i < firstPass.Count; i++)
        {
            secondPass.Add(new List<Output>());
            int postProjectilesToGroup = 0;
            int triggerIndex = 0;
            bool foundTrigger = false;
            for (int j = 0; j < firstPass[i].Count; j++)
            {
                var curSlot = firstPass[i][j].slot;
                var curProjectile = curSlot.item as Modifier;
                // First occurence of a trigger
                if (curProjectile is ITrigger)
                {
                    if (!foundTrigger)
                    {
                        foundTrigger = true;
                        secondPass[i].Add(firstPass[i][j]);
                        triggerIndex = secondPass[i].Count - 1;
                        postProjectilesToGroup++;
                        continue;
                    }
                    else
                    {
                        secondPass[i][triggerIndex].postModifiers.Add(firstPass[i][j]);
                    }
                }
                else if (curProjectile is ICastX)
                {
                    if (foundTrigger)
                    {
                        var castX = curProjectile as ICastX;
                        // TODO: Not sure if this is needed
                        secondPass[i][triggerIndex].postModifiers.Add(firstPass[i][j]);
                        postProjectilesToGroup += castX.ModifiersPerCast;
                        postProjectilesToGroup--;
                    }
                    else
                    {
                        secondPass[i].Add(firstPass[i][j]);
                    }
                }
                else
                {
                    if (foundTrigger)
                    {
                        secondPass[i][triggerIndex].postModifiers.Add(firstPass[i][j]);
                        postProjectilesToGroup--;
                    }
                    else
                    {
                        secondPass[i].Add(firstPass[i][j]);
                    }
                }
                if (foundTrigger && postProjectilesToGroup == 0)
                {
                    foundTrigger = false;
                }
            }
        }

        return secondPass;
    }

    private void PrintOutput(List<List<Output>> outputList)
    {
        var debugString = "";
        for (int i = 0; i < outputList.Count; i++)
        {
            debugString += "Group " + i + ":\n";
            for (int j = 0; j < outputList[i].Count; j++)
            {
                debugString += "   - " + outputList[i][j].slot.item.name + "\n";
                if (outputList[i][j].slot is ITrigger)
                {
                    var postProjectiles = outputList[i][j].postModifiers;
                    for (int k = 0; k < postProjectiles.Count; k++)
                    {
                        debugString += "      * " + postProjectiles[k].slot.item.name + "\n";
                    }
                }
            }
        }
        Debug.Log(debugString);
    }
}
