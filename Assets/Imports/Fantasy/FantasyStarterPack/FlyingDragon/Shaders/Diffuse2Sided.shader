Shader "ArtCoding/FlyingDragon/Diffuse 2-sided" {
	
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Cutout( "Alpha Cutout", Float ) = 0.5
	}
	SubShader {
		Tags{ "RenderType" = "TransparentCutout"  "Queue" = "AlphaTest" "IgnoreProjector" = "True" }
		Cull Off
		
		CGPROGRAM
		
		#pragma surface surf StandardSpecular keepalpha addshadow fullforwardshadows 
		#pragma target 3.0
		#pragma multi_compile_instancing

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
			fixed4 color : COLOR;
		};
		
		half4 _Color;
		half _Cutout = 0.5;

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * UNITY_ACCESS_INSTANCED_PROP(_Color_arr, _Color);
			
			clip(c.a - _Cutout );
			
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}
		ENDCG
	}
	Fallback "Standard (Specular setup)"
	Fallback "Diffuse"
}
