using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    [SerializeField] Sprite sprite;

    [ContextMenu("TT")]
    public void Load()
    {
        sprite = Resources.Load<Sprite>("Items/Weapons/No_bg/02_Sword_nobg");
    }
}
