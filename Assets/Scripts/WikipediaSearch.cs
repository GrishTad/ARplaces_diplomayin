using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System;

public class WikipediaSearch : MonoBehaviour {

    [System.Serializable]
    public class wikiSearchJsonData
    {
        public query query;

    }
    [System.Serializable]
    public class query 
    {
        public List<search> search = new List<search>();
        public pages pages;
    }
    [System.Serializable]
    public class search
    {
        public string pageid;
    }
    [System.Serializable]
    public class pages
    {
        public content content;
    }
    [System.Serializable]
    public class content
    {
        public string extract="";
        public string title;
    }
    public static WikipediaSearch instance;
    public Text wikiText;
    public Text container;
    public Button[] infoAndGid;
    public RawImage touch;

    public string speechText;
    private void Awake()
    {
        instance = this;
    }
    public void searchinfo(string serachText)
    {
        
        StartCoroutine(searchInWikipedia(serachText));
    }
    public IEnumerator searchInWikipedia(string searchByName)
    {
        searchByName = searchByName.Replace(" ", "%20");
        WWW searchUrl = new WWW("https://en.wikipedia.org/w/api.php?action=query&list=search&srsearch="+searchByName+"&srlimit=1&formatversion=2&utf8&format=json");
        yield return searchUrl;
        Debug.Log(searchUrl.text);
        wikiSearchJsonData wikiSearchData = JsonUtility.FromJson<wikiSearchJsonData>(searchUrl.text);
        Debug.Log(wikiSearchData.query.search.Count);
        if (wikiSearchData.query.search.Count > 0)
        {
            foreach (Button item in infoAndGid)
            {
                item.interactable = true;
            }
            Debug.Log(wikiSearchData.query.search[0].pageid);
            StartCoroutine(getInfoFromWiki(wikiSearchData.query.search[0].pageid));
        }
        else
        {
            foreach (Button item in infoAndGid)
            {
                item.interactable = false;
                touch.enabled = false;
            }
       }

    }

    public IEnumerator getInfoFromWiki(string pageid)
    {
        WWW getDataUrl = new WWW("https://en.wikipedia.org/w/api.php?action=query&format=json&prop=extracts&exintro&&exlimit=max&explaintext&utf8&pageids=" + pageid);
        yield return getDataUrl;
        string data = getDataUrl.text;
        data = data.Replace(pageid, "content");
        string removeText = '"' + "pageid" + '"' + ":content,";
        data = data.Replace(removeText,"");
        Debug.Log(data);
        wikiSearchJsonData wikiData= JsonUtility.FromJson<wikiSearchJsonData>(data);
        Debug.Log(wikiData.query.pages.content.extract);
        //string speechText = TruncateLongString(wikiData.query.pages.content.extract, 300);
        wikiText.text = wikiData.query.pages.content.extract;
        container.text = wikiData.query.pages.content.extract;
        speechText= wikiData.query.pages.content.extract;
        //test.instance.Speak(wikiData.query.pages.content.extract);

        //ExampleTextToSpeech.instance._testString = wikiData.query.pages.content.extract;

        // ExampleTextToSpeech.instance.StartSpeach();

    }

    public string TruncateLongString(string str, int maxLength)
    {
        return str.Substring(0, Math.Min(str.Length, maxLength));
    }
    //IEnumerator Start()
    //{
    //    string words = "Hello";
    //    // Remove the "spaces" in excess
    //    Regex rgx = new Regex("\\s+");
    //    // Replace the "spaces" with "% 20" for the link Can be interpreted
    //    string result = rgx.Replace(words, "%20");
    //    string url = "http://translate.google.com/translate_tts?tl=en&q=" + result;
    //    WWW www = new WWW(url);
    //    yield return www;
    //    AudioSource audio = gameObject.AddComponent<AudioSource>();
    //    audio.clip = www.GetAudioClip(false, false, AudioType.MPEG);
    //    audio.Play();

    //}
}
//https://en.wikipedia.org/w/api.php?action=query&format=json&prop=extracts&exlimit=max&explaintext&pageids=1008616&utf8
