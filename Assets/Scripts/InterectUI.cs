using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InterectUI : MonoBehaviour
{
    public static InterectUI Instance { get; private set; }

    [SerializeField] GameObject panel;
    [SerializeField] Image iconImage;
    [SerializeField] TMP_Text descriptText;
    [SerializeField] TMP_Text hotkeyText;

    private void Awake()
    {
        Instance = this;
    }

    public void Show(IInteract target, Transform pivot)
    {
        iconImage.sprite = target.IconSprite;
        descriptText.text = target.InterctText;
        hotkeyText.text = "G";
        panel.SetActive(true);

        transform.position = Camera.main.WorldToScreenPoint(pivot.position);
    }
    public void Close()
    {
        panel.SetActive(false);
    }
}
