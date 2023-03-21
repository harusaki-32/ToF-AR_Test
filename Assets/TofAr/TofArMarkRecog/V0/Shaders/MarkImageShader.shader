Shader "Unlit/MarkImageShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Angle("Angle", int) = 0
	}
	SubShader
	{
		/*Tags { "RenderType"="Opaque" }
		LOD 100*/
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog 
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				//UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			//float4 _MainTex_ST;
			int _Angle;
			
			/*v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				
				float2x2 rotate = float2x2(cos(_Angle), -sin(_Angle), sin(_Angle), cos(_Angle));

				float scale = 0.5;
				float2 pivot_uv = float2(0.5, 0.5);

				float2 uv_rot = v.uv;
				//float2 uv_rot = TRANSFORM_TEX(v.uv, _MainTex);

				uv_rot = (uv_rot - pivot_uv) * (1 / scale);
				o.uv = mul(rotate, uv_rot) + pivot_uv;

				o.uv = TRANSFORM_TEX(o.uv, _MainTex);

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				
				return col;
			}*/

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				/*float scale = 0.5;
				float2 pivot_uv = float2(0.5, 0.5);
				float2 uv_rot = i.uv;
				float2x2 rotate = float2x2(cos(_Angle), -sin(_Angle), sin(_Angle), cos(_Angle));
				uv_rot = (uv_rot - pivot_uv) * (1 / scale);
				uv_rot = mul(rotate, uv_rot) + pivot_uv;*/

				fixed4 col;
				if (_Angle == 0)
				{
					col = tex2D(_MainTex, i.uv);
				}
				else if (_Angle == 180)
				{
					col = tex2D(_MainTex, 1 - i.uv);
				}
				else if (_Angle == 90)
				{
					float2 uv_t = float2(i.uv.y, 1-i.uv.x);
					col = tex2D(_MainTex, uv_t);
				}
				else 
				{
					float2 uv_t = float2(1-i.uv.y, i.uv.x);
					col = tex2D(_MainTex, uv_t);
				}
				//fixed4 col = tex2D(_MainTex, i.uv);
				// just invert the colors
				//col = 1 - col;
				return col;
			}
			ENDCG
		}
	}
}
