using System.Collections.Generic;

namespace ExtensionMethods
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Creates a copy of an item's properties
        /// </summary
        /// <param name="properties">The properties to copy.</param>
        /// <returns>The copied properties.</returns>
        public static Dictionary<string, object> CopyProperties(this Dictionary<string, object> properties)
        {
            if (properties == null)
            {
                return new Dictionary<string, object>();
            }
            Dictionary<string, object> propertiesToReturn = new Dictionary<string, object>();

            foreach (string key in properties.Keys)
            {
                switch (key)
                {
                    case Constants.P_W_MODIFIERS_LIST:
                        propertiesToReturn.Add(key, new List<Modifier>((List<Modifier>)properties[key]));
                        break;
                    case Constants.P_W_MODIFIER_SLOT_INDICES_LIST:
                        propertiesToReturn.Add(key, new List<int>((List<int>)properties[key]));
                        break;
                    case Constants.P_W_MAX_SLOTS_INT:
                        propertiesToReturn.Add(key, properties[key]);
                        break;
                    default:
                        break;
                }
            }

            return propertiesToReturn;
        }
    }
}
