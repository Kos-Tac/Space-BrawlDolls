// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Self-Illumin/StarShader"
{
    Properties
    {
		_BaseColor ("Base colour", Color) = (1,1,1,1)
		_MainTex ("Albedo", 2D) = "white"
		_EmissionMap("Emission Map", 2D) = "black" {}
		[HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
    }
    SubShader
    {
		
		//TODO : Jouer un peu plus avec les offsets et le tiling, et la HeightMap
		// Ajouter une émission de lumière
		// Ajouter un post process, un halo/brouillard etc..

		//Nettoyer et choisir methode
        Tags { 
			"RenderType"="Transparent"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#include "UnityCG.cginc"

			half4 _BaseColor;

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _EmissionMap;
			float4 _EmissionColor;

			float _Height;

            #include "UnityCG.cginc"

            struct vertInput
            {
                float4 position : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
			};

			
            v2f vert (vertInput v)
            {
                v2f i;
                i.pos = UnityObjectToClipPos(v.position);
				i.normal = mul(unity_ObjectToWorld, float4(v.normal, 0));
				i.normal = normalize(i.normal);
                i.uv = v.uv * (1.3,1.3)*_MainTex_ST.xy + (1.7,1)*_MainTex_ST.zw;
				return i;
            }

			//Nettoyer ça
            half4 frag (v2f i) : COLOR 
            {
				/*
                fixed4 col = tex2D(_MainTex, i.uv);
				col *= _BaseColor;
				return col;
				*/
				fixed4 albedo = tex2D(_MainTex, i.uv);
				half4 output = half4(albedo.rgb * _BaseColor, albedo.a);
				half4 emission = tex2D(_EmissionMap, i.uv) * _EmissionColor;
				output.rgb += emission.rgb;
				return output;
            } 
            ENDCG
        }
    }
}
