using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Npc : MonoBehaviour, IInteract
{
    [SerializeField] Sprite iconSprite;
    [SerializeField] string interctText;
    [SerializeField] Transform interectPivot;

    [SerializeField] string id;
    [SerializeField] string npcName;
    [SerializeField] string[] speechs;

    public Sprite IconSprite => iconSprite;
    public string InterctText => interctText;
    public bool CanInterect => true;
    public Transform InterectPivot => interectPivot;
    public string ID => id;
    public string InterectID => id;

    public void OnInterect(GameObject owner, Action _callback)
    {
        Player player = owner.GetComponent<Player>();
        player.LockControl(true);
        player.LockNavmesh(true);

        player.transform.position = transform.position + transform.forward * 7f;
        player.transform.rotation = transform.rotation;
        player.transform.Rotate(new Vector3(0f, 180f, 0f));
        player.SwitchTalkCamera(true);

        // 퀘스트 확인.
        QuestManager.Instance.CheckFromNPC(id, out string[] overrideSpeechs, out Action callback);
        Dialogue.Instance.OpenDialogue(overrideSpeechs ?? speechs, () =>
        {
            player.transform.position = transform.position + transform.forward * 2f;
            player.LockControl(false);
            player.LockNavmesh(false);
            player.SwitchTalkCamera(false);

            // 퀘스트를 진행하는 상태에서 끝나야 callback을 전달한다.
            if(overrideSpeechs != null)
                callback?.Invoke();

            _callback?.Invoke();
        }) ;
    }
}
