using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteract
{
    Sprite IconSprite { get; }
    string InterctText { get; }
    Transform InterectPivot { get; }
    bool CanInterect { get; }
    string InterectID { get; }

    void OnInterect(GameObject owner, Action callback);
}

public class ItemObject : MonoBehaviour, IInteract
{
    [SerializeField] Sprite iconSprite;
    [SerializeField] string interctText;

    public Sprite IconSprite => iconSprite;
    public string InterctText => interctText;
    public bool CanInterect => true;
    public Transform InterectPivot => null;
    public string InterectID => string.Empty;

    public void OnInterect(GameObject owner, Action callback)
    {
        Debug.Log("아이템을 먹었다!!");
        Destroy(gameObject);
        callback?.Invoke();
    }
}
