using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimInterect : MonoBehaviour, IInteract
{
    [SerializeField] Sprite iconSprite;
    [SerializeField] string interectText;
    [SerializeField] Animator anim;
    [SerializeField] Transform interectPivot;

    bool isOpen;

    public Sprite IconSprite => iconSprite;
    public virtual string InterctText => interectText;
    public bool CanInterect => !isOpen;
    public Transform InterectPivot => interectPivot;

    public void OnInterect(GameObject owner)
    {
        isOpen = true;
        Player player = owner.GetComponent<Player>();

        StartCoroutine(IEOpenDoor(player));
    }
    private IEnumerator IEOpenDoor(Player player)
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
    }
}
