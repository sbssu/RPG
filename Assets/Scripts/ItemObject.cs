using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    Sprite IconSprite { get; }
    string InterctText { get; }
    Transform InterectPivot { get; }
    bool CanInterect { get; }

    void OnInterect(GameObject owner);
}

public class ItemObject : MonoBehaviour, IInteract
{
    [SerializeField] Sprite iconSprite;
    [SerializeField] string interctText;

    public Sprite IconSprite => iconSprite;
    public string InterctText => interctText;
    public bool CanInterect => true;
    public Transform InterectPivot => null;

    public void OnInterect(GameObject owner)
    {
        Debug.Log("아이템을 먹었다!!");
        Destroy(gameObject);
    }
}
