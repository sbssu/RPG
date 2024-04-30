using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HoverObject : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] Material defaultMat;
    [SerializeField] Material outlineMat;
    [SerializeField] UnityEvent<GameObject> onClickObject;

    public void OnEnterHover()
    {
        if(meshRenderer != null)
            meshRenderer.material = outlineMat;

        if(skinnedMeshRenderer != null)
            skinnedMeshRenderer.material = outlineMat;
    }
    public void OnExitHover()
    {
        if (meshRenderer != null)
            meshRenderer.material = defaultMat;

        if (skinnedMeshRenderer != null)
            skinnedMeshRenderer.material = defaultMat;
    }
    public void OnClickObject(GameObject owner)
    {
        onClickObject?.Invoke(owner);
    }
}
