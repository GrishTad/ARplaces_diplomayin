using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyboardtest : MonoBehaviour {

    private TouchScreenKeyboard keyboard;
    string text= "";
	// Use this for initialization
	public void Start () {
        keyboard = TouchScreenKeyboard.Open(text, TouchScreenKeyboardType.Default);
    }

    public  void GetKeyboardActivity()
    {
        if (keyboard != null)
        {
            if (keyboard.done)
            {
                text = keyboard.text;
                            }
            if (keyboard.wasCanceled)
            {
                text = keyboard.text;
            }
        }

    }
}
