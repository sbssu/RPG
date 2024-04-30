using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public static ProgressBar Instance {  get; private set; }

    [SerializeField] GameObject panel;
    [SerializeField] Image fillImage;
    [SerializeField] TMP_Text descripText;

    Action onCallback;

    private void Awake()
    {
        Instance = this;
        panel.SetActive(false);
    }

    public void StartProgress(string text, float time, Action onCallback)
    {
        this.onCallback = onCallback;

        descripText.text = text;
        fillImage.fillAmount = 0f;
        panel.SetActive(true);

        StartCoroutine(IEProgress(time));
    }
    private IEnumerator IEProgress(float maxTime)
    {
        float time = 0f;
        while (time < maxTime)
        {
            time = Mathf.Clamp(time + Time.deltaTime, 0f, maxTime);
            fillImage.fillAmount = time / maxTime;
            yield return null;
        }

        onCallback?.Invoke();
        onCallback = null;

        panel.SetActive(false);
    }
}
