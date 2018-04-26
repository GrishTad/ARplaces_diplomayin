using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {

    public static test instance;
    TextToSpeech tts;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        tts = GetComponent<TextToSpeech>();
    }
    public void Speak(string text)
    {
        tts.Speak(text, (string msg) =>
        {
            tts.ShowToast(msg);
        });
    }
    public void ChangeSpeed(float speed)
    {
        tts.SetSpeed(speed);
    }
    public void ChangeLanguage()
    {
        tts.SetLanguage(TextToSpeech.Locale.UK);
    }
    public void ChangePitch()
    {
        tts.SetPitch(0.5f);
    }
    public void speech(string test)
    {

        Speak(test);
    }
    private void OnApplicationQuit()
    {
        tts.Speed = 0;
        tts.enabled = false;
        Destroy(tts);
        
    }
}
