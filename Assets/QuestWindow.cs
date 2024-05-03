using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestWindow : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text descriptText;

    [HideInInspector]
    public string id;

    public void Setup(string id, string title, string descript)
    {
        this.id = id;
        titleText.text = title;
        descriptText.text = descript;
    }
}
