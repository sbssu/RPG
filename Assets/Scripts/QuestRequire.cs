using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestRequire
{
    public string description;      // 요구사항.

    [Header("Progress")]
    public string id;
    public int current;
    public int max;

    public bool isComplete => current >= max;

    // 개수가 필용하면 {c}라고 붙인다.
    public bool Action(string id, int count)
    {
        if (this.id != id)
            return false;

        current = count;
        return current >= max;
    }
    public override string ToString()
    {
        return description.Replace("{c}", max.ToString());
    }
}