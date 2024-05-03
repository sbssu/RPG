using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestIcon : MonoBehaviour
{
    public string id;
    public Transform pivot;
    public Camera mainCam;
    public Image image;

    public void Setup(string id, Transform pivot, Sprite sprite)
    {
        this.id = id;
        this.pivot = pivot;
        mainCam = Camera.main;
        image = GetComponent<Image>();
        image.sprite = sprite;

        float ratio = sprite.rect.width / sprite.rect.height;
        RectTransform rect = image.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(rect.sizeDelta.x * ratio, rect.sizeDelta.y);
    }

    private void Update()
    {
        if (!mainCam.enabled)
        {
            if (image.enabled)
                image.enabled = false;
            return;
        }

        if (!image.enabled)
            image.enabled = true;

        transform.position = mainCam.WorldToScreenPoint(pivot.position);
    }
}
