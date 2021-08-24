using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Mech Base", menuName = "Mech/Base")]
public class MechBase : ScriptableObject
{
    public static string baseName = "";

    public static int headSlots = 1;

    public static int torsoSlots = 1;

    public static int leftArmSlots = 1;
    public static int rightArmSlots = 1;

    public static int leftShoulderSlots = 1;
    public static int rightShoulderSlots = 1;

    public static int leftLegSlots = 1;
    public static int rightLegSlots = 1;
}
