using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestRequire
{
    public string description;      // �䱸����.

    [Header("Progress")]
    public string id;
    public int current;
    public int max;

    public bool isComplete => current >= max;

    // ������ �ʿ��ϸ� {c}��� ���δ�.
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