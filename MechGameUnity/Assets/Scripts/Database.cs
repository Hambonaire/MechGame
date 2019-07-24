using System;

public class Database : MonoBehaviour
{
    // Make Singleton instance

	public List<Item> database = new List<Item>();

    public Item GetActual(string byID)
    {
        if (string.IsNullOrEmpty(byID))
        {
            return null;
        }

        for (int i  = 0; i < database.Length; i++)
        {
            if (database[i].itemID == byID)
            {
                return database[i];
            }
        }

        Debug.Log("Could not find an Item with ID: " + byID);
        return null;
    }

    public Item GetClone(string byID)
    {
        if (string.IsNullOrEmpty(byID))
        {
            return null;
        }

        for (int i = 0; i < database.Length; i++)
        {
            if (database[i].itemID == byID)
            {
                var clone = Instantiate(database[i]);
                return clone;
            }
        }

        return null;
    }
}
