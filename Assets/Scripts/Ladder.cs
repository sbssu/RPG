using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInteract
{
    [SerializeField] Transform startPivot;
    [SerializeField] Transform endPivot;
    [SerializeField] Transform exitPivot;

    [SerializeField] Sprite iconSprite;
    [SerializeField] string interctText;

    bool isLadder;

    public Sprite IconSprite => iconSprite;
    public string InterctText => interctText;
    public bool CanInterect => !isLadder;
    public Transform InterectPivot => null;

    public void OnInterect(GameObject owner)
    {
        Debug.Log("캐릭터를 움직인다.");
        Player player = owner.GetComponent<Player>();
        StartCoroutine(IELadder(player));
    }
    private IEnumerator IELadder(Player player)
    {
        isLadder = true;
        player.LockControl(true);
        player.LockNavmesh(true);

        Transform target = player.transform;
        yield return StartCoroutine(IEMovement(target, startPivot, 10));       
        yield return StartCoroutine(IEMovement(target, endPivot, 4));       
        yield return StartCoroutine(IEMovement(target, exitPivot, 1.5f));

        player.LockControl(false);
        player.LockNavmesh(false);
        isLadder = false;
    }

    private IEnumerator IEMovement(Transform target, Transform destination, float speed)
    {
        while (target.position != destination.position)
        {
            target.position = Vector3.MoveTowards(target.position, destination.position, speed * Time.deltaTime);
            yield return null;
        }
    }
}
