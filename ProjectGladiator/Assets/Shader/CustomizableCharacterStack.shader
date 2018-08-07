Shader "Custom/CustomizableCharacterStack" {
	Properties {
		_MainColor ("Primary Color", Color) = (1,1,1,1)
		_MainTex ("Primary Albedo (RGB)", 2D) = "white" {}

		_SecondaryColor("Secondary Color", Color) = (1,1,1,0)
		_SecondaryTex("Secondary Albedo (RGB)", 2D) = "white" {}

		_TertiaryColor("Tertiary Color", Color) = (1,1,1,0)
		_TertiaryTex("Tertiary Albedo (RGB)", 2D) = "white" {}

		_QuaternaryColor("Quaternary Color", Color) = (1,1,1,0)
		_QuaternaryTex("Quaternary Albedo (RGB)", 2D) = "white" {}

		_AmbientMap("Ambient Occlusion", 2D) = "white" {}

		_BumpMap("Normal Map", 2D) = "bump" {}

		_RoughnessMap ("Roughness", 2D) = "white" {}
		_MetallicTex("Metallic Albedo (RGB)", 2D) = "white" {}

		_EmisionMap("Emission", 2D) = "white" {}
		_EmissionStrength("Emission Strength", Range(0.0, 25.0)) = 1.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		half _EmissionStrength;

		fixed4 _MainColor;
		fixed4 _SecondaryColor;
		fixed4 _TertiaryColor;
		fixed4 _QuaternaryColor;

		//Texture slots
		sampler2D _MainTex;
		sampler2D _SecondaryTex;
		sampler2D _TertiaryTex;
		sampler2D _QuaternaryTex;

		sampler2D _AmbientMap;

		sampler2D _BumpMap;

		sampler2D _RoughnessMap;
		sampler2D _MetallicTex;

		sampler2D _EmisionMap;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 main = tex2D (_MainTex, IN.uv_MainTex);
			fixed3 c = main.rgb * _MainColor;

			//Color compression stack
			//Secondary
			fixed4 secondary = tex2D(_SecondaryTex, IN.uv_MainTex);
			c.rgb = c.rgb * (1.0 - secondary.a) + secondary.rgb * secondary.a * _SecondaryColor.rgb;

			//Tertiary
			fixed4 tertiary = tex2D(_TertiaryTex, IN.uv_MainTex);
			c.rgb = c.rgb * (1.0 - tertiary.a) + tertiary.rgb * tertiary.a * _TertiaryColor.rgb;

			//Quaternary
			fixed4 quaternary = tex2D(_QuaternaryTex, IN.uv_MainTex);
			c.rgb = c.rgb * (1.0 - quaternary.a) + quaternary.rgb * quaternary.a * _QuaternaryColor.rgb;

			o.Albedo = c.rgb;

			o.Occlusion = tex2D(_AmbientMap, IN.uv_MainTex).r;

			o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
			// Metallic and smoothness come from slider variables
			o.Metallic = tex2D(_MetallicTex, IN.uv_MainTex).r;

			o.Smoothness = 1.0 - tex2D(_RoughnessMap, IN.uv_MainTex).r;

			o.Emission = tex2D(_EmisionMap, IN.uv_MainTex) * _EmissionStrength;

			o.Alpha = main.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
