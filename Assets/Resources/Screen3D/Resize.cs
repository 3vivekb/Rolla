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
using System;

public class Resize : MonoBehaviour {
	public Camera cam;
	// Use this for initialization
	void Start () {
		float h = 2 * cam.orthographicSize;
		float w = h * cam.aspect;
		Vector3 center = gameObject.transform.position;
		Vector3 centerP = new Vector3();
		Vector3 centerN = new Vector3();
		
		centerP.x = (center.x + 30.0f*0.5f)/30.0f;
		centerP.y = (center.y + 20.0f*0.5f)/20.0f;
		
		centerN.x = w * centerP.x - w/2.0f;
		centerN.y = h * centerP.y - h/2.0f;
		centerN.z = center.z;

		Bounds b = ((MeshRenderer)gameObject.GetComponent("MeshRenderer")).bounds;
		float width = b.max[0] - b.min[0];
		float height = b.max[1] - b.min[1];
		
		float pWidth = width/30.0f;
		float pHeight = height/20.0f;
		
		float newWidth = pWidth*w;
		float newHeight = pHeight*h;
		
		float scaleFactorX = newWidth / width;
		float scaleFactorY = newHeight / height;
		
		Vector3 v = new Vector3( centerN.x + cam.transform.position.x,
		                        centerN.y + cam.transform.position.y,
		                        centerN.z);
		this.transform.position = v;
		this.transform.localScale = new Vector3(this.transform.localScale.x * (scaleFactorX), 0, this.transform.localScale.z * (scaleFactorY));
	}
}
