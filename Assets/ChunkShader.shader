Shader "Custom/ChunkShader" {
	Properties {
		_Color ("Diffuse Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Lambert

		float4 _Color;

		struct Input {
			float4 nothing;
		};

		void surf (Input IN, inout SurfaceOutput o) {
		o.Albedo = _Color.rgb;
		
		}
		ENDCG
	} 
	FallBack "Diffuse"
}
