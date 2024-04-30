using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
    public static Fader Instance;

    [SerializeField] GameObject panel;
    [SerializeField] Image blindImage;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        panel.SetActive(false);
    }
    public void FadeIn(float fadeTime = 1.0f)
    {
        StartCoroutine(IEFadeIn(fadeTime));
    }
    public IEnumerator IEFadeIn(float fadeTime)
    {
        panel.SetActive(true);
        float time = fadeTime + 0.2f;
        while(time > 0f)
        {
            time = Mathf.Clamp(time - Time.deltaTime, 0f, fadeTime);
            Color color = blindImage.color;
            color.a = time / fadeTime;
            blindImage.color = color;
            yield return null;
        }
        panel.SetActive(false);
    }
}
