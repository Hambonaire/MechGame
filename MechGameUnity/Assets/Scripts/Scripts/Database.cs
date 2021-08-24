using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Database : MonoBehaviour
{
    // Make Singleton instance (?)

	public List<Item> database = new List<Item>();

    public Item GetActual(int byID)
    {
        for (int i  = 0; i < database.Count; i++)
        {
            if (database[i].idNum == byID)
            {
                return database[i];
            }
        }

        Debug.Log("Could not find an Item with ID: " + byID);
        return null;
    }

    public Item GetClone(int byID)
    {
        for (int i = 0; i < database.Count; i++)
        {
            if (database[i].idNum == byID)
            {
                var clone = Instantiate(database[i]);
                return clone;
            }
        }

        return null;
    }
}
