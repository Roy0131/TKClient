Shader "IH/ParAddClip"
{
	Properties
	{
		_TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex ("Particle Texture", 2D) = "white" {}
		
		//新增 记录裁剪框的四个边界的值
		_Area ("Area", Vector) = (0,0,1,1)
		//----end----
	}
	SubShader
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		LOD 100
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				//新增，记录顶点的世界坐标
				float2 worldPos : TEXCOORD1;
				//----end----
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float4 _TintColor;
			
			//新增，对应上面的_Area
			float4 _Area;
			//----end----
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				//新增，计算顶点的世界坐标
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
				//----end----

				return o;

			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				//新增，判断顶点坐标是否在裁剪框内
				bool inArea = i.worldPos.x >= _Area.x && i.worldPos.x <= _Area.z && i.worldPos.y >= _Area.y && i.worldPos.y <= _Area.w;
				//----end----
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv)*_TintColor*2.0f;
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return inArea? col: fixed4(0,0,0,0);
			}
			ENDCG
		}
	}
}
