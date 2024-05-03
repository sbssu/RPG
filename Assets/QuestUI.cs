using System.Collections.Generic;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    public enum ICON
    {
        READY,
        PROGRESS,
        COMPLETE,
    }

    public static QuestUI Instance;

    [SerializeField] QuestIcon iconPrefab;
    [SerializeField] QuestWindow windowPrefab;
    [SerializeField] RectTransform iconParent;
    [SerializeField] RectTransform windowParent;
    [SerializeField] Sprite[] progressSprite;

    Dictionary<string, QuestIcon> iconTable;
    Dictionary<string, QuestWindow> windowTable;

    private void Awake()
    {
        Instance = this;
        iconPrefab.gameObject.SetActive(false);
        windowPrefab.gameObject.SetActive(false);   
        iconTable = new Dictionary<string, QuestIcon>();
        windowTable = new Dictionary<string, QuestWindow>();
    }

    public void CreateIcon(string id, Transform pivot, ICON progress)
    {
        if (!iconTable.ContainsKey(id))
            iconTable.Add(id, Instantiate(iconPrefab, iconParent));

        QuestIcon image = iconTable[id];
        image.Setup(id, pivot, progress switch
        {
            ICON.READY => progressSprite[0],
            ICON.PROGRESS => progressSprite[1],
            ICON.COMPLETE => progressSprite[2],
            _ => null
        });
        image.gameObject.SetActive(true);
    }
    public void DeleteIcon(string id)
    {
        if(iconTable.ContainsKey(id))
        {
            Destroy(iconTable[id].gameObject);
            iconTable.Remove(id);
        }    
    }

    public void CreateWindow(string id, string title, string descript)
    {
        if (!windowTable.ContainsKey(id))
            windowTable.Add(id, Instantiate(windowPrefab, windowParent));

        windowTable[id].Setup(id, title, descript);
        windowTable[id].gameObject.SetActive(true);
    }
    public void DeleteWindow(string id)
    {
        if(windowTable.ContainsKey(id))
        {
            Destroy(windowTable[id].gameObject);
            windowTable[id] = null;
        }
    }
}
