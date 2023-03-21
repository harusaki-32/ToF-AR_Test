Shader "TofAr/PointCloudMesh"
{
	Properties
	{
		_MainColor("Color", Color) = (1, 1, 1)
		_PointSize("Point Size", float) = 0.01
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			
			CGPROGRAM
			#pragma vertex vert_f
			#pragma geometry geom_f
			#pragma fragment frag_f
			#pragma exclude_renderers metal

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 col : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 col : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainColor;
			float4 _MainTex_ST;
			float _PointSize;

			appdata vert_f(appdata v)
			{
				appdata o;
				o.vertex = v.vertex;
				o.col = v.col * _MainColor;
				return o;
			}

			[maxvertexcount(4)]
			void geom_f(point appdata input[1], inout TriangleStream<appdata> stream)
			{
				appdata output;
				output.col = input[0].col;
				float4 pos = input[0].vertex;

				UnityObjectToViewPos(pos);

				float3 right = mul(transpose(UNITY_MATRIX_IT_MV), float3(1, 0, 0));
				float3 up = mul(transpose(UNITY_MATRIX_IT_MV), float3(0, 1, 0));

				float halfSize = _PointSize / 2;
				output.vertex = UnityObjectToClipPos(pos + halfSize * right + halfSize * up);
				stream.Append(output);
				output.vertex = UnityObjectToClipPos(pos + halfSize * right - halfSize * up);
				stream.Append(output);
				output.vertex = UnityObjectToClipPos(pos - halfSize * right + halfSize * up);
				stream.Append(output);
				output.vertex = UnityObjectToClipPos(pos - halfSize * right - halfSize * up);
				stream.Append(output);
			}

			fixed4 frag_f(appdata i) : COLOR
			{
				return i.col;
			}
			ENDCG
		}
	}

	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma only_renderers metal

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float4 col : COLOR;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 col : COLOR;
				float psize : PSIZE;
			};

			sampler2D _MainTex;
			float4 _MainColor;
			float4 _MainTex_ST;
			float _PointSize;
			

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.col = v.col * _MainColor;
				o.psize = _PointSize * 1000;
				return o;
			}

			fixed4 frag(v2f i) : COLOR
			{
				return i.col;
			}
			ENDCG
		}


	}
		Fallback "Diffuse"
}
