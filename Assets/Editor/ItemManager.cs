using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class ItemManager : Editor
{
    const string ITEM_URL = "https://docs.google.com/spreadsheets/d/1KJznVaj9WRzva93FxKrfzj9bZvGVdT5hdoQ0NijFjKU/export?format=tsv&range=A2:C";

    [MenuItem("Database/Update Item &i")]
    public static void UpdateItem()
    {
        DownloadItem();
    }
    private static async void DownloadItem()
    {
        UnityWebRequest web = UnityWebRequest.Get(ITEM_URL);
        UnityWebRequestAsyncOperation op = web.SendWebRequest();
        while (!op.isDone)
            await Task.Yield();

        string tsv = web.downloadHandler.text;
        string[] lines = tsv.Trim().Split('\n');
        var itemDatas = lines.Select(line => line.Split('\t')).Select(datas => new {
            ID = datas[0],
            Name = datas[1],
            SpritePath = datas[2] 
        }).ToArray();

        List<Item> itemList = new List<Item>();

        foreach( var itemData in itemDatas )
        {
            string path = $"Scriptable/Item/{itemData.ID}";
            Item item = Resources.Load<Item>(path);
            if (item == null)
            {
                item = CreateInstance<Item>();
                AssetDatabase.CreateAsset(item, $"Assets/Resources/{path}.asset");
            }
            item.id = itemData.ID;
            item.itemName = itemData.Name;
            item.iconSprite = Resources.Load<Sprite>(itemData.SpritePath.Trim());
            itemList.Add(item);
            EditorUtility.SetDirty(item);
        }

        Item selectItem = itemList.OrderBy(i => i.id).FirstOrDefault();
        EditorGUIUtility.PingObject(selectItem);
        Selection.activeObject = selectItem;
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}
