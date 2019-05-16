Shader "IH/UiGrayClip"
{
	Properties
	{
		[PerRendererData]_MainTex ("Texture", 2D) = "white" {}
		_Color("Color",Color) = (1,1,1,1)

		//新增 记录裁剪框的四个边界的值
		_Area ("Area", Vector) = (0,0,1,1)
		//----end----

	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent"  }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
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
				float4 vertex : SV_POSITION;
				//新增，记录顶点的世界坐标
				float2 worldPos : TEXCOORD1;
				//----end----
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;
			//新增，对应上面的_Area
			float4 _Area;
			//----end----
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
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
				fixed4 col = tex2D(_MainTex, i.uv);
				float gray = dot(col.rgb, float3(0.299, 0.587, 0.114))*_Color;
				return inArea?half4(gray,gray,gray,col.a): fixed4(0,0,0,0);
			}
			ENDCG
		}
	}
}
