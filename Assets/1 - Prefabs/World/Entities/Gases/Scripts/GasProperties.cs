using UnityEngine;

[System.Serializable]
public struct GasProps
{
    public float accelerationFactor;
    public float maxSpeed;
    public float minSpeed;
}

public class GasProperties : MonoBehaviour
{
    public GasProps gasProps;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals(Constants.PLAYER))
        {
            var equipmentContainer = other.gameObject.GetComponent<EquipmentContainer>();
            equipmentContainer.inGas = true;
            equipmentContainer.curGasProps = gasProps;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals(Constants.PLAYER))
        {
            var equipmentContainer = other.gameObject.GetComponent<EquipmentContainer>();
            equipmentContainer.inGas = false;
            equipmentContainer.curGasProps = new GasProps();
        }
    }
}
