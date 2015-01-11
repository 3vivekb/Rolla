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
using System.IO;
using System;
using AOT;

public class S3D : MonoBehaviour 
{
	static short MI3D_TN_CMD_OFF = ((short) 0x10);
	static short MI3D_TN_CMD_VERTICAL = ((short) 0x20);

	#if UNITY_ANDROID || UNITY_EDITOR

	public void Stereo3D(bool state) 
	{
		short mode = MI3D_TN_CMD_VERTICAL;
		if (state == false)
			mode = MI3D_TN_CMD_OFF;
		try
		{
			using (AndroidJavaClass SDecKit = new AndroidJavaClass("org.sdeck.SDecKit"))
			{
				SDecKit.CallStatic("setTnMode", mode);
			}
		}
		catch (Exception exc)
		{
		}		
	}
	
	void OnDisable()
	{
		Stereo3D(false);
	}
	
	void OnApplicationFocus(bool state)
	{
		Stereo3D(state);
	}
	
	#endif
}