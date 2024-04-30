using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc : MonoBehaviour, IInteract
{
    [SerializeField] Sprite iconSprite;
    [SerializeField] string interctText;

    [SerializeField] string id;
    [SerializeField] string npcName;
    [SerializeField] string[] speechs;

    public Sprite IconSprite => iconSprite;
    public string InterctText => interctText;
    public bool CanInterect => true;
    public Transform InterectPivot => null;

    public void OnInterect(GameObject owner)
    {
        Player player = owner.GetComponent<Player>();
        player.LockControl(true);
        player.LockNavmesh(true);

        player.transform.position = transform.position + transform.forward * 7f;
        player.transform.rotation = transform.rotation;
        player.transform.Rotate(new Vector3(0f, 180f, 0f));
        player.SwitchTalkCamera(true);

        Dialogue.Instance.OpenDialogue(speechs, () => {
            player.transform.position = transform.position + transform.forward * 2f;
            player.LockControl(false);
            player.LockNavmesh(false);
            player.SwitchTalkCamera(false);
        });
    }
}
