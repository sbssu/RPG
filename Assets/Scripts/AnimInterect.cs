using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class AnimInterect : MonoBehaviour, IInteract
{
    public class SaveData
    {
        public bool isOpen;
    }

    [SerializeField] string interectID;
    [SerializeField] string interectText;
    [SerializeField] Sprite iconSprite;
    [SerializeField] Animator anim;
    [SerializeField] Transform interectPivot;

    bool isOpen;

    public Sprite IconSprite => iconSprite;
    public virtual string InterctText => interectText;
    public bool CanInterect => !isOpen;
    public Transform InterectPivot => interectPivot;
    public string InterectID => interectID;

    void Start()
    {
        LoadData();
    }

    public void OnInterect(GameObject owner, System.Action callback)
    {
        isOpen = true;
        Player player = owner.GetComponent<Player>();

        StartCoroutine(IEOpenDoor(player, callback));
    }
    private IEnumerator IEOpenDoor(Player player, System.Action callback)
    {
        player.LockControl(true);

        bool isProgress = true;
        ProgressBar.Instance.StartProgress("열쇠가 쇳소리를 내며 돌아간다.", 3f, () => {
            isProgress = false;
        });

        while(isProgress)
            yield return null;

        player.LockControl(false);
        anim.SetTrigger("onOpen");

        callback?.Invoke();
        callback = null;
    }

    private void LoadData()
    {
        string json = PlayerPrefs.GetString("DOOR_DATA", string.Empty);
        if (string.IsNullOrEmpty(json))
            return;

        SaveData data = JsonUtility.FromJson<SaveData>(json);
        isOpen = data.isOpen;
        if (isOpen)
            anim.SetTrigger("onOpen");
    }
    public void OnApplicationQuit()
    {
        SaveData saveData = new SaveData();
        saveData.isOpen = isOpen;
        PlayerPrefs.SetString("DOOR_DATA", JsonUtility.ToJson(saveData));
    }
}
