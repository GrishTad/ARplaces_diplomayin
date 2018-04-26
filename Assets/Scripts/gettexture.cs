using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gettexture : MonoBehaviour {

	public string url = getLocationJson.tex;
	IEnumerator Start()
	{
		// Start a download of the given URL
		WWW www = new WWW(url);

		// Wait for download to complete
		yield return www;

		// assign texture
		Renderer renderer = GetComponent<Renderer>();
		renderer.material.mainTexture = www.texture;
	}

}
