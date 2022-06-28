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
            Dictionary<string, object> propertiesToReturn = new Dictionary<string, object>();

            foreach (string key in properties.Keys)
            {
                switch (key)
                {
                    case Constants.P_W_MODIFIERS_LIST:
                        propertiesToReturn.Add(key, new List<Modifier>((List<Modifier>)properties[key]));
                        break;
                    default:
                        break;
                }
            }

            return propertiesToReturn;
        }
    }
}
