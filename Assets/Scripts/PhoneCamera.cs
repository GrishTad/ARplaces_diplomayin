using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhoneCamera : MonoBehaviour {
	public Text cameraText;
	private bool camAvilable;
	private WebCamTexture backCam;
	private Texture defaultBackground;

	public RawImage background;
	public AspectRatioFitter fit;

	public void Start ()
	{
		defaultBackground = background.texture;
		WebCamDevice[] devices = WebCamTexture.devices;
		if (devices.Length == 0) 
		{
			Debug.Log ("No Camera detected");
			camAvilable = false;
			return;
		}
		for (int i = 0; i < devices.Length; i++) 
		{
			if (devices [i].isFrontFacing  && Application.platform == RuntimePlatform.WindowsEditor)
			{
				backCam = new WebCamTexture (devices[i].name,Screen.width, Screen.height, 30);
			}
			else if(!devices [i].isFrontFacing  && Application.platform != RuntimePlatform.WindowsEditor)
				backCam = new WebCamTexture (devices[i].name,Screen.width, Screen.height, 30);
			
		}

		if (backCam == null)
		{
			Debug.Log ("Unable to find back camera");
			return;
		}
		Application.RequestUserAuthorization (UserAuthorization.WebCam);
		backCam.Play ();
		background.texture = backCam;

		//float rat = (float)background.texture.height / (float)background.texture.width;
		//background.GetComponent<RectTransform> ().sizeDelta = new Vector2 (background.GetComponent<RectTransform> ().sizeDelta.x,background.GetComponent<RectTransform> ().sizeDelta.x * rat);
		//cameraText.text= "fps:"+backCam.requestedFPS.ToString()+" width:" + backCam.width.ToString() +" height:" + backCam.height.ToString()+" req width:" + backCam.requestedWidth.ToString() +" req height:" + backCam.requestedHeight.ToString();
		camAvilable = true;

		if (!camAvilable)
			return;

		float ratio = (float)backCam.width / (float)backCam.height;

		fit.aspectRatio = ratio;

		float scaleY=backCam.videoVerticallyMirrored ? -1f:1f;
		background.rectTransform.localScale = new Vector3 (1f, scaleY, 1f);

	}
	

	private void Update () 
	{
		
		int orient = -backCam.videoRotationAngle;
		background.rectTransform.localEulerAngles = new Vector3 (0, 0, orient);

		
	}
}
