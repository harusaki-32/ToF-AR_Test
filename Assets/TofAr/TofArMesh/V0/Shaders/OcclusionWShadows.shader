Shader "VR/OcclusionWShadows"
{
	Properties
	{
		_ShadowIntensity("Shadow Intensity", Range(0,2)) = 0.6
	}
		SubShader
	{
		Tags { "RenderType" = "Transparent" "RenderQueue" = "Geometry"}

		Blend SrcAlpha zero

		Pass
		{
			Stencil {
				Ref 2
				Comp NotEqual
				Pass Keep
			}

			Tags {"LightMode" = "ForwardBase"}
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fwdbase
			#include "AutoLight.cginc"
			#include "UnityCG.cginc"

			float _ClippingDistance;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				LIGHTING_COORDS(0,1)
				float4 pos : SV_POSITION;
				float clip : DEPTH;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				if (v.vertex.z < _ClippingDistance) {
					o.clip = 0;
				}
				else {
					o.clip = -1;
				}
				TRANSFER_VERTEX_TO_FRAGMENT(o);
				return o;
			}
			float _ShadowIntensity;

			fixed4 frag(v2f i) : COLOR
			{
				//shadow parameter
				float atten = (1 - LIGHT_ATTENUATION(i)) * _ShadowIntensity;
				fixed4 col = fixed4(0, 0, 0, atten);
				clip(i.clip);
				return col;
			}
			ENDCG
		}
	}
		FallBack "Diffuse"
}
