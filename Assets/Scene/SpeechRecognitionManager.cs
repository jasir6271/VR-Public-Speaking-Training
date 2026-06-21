using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class SpeechRecognitionManager : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI recognizedText;
    public TextMeshProUGUI wpmText;
    public TextMeshProUGUI fillerText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI loudnessText;
    public TextMeshProUGUI pitchText;
    public TextMeshProUGUI eyeText;

    [Header("Camera")]
    public Camera playerCamera;

    [Header("Audience Reaction")]
    public AudienceReaction specialAudience;
    public TalkingReaction student1;
    public TalkingReaction student2;
    public AudienceGreeting audienceGreeting;

    [Header("Clap Audience")]
    public AudienceClap[] audienceMembers;

    private DictationRecognizer dictation;

    private AudioClip micClip;
    private string micName;
    private const int sampleWindow = 1024;

    private int fillerCount = 0;
    private int totalWords = 0;

    private float startTime;
    private float maxTime = 60f;
    private bool started = false;

    private float eyeTime = 0f;
    private float currentPitch = 0f;

    private List<string> fillers = new List<string>()
    {
        "um","uh","like","actually","basically","you know"
    };

    void Start()
    {
        StartCoroutine(InitializeSystem());
    }

    IEnumerator InitializeSystem()
    {
        yield return new WaitForSeconds(1.5f);

        if (Microphone.devices.Length == 0)
        {
            Debug.LogError("No microphone found!");
            yield break;
        }

        micName = Microphone.devices[0];
        micClip = Microphone.Start(micName, true, 10, 44100);

        yield return new WaitForSeconds(0.5f);

        if (audienceGreeting == null)
            audienceGreeting = FindObjectOfType<AudienceGreeting>();

        if (specialAudience == null)
            specialAudience = FindObjectOfType<AudienceReaction>();

        if (student1 == null || student2 == null)
        {
            var students = FindObjectsOfType<TalkingReaction>();
            if (students.Length >= 2)
            {
                student1 = students[0];
                student2 = students[1];
            }
        }

        if (audienceMembers == null || audienceMembers.Length == 0)
            audienceMembers = FindObjectsOfType<AudienceClap>();

        dictation = new DictationRecognizer();

        dictation.DictationHypothesis += (text) =>
        {
            if (recognizedText != null)
                recognizedText.text = "Listening: " + text;
        };

        dictation.DictationResult += (text, confidence) =>
        {
            if (recognizedText != null)
                recognizedText.text = "Recognized: " + text;

            ProcessSpeech(text);
            DetectGreetings(text);
            DetectClapWords(text);
        };

        dictation.Start();
    }

    void Update()
    {
        if (!started) return;

        float time = Time.time - startTime;

        if (timerText != null)
            timerText.text = "Time: " + (int)time;

        if (time >= maxTime)
        {
            EndSession();
            return;
        }

        float wpm = (totalWords / time) * 60f;
        if (wpmText != null)
            wpmText.text = "WPM: " + wpm.ToString("F0");

        float loud = GetLoudness();
        if (loudnessText != null)
            loudnessText.text = "Loudness: " + loud.ToString("F3");

        float rawPitch = EstimatePitchRaw();
        float clampedPitch = Mathf.Clamp(rawPitch, 80f, 300f);
        currentPitch = Mathf.Lerp(currentPitch, clampedPitch, 0.1f);

        if (pitchText != null)
            pitchText.text = "Pitch: " + currentPitch.ToString("F0") + " Hz";

        UpdateEyeContact();

        if (eyeText != null)
            eyeText.text = "Eye Contact: " + SessionData.EyeContact.ToString("F0") + "%";
    }

    void ProcessSpeech(string text)
    {
        if (!started)
        {
            started = true;
            startTime = Time.time;
        }

        string[] words = text.ToLower().Split(
            new char[] { ' ', '.', ',', '!', '?' },
            System.StringSplitOptions.RemoveEmptyEntries);

        totalWords += words.Length;

        DetectFillers(words);
    }

    void DetectFillers(string[] words)
    {
        foreach (string word in words)
        {
            if (fillers.Contains(word))
            {
                fillerCount++;

                specialAudience?.PlayBored();
                student1?.StartTalking();
                student2?.StartTalking();

                if (fillerText != null)
                    fillerText.text = "Filler Words: " + fillerCount;
            }
        }
    }

    void DetectGreetings(string text)
    {
        if (audienceGreeting == null) return;

        string lower = text.ToLower();

        if (lower.Contains("good") && lower.Contains("morning"))
            audienceGreeting.ReplyGoodMorning();

        if (lower.Contains("hello"))
            audienceGreeting.ReplyHello();

        if (lower.Contains("hi"))
            audienceGreeting.ReplyHi();

        if (lower.Contains("thank"))
            audienceGreeting.ReplyThankYou();

        if (lower.Contains("how") && lower.Contains("you"))
            audienceGreeting.ReplyHowAreYou();

        if (lower.Contains("name"))
            audienceGreeting.ReplyName();
    }

    void DetectClapWords(string text)
    {
        string lower = text.ToLower();

        if (lower.Contains("thank") || lower.Contains("congratulations"))
        {
            if (audienceMembers != null)
            {
                foreach (var member in audienceMembers)
                    member?.PlayClap();
            }

            audienceGreeting?.PlayClap();
        }
    }

    void UpdateEyeContact()
    {
        if (playerCamera == null) return;

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Audience"))
                eyeTime += Time.deltaTime;
        }

        SessionData.EyeContact = (eyeTime / maxTime) * 100f;
    }

    float GetLoudness()
    {
        if (micClip == null) return 0;

        int micPos = Microphone.GetPosition(micName) - sampleWindow;
        if (micPos < 0) return 0;

        float[] data = new float[sampleWindow];
        micClip.GetData(data, micPos);

        float sum = 0;
        foreach (float s in data)
            sum += Mathf.Abs(s);

        return sum / sampleWindow;
    }

    float EstimatePitchRaw()
    {
        int micPos = Microphone.GetPosition(micName) - sampleWindow;
        if (micPos < 0) return 0;

        float[] data = new float[sampleWindow];
        micClip.GetData(data, micPos);

        int crossings = 0;

        for (int i = 1; i < sampleWindow; i++)
        {
            if ((data[i - 1] > 0 && data[i] < 0) ||
                (data[i - 1] < 0 && data[i] > 0))
                crossings++;
        }

        return crossings * (44100f / sampleWindow) / 2f;
    }

    // 🔥 MAIN FIX HERE
    void EndSession()
    {
        float totalTime = Time.time - startTime;

        SessionData.WPM = (totalWords / totalTime) * 60f;

        SessionData.Loudness = Mathf.Clamp(GetLoudness() * 50f, 0f, 1f);

        SessionData.Pitch = currentPitch;

        SessionData.FillerCount = fillerCount;

        SceneManager.LoadScene("ScoreScene");
    }

    void OnDestroy()
    {
        if (dictation != null)
        {
            dictation.Stop();
            dictation.Dispose();
        }

        if (!string.IsNullOrEmpty(micName))
            Microphone.End(micName);
    }
}