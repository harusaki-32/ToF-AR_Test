// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// *****************************************************
// Optimized BumpedSpecular w/ RimLighting shader
// 
// Limitation - 1
// Only on 1 DirectionalLight. No pixel lights.
//
// Limitation - 2
// No shadow casting.
// *****************************************************
Shader "Custom/NPRv2/snow" {
	Properties {
		_MainTex ("_MainTex", 2D) = "white" {}
		_Color("_Color", Color) = (1,1,1,1)
	_Specular("_Specular", float) = 0.5
		_Normal ("_Normal", 2D) = "bump" {}
		_ShadowColor("_ShadowColor", Color) = (1,1,1,1)
		_SkyBox("_SkyBox", CUBE) = "white" {}
		_SkyIntensity("_SkyIntensity", float) = 0
	}
      CGINCLUDE

      #include "UnityCG.cginc"
      #include "AutoLight.cginc"

      // color of light source (from "Lighting.cginc")
      uniform float4 _LightColor0; 
 
      // User-specified properties
      uniform sampler2D _MainTex; 
      uniform sampler2D _Normal;
	  uniform float _Specular;
	  uniform float _SkyIntensity;
      uniform samplerCUBE _SkyBox;
      uniform float4 _Normal_ST;
	  uniform float4 _ShadowColor;
	  uniform float4 _Color;

      struct AppData {
         float4 vertex : POSITION;
         float4 texcoord : TEXCOORD0;
         float3 normal : NORMAL;
         fixed4 vColor : COLOR;
      };
      struct v2f {
         float4 pos : SV_POSITION;
         float4 posWorld : TEXCOORD0;
         float4 uv : TEXCOORD1;
         float3 normal : TEXCOORD2;
		 float3 viewDir : TEXCOORD3;
		 float3 lightDir : TEXCOORD4;
         fixed4 vColor : COLOR;
         UNITY_FOG_COORDS(5)
         LIGHTING_COORDS(6,7)
      };
 
      v2f vert(AppData v) 
      {
		v2f o;

		o.posWorld = mul(unity_ObjectToWorld, v.vertex);
		o.pos = UnityObjectToClipPos(v.vertex);
		o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
		o.lightDir = normalize(ObjSpaceLightDir(v.vertex));
		o.normal = v.normal;
		o.uv = v.texcoord;
		o.vColor = v.vColor;

		UNITY_TRANSFER_FOG(o,o.pos);
		TRANSFER_VERTEX_TO_FRAGMENT(o);

		return o;
      }
      
      // fragment shader with ambient lighting
	float4 frag(v2f i) : COLOR
   {         
	// **********
	// Normal
	// **********
	float3 normal = UnpackNormal(tex2D(_Normal, i.uv * _Normal_ST.xy));
	// Blending world normal with normal map
	normal = normalize(float3(i.normal.xy + normal.xy, i.normal.z * normal.z));

    float3 ambient = UNITY_LIGHTMODEL_AMBIENT.rgb;
 
	float4 col = tex2D(_MainTex, i.uv.xy);
	
	// **********
	// Lighting
	// **********
	//fixed atten = SHADOW_ATTENUATION(i);

	//float diffuseFactor = max(0.0, dot(normal, i.lightDir));
    //float3 diffuse = ambient +
	//	_LightColor0.rgb * diffuseFactor;
 
	float3 specular;
		specular = _LightColor0.rgb * pow(max(0.0, dot(reflect(-i.lightDir, normal), i.viewDir)), _Specular);

	float rim = pow((1.3 - dot(i.viewDir, normal)), 2);
		 
	float3 skyColor = texCUBE(_SkyBox, reflect(i.viewDir, -normal)) * _SkyIntensity * rim;
	// **********
	// Combining
	// **********
	col *= _ShadowColor;
	col += float4(specular + skyColor, 0);
	//col *= clamp(_FootColor, 1, lerp(0, 1, i.posWorld.y / _FootRatio));


	//UNITY_APPLY_FOG(i.fogCoord, col);
	//col.a = 1;
	col.a = _ShadowColor.a * _Color.a;
	return col;// *i.vColor;

	}
   ENDCG
	SubShader {
		Pass {      
	 		Tags { "Queue"="Transparent" "RenderType"="Transparent"
				   "LightMode"="ForwardBase"}
			Blend SrcAlpha OneMinusSrcAlpha
			Cull Off
			ZTest Less
			Lighting On
 
         	CGPROGRAM
	            #pragma vertex vert  
	            #pragma fragment frag
	            #pragma multi_compile_fog
				#pragma multi_compile_fwdadd_fullshadows


	            // the functions are defined in the CGINCLUDE part
         	ENDCG
         }

   }
   Fallback "Mobile/VertexLit"
}