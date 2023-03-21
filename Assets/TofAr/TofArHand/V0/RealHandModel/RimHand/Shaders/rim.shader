Shader "Custom/rim" {
	Properties{
		_MainColor("Main Color", Color) = (1,1,1,1)
		_Color("Color", Color) = (1,1,1,1)
		_RimColor("Rim Color", Color) = (1,1,1,1)
	}
		SubShader{
		Tags{ "RenderType" = "Opaque"
			  "Queue" = "Transparent"}
		LOD 200

		CGPROGRAM
#pragma surface surf Standard  alpha:fade
#pragma target 3.0

		struct Input {
		float2 uv_MainTex;
		float3 worldNormal;
		float3 viewDir;
	};
	fixed4 _MainColor;
	fixed4 _RimColor;
	fixed4 _Color;

	void surf(Input IN, inout SurfaceOutputStandard o) {
		o.Albedo = _MainColor;
		float alpha = 1 - (abs(dot(IN.viewDir, o.Normal)));
		o.Alpha = alpha * 1.5f * _Color.a;
		float rim = 1 - saturate(dot(IN.viewDir, o.Normal));
		o.Emission = _RimColor * pow(rim, 2.5);
	}
	ENDCG
	}
		FallBack "Diffuse"
}
