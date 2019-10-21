Shader "Custom/RealistcStarShader"
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		[HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows
			#pragma target 3.0

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
				float2 uv_Illum;
			};


			float2 FlowUV(float2 uv, float time) {
				return uv + (0.40 * time);
			}

			half _Glossiness;
			half _Metallic;
			fixed4 _Color;
			fixed4 _EmissionColor;

			void surf(Input IN, inout SurfaceOutputStandard o) {
				float2 uv = FlowUV(IN.uv_MainTex, _Time.y);
				fixed4 c = tex2D(_MainTex, uv) * _Color;
				o.Albedo = c.rgb;
				//o.Metallic = _Metallic;
				//o.Smoothness = _Glossiness;
				o.Emission = c.rgb * _EmissionColor;
				//o.Alpha = c.a;
			}
			ENDCG
		}

	FallBack "Diffuse"
}
