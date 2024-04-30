using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Scriptable/Item")]
public class Item : ScriptableObject
{
    public string id;
    public string itemName;
    public Sprite iconSprite;
}
