/*
 * Copyright (c) 2014. InS3D.com. All rights reserved.
 *
 * The contents of this file are subject to the license terms.
 * You may not use, reproduce, distribute or modify this file
 * except in compliance with the License.
 *
 * PROPRIETARY/CONFIDENTIAL. DO NOT ALTER OR REMOVE THIS HEADER.
 */

Shader "Custom/TextureCoordinates/Base" {
    Properties {
        _ScreenWidth ("ScreenWidth", Range(0,10000.0)) = 1077.0
        _ScreenHeight ("ScreenHeight", Range(0,10000.0)) = 596.0
        _Texture1 ("Texture1", 2D) = "white" {}
        _Texture2 ("Texture2", 2D) = "white" {}
    }
    SubShader {
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0

            #include "UnityCG.cginc"
            
            uniform sampler2D _Texture1;
            uniform sampler2D _Texture2;
            float _ScreenWidth;
            float _ScreenHeight;

            struct vertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };

            struct fragmentInput{
                float4 position : SV_POSITION;
                float2 texcoord0 : TEXCOORD0;
            };

            fragmentInput vert(vertexInput i){
                fragmentInput o;
                o.position = mul (UNITY_MATRIX_MVP, i.vertex);
                o.texcoord0 = i.texcoord0;
                return o;
            }
            float4 frag(fragmentInput i) : COLOR {
                i.texcoord0.y = 1 - i.texcoord0.y;
                i.texcoord0.x = 1 - i.texcoord0.x;
                float4 pix1 = float4(tex2D(_Texture1, i.texcoord0).xyz,1.0);
                float4 pix2 = float4(tex2D(_Texture2, i.texcoord0).xyz,1.0);
                if( (int)(i.texcoord0.x * _ScreenWidth) % 2 == 0.0){
                    return pix1;
                }
                return pix2;
            }
            ENDCG
        }
    }
}