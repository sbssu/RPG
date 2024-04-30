using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public static Dialogue Instance { get; private set; }

    [SerializeField] GameObject panel;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text descriptionText;
    [SerializeField] TMP_Text questText;
    [SerializeField] float textSpeed;
    [SerializeField] AudioClip[] clips;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        panel.SetActive(false);
    }

    public void OpenDialogue(string[] dialogues, System.Action onEnd)
    {
        panel.SetActive(true);
        StartCoroutine(IEDialogue(dialogues, onEnd));
    }

    private IEnumerator IEDialogue(string[] dialogues, System.Action onEnd)
    {
        foreach (var dialogue in dialogues)
        {
            yield return StartCoroutine(Texting(dialogue));
            yield return StartCoroutine(IEInput());
        }

        onEnd?.Invoke();
        onEnd = null;

        panel.SetActive(false);
    }
    private IEnumerator Texting(string dialogue)
    {
        int count = 0;
        StringBuilder sb = new StringBuilder();
        foreach(char c in dialogue)
        {
            sb.Append(c);
            descriptionText.text = sb.ToString();

            if(count == 0)
                new GameObject("SFX").AddComponent<SFX>().Play(clips[Random.Range(0, clips.Length - 1)]);
            count = (int)Mathf.Repeat(count + 1, 2);
            yield return new WaitForSeconds(textSpeed);
        }
    }
    private IEnumerator IEInput()
    {
        while(true)
        {
            if(Input.GetKeyDown(KeyCode.G) || Input.GetMouseButtonDown(0))
                break;

            yield return null;
        }
    }
}


public class SFX : MonoBehaviour 
{
    AudioSource source;
    public void Play(AudioClip clip)
    {
        source = gameObject.AddComponent<AudioSource>();
        source.volume = 0.7f;
        source.clip = clip;
        source.Play();
    }
    private void Update()
    {
        if(!source.isPlaying)
            Destroy(gameObject);
    }
}
