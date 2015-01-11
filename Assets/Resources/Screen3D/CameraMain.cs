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
using System.Collections;

public class CameraMain : MonoBehaviour {
	public Shader shader;

	public RenderTexture tex1;
	public RenderTexture tex2;
	public Camera cam1;
	public Camera cam2;

	// Use this for initialization
	void Start () {
		
	}
	
	public void SetCameras(Camera c1, Camera c2){
		cam1 = c1;
		cam2 = c2;
		
		ApplyShader();
	}
	
	void ApplyShader(){
		Shader.SetGlobalFloat ("_ScreenWidth", Screen.width);
		Shader.SetGlobalFloat ("_ScreenHeight", Screen.height);
		tex1 = new RenderTexture (Screen.width, Screen.height, 16);
		tex1.Create ();
		cam1.targetTexture = tex1;
		tex1.SetGlobalShaderProperty ("_Texture1");
		tex2 = new RenderTexture (Screen.width, Screen.height, 16);
		tex2.Create ();
		cam2.targetTexture = tex2;
		tex2.SetGlobalShaderProperty ("_Texture2");
		GetComponent<Camera>().SetReplacementShader (shader, "");
	}
}
