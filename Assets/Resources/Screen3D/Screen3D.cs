/*
 * Copyright (c) 2014. InS3D.com. All rights reserved.
 *
 * The contents of this file are subject to the license terms.
 * You may not use, reproduce, distribute or modify this file
 * except in compliance with the License.
 *
 * PROPRIETARY/CONFIDENTIAL. DO NOT ALTER OR REMOVE THIS HEADER.
 */

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Screen3D : MonoBehaviour {

	public Camera guiCamera;
	public Camera leftEye;
	public Camera rightEye;
	public Transform lookPoint;								// Object to look at (typically player object)

	[Range(0,5)] public float eyeWidth = 0.5f;				// Distance between left and right eye
	[Range(-100,100)] public float focusDistance = 0.0f;	// Distance between focusPoint and lookPoint

	public bool noGUI = false;
	public bool interlaced = false;
	private bool hideGUI = false;

	private GameObject stereoCamera = null;
	private GameObject s3dGui = null;

	void Awake(){
		// Initialize GUI
		if ( !noGUI ) {
			Instantiate(Resources.Load("Screen3D/S3D Canvas"));
			s3dGui = GameObject.Find("S3D GUI");
		}

		// Set 3D vertical mode on 3D tablet
		if ( GetComponent<S3D>() == null ){
			gameObject.AddComponent<S3D>();
			GetComponent<S3D>().Stereo3D(interlaced);
		}

		// Check for left eye
		if ( leftEye == null ) {
			leftEye = GetComponent<Camera>();
			gameObject.name = "Left Eye";
			if ( leftEye == null ) {
				Debug.Log("Left Eye camera MUST exist!!!");
				return;
			}
		}
		// Check for right eye
		if ( rightEye == null ){
			GameObject o = new GameObject("Right Eye");
			rightEye = o.AddComponent<Camera>();
			rightEye.nearClipPlane = leftEye.nearClipPlane;
			rightEye.farClipPlane = leftEye.farClipPlane;
			rightEye.renderingPath = leftEye.renderingPath;
			rightEye.depth = leftEye.depth;
			rightEye.fieldOfView = leftEye.fieldOfView;	// Added -> Eric Santiago
			rightEye.clearFlags = leftEye.clearFlags;
			rightEye.backgroundColor = leftEye.backgroundColor;
			o.transform.position = leftEye.transform.position;
			o.transform.rotation = leftEye.transform.rotation;
			o.transform.parent = leftEye.transform;
		}

		// Face should be looking at point
        transform.LookAt(lookPoint);

		// Adjust eyeWidth
		leftEye.transform.position = transform.position + (eyeWidth/2.0f)*-transform.right;
		rightEye.transform.position = transform.position + (eyeWidth/2.0f)*transform.right;

		// Check for NoDraw layer
		if ( LayerMask.NameToLayer("NoDraw") < 0 || LayerMask.NameToLayer("NoDraw") > 31 ) {
			Debug.Log ("NoDraw layer DOES NOT exist!!!");
			return;
		}

		// Set interlace
		if ( !interlaced ){
			leftEye.rect = new Rect(0,0,0.5f,1f);
			rightEye.rect = new Rect(0.5f,0,0.5f,1f);
			leftEye.projectionMatrix *= Matrix4x4.Scale( new Vector3(0.5f,1f,1f) );
			rightEye.projectionMatrix *= Matrix4x4.Scale( new Vector3(0.5f,1f,1f) );
		} else {
			Instantiate(Resources.Load("Screen3D/Stereo"));

			stereoCamera = GameObject.FindWithTag("SteroeCamera");

			stereoCamera.layer = LayerMask.NameToLayer("NoDraw"); 
			stereoCamera.GetComponent<CameraMain>().SetCameras(leftEye,rightEye);
			stereoCamera.GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("NoDraw");
		}
		
		// Remove NoDraw layer from left and right eye
		leftEye.cullingMask = ~(1 << LayerMask.NameToLayer("NoDraw"));
		rightEye.cullingMask = ~(1 << LayerMask.NameToLayer("NoDraw"));

		// Adjust Focus Point
		Vector3 focusPoint = lookPoint.position + transform.forward*focusDistance;
		leftEye.transform.LookAt(focusPoint,leftEye.transform.up);
		rightEye.transform.LookAt(focusPoint,rightEye.transform.up);

		if ( !noGUI ) {
			// Set Logo OnClick
			GameObject.Find("S3D Logo").GetComponent<Button>().onClick.AddListener(() => HideGui());

			// Set Interlaced OnClick
			GameObject.Find("Interlaced Button").GetComponent<Button>().onClick.AddListener(() => ChangeToInterlace());

			// Set Side By Side OnClick
			GameObject.Find("Side by Side Button").GetComponent<Button>().onClick.AddListener(() => ChangeToSideBySide());

			// Set 2D/3D OnClick
			GameObject.Find("2D Button").GetComponent<Button>().onClick.AddListener(() => ChangeDimension());

			// Set depth OnClicks
			GameObject.Find("Depth Less Button").GetComponent<Button>().onClick.AddListener(() => DecrementEyeWidth());
			GameObject.Find("Depth More Button").GetComponent<Button>().onClick.AddListener(() => IncrementEyeWidth());

			// Set world OnClicks
			GameObject.Find("World Near Button").GetComponent<Button>().onClick.AddListener(() => IncrementFocusPoint());
			GameObject.Find("World Far Button").GetComponent<Button>().onClick.AddListener(() => DecrementFocusPoint());

			GameObject.Find("S3D Depth Value").GetComponent<Text>().text = eyeWidth+"";
			GameObject.Find("S3D World Value").GetComponent<Text>().text = focusDistance+"";

			GameObject.Find("Close").GetComponent<Button>().onClick.AddListener(() => HideGui());

			// Reset Gui Camera
			if ( guiCamera != null ) {
				guiCamera.gameObject.SetActive(false);
				guiCamera.gameObject.SetActive(true);
			}

			s3dGui.SetActive(hideGUI);
		}
	}

	public void HideGui(){
		hideGUI = !hideGUI;

		s3dGui.SetActive(hideGUI);
	}

	public void IncrementEyeWidth(){
		eyeWidth += 0.1f;
		AdjustEyeWidth();
	}
	public void DecrementEyeWidth(){
		eyeWidth -= 0.1f;
		AdjustEyeWidth();
	}
	public void IncrementFocusPoint(){
		focusDistance += 0.5f;
		UpdateFocusPoint();
	}
	public void DecrementFocusPoint(){
		focusDistance -= 0.5f;
		UpdateFocusPoint();
	}


	public void AdjustEyeWidth(){
		if ( eyeWidth < 0.1f ) eyeWidth = 0f;
		if ( eyeWidth > 2f ) eyeWidth = 2f;

		leftEye.transform.position = transform.position + (eyeWidth/2.0f)*-transform.right;
		rightEye.transform.position = transform.position + (eyeWidth/2.0f)*transform.right;

		Vector3 focusPoint = lookPoint.position + transform.forward*focusDistance;
		leftEye.transform.LookAt(focusPoint,leftEye.transform.up);
		rightEye.transform.LookAt(focusPoint,rightEye.transform.up);

		GameObject.Find("S3D Depth Value").GetComponent<Text>().text = eyeWidth+"";
	}

	public void UpdateFocusPoint(){
		if ( focusDistance < -6f ) focusDistance = -6f;
		if ( focusDistance > 6f ) focusDistance = 6f;

		Vector3 focusPoint = lookPoint.position + transform.forward*focusDistance;
		leftEye.transform.LookAt(focusPoint,leftEye.transform.up);
		rightEye.transform.LookAt(focusPoint,rightEye.transform.up);

		GameObject.Find("S3D World Value").GetComponent<Text>().text = focusDistance+"";
	}

	public void ChangeToInterlace(){
		leftEye.rect = new Rect(0,0,1f,1f);
		rightEye.rect = new Rect(0,0,1f,1f);
		leftEye.ResetProjectionMatrix();
		rightEye.ResetProjectionMatrix();

		if ( GameObject.Find("Stereo(Clone)") == null ){
			Instantiate(Resources.Load("Screen3D/Stereo"));

			stereoCamera = GameObject.FindWithTag("SteroeCamera");

			stereoCamera.layer = LayerMask.NameToLayer("NoDraw"); 
			stereoCamera.GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("NoDraw");
		} else {
			stereoCamera.SetActive(true);
		}

		stereoCamera.GetComponent<CameraMain>().SetCameras(leftEye,rightEye);

		interlaced = true;
		GetComponent<S3D>().Stereo3D(interlaced);

		if ( guiCamera != null ) {
			guiCamera.gameObject.SetActive(false);
			guiCamera.gameObject.SetActive(true);
		}
	}

	public void ChangeToSideBySide(){
		leftEye.targetTexture = null;
		rightEye.targetTexture = null;

		if ( stereoCamera != null ) stereoCamera.SetActive(false);

		leftEye.rect = new Rect(0,0,0.5f,1f);
		rightEye.rect = new Rect(0.5f,0,0.5f,1f);
		leftEye.ResetProjectionMatrix();
		rightEye.ResetProjectionMatrix();
		leftEye.projectionMatrix *= Matrix4x4.Scale( new Vector3(0.5f,1f,1f) );
		rightEye.projectionMatrix *= Matrix4x4.Scale( new Vector3(0.5f,1f,1f) );

		interlaced = false;
		GetComponent<S3D>().Stereo3D(interlaced);
	}

	public void ChangeDimension(){
		if ( leftEye != null ) {
			leftEye.targetTexture = null;
			leftEye.rect = new Rect(0,0,1f,1f);
			leftEye.ResetProjectionMatrix();
		}
		if ( rightEye != null ) {
			rightEye.targetTexture = null;
			rightEye.rect = new Rect(0,0,1f,1f);
			rightEye.ResetProjectionMatrix();
		}

		if ( stereoCamera != null ) stereoCamera.SetActive(false);

		GetComponent<S3D>().Stereo3D(false);
	}
}