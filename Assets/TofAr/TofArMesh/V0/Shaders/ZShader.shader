Shader "TofAr/ZShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_DistanceMultiplier("Distance Multiplier", float) = 1
		_Alpha("Alpha", float) = 1
	}
	SubShader
	{
		Tags{ "DisableBatching" = "true"}
		Tags { "RenderType"="Transparent" "RenderQueue"="Transparent"}
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

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
				float z: TEXCOORD0;
				float2 uv : COLOR;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _DistanceMultiplier;
			float _Alpha;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.z = v.vertex.z;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			/*
			mapping
			{
			0    < d < 5/6   ->     0, d*6/5, 1
			5/6  < d < 10/6  ->     0, 1, 1 - (d - 5/6)*6/5
			10/6 < d < 15/6  ->     (d-10/6)*6/5, 1, 0
			15/6 < d < 20/6  ->     1,1 - (d-15/6)*6/5, 0
			20/6 < d < 25/6  ->     1,0,(d-20/6)*6/5
			25/6 < d < 30/6  ->     1,1(d-25/6)*6/5, 1
			}
			which tidies up to
			{
			0    < d < 5/6   ->     0, d*6/5, 1
			5/6  < d < 10/6  ->     0, 1, 2 - d*6/5
			10/6 < d < 15/6  ->     d*6/5 - 2 , 1, 0
			15/6 < d < 20/6  ->     1, 4-d*6/5, 0
			20/6 < d < 25/6  ->     1,0,d*6/5 - 4
			25/6 < d < 30/6  ->     1,d*6/5 - 5, 1
			}

			this can be re expressed as two arrays M and A where col = M*d*6/5 + A
			these arrays are indexed by d*6/5, which avoids the branching problem

			M = [[0,1,0], [0,0,-1], [1,0,0],  [0,-1,0], [0,0,1],  [0,1,0]]
			A = [[0,0,1], [0,1,2],  [-2,1,0], [1,4,0],  [1,0,-4], [1,-5,1]]

			*/
			fixed4 applyColorMap(float d, int idx)
			{
				static half4 colormapM[6] = { half4(0,1,0,0),half4(0,0,-1,0),half4(1,0,0,0),half4(0,-1,0,0),half4(0,0,1,0),half4(0,1,0,0) };
				static half4 colormapA[6] = { half4(0,0,1,1),half4(0,1,2,1),half4(-2,1,0,1),half4(1,4,0,1),half4(1,0,-4,1),half4(1,-5,1,1) };
				return colormapM[idx] * d + colormapA[idx];
			}

			fixed4 frag (v2f i) : SV_Target
			{
				float u16 = i.z * 1000.0f;
				float colScaleD = u16 * .0012*_DistanceMultiplier; // /1000 to adjust the scale (6/(5*1000))
				int idx = floor(colScaleD);
				fixed4 col;
				if (idx < 0) return col = fixed4(0, 0, 1, _Alpha);
				if (idx > 5) return col = fixed4(1, 1, 1, _Alpha);

				col = applyColorMap(colScaleD, idx);
				col.a = _Alpha;
				return col;
			}
			ENDCG
		}
	}
}
