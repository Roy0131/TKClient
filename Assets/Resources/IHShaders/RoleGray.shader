Shader "IHGame/RoleGray" {
Properties {
 [HideInInspector]_Cutoff ("Shadow alpha cutoff", Range(0,1)) = 0.1
 [HideInInspector][NoScaleOffset]  _MainTex ("Main Texture", 2D) = "black" { }
}
SubShader { 
		 Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "PreviewType"="Plane" }
		 Pass {
			  Tags { "QUEUE"="Transparent" "IGNOREPROJECTOR"="true" "RenderType"="Transparent" "PreviewType"="Plane" }
			  ZWrite Off
			  Cull Off
			  Blend One OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#include "UnityCG.cginc"

			float4 _MainTex_ST;

			struct appdata {
			  float3 pos : POSITION;
			  half4 color : COLOR;
			  float3 uv0 : TEXCOORD0;
			  UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f {
			  fixed4 color : COLOR0;
			  float2 uv0 : TEXCOORD0;
			  float4 pos : SV_POSITION;
			  UNITY_VERTEX_OUTPUT_STEREO
			};

			// vertex shader
			v2f vert (appdata IN) {
			  v2f o;
			  UNITY_SETUP_INSTANCE_ID(IN);
			  UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			  half4 color = IN.color;
			  half3 viewDir = 0.0;
			  o.color = saturate(color);
			  o.uv0 = IN.uv0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
			  o.pos = UnityObjectToClipPos(IN.pos);
			  return o;
			}

			// textures
			sampler2D _MainTex;

			// fragment shader
			fixed4 frag (v2f IN) : SV_Target {
			  fixed4 col;
			  fixed4 tex, tmp0, tmp1, tmp2;
			  tex = tex2D (_MainTex, IN.uv0.xy);
			  fixed gray = dot(tex.rgb, float3(0.299, 0.587, 0.114));  
			  tex.rgb = float3(gray, gray, gray);  
			  col = tex * IN.color;
			  return col;
			}

			ENDCG
			 }
		}
}