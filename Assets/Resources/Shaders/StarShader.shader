// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/StarShader"
{
    Properties
    {
		_BaseColor ("Base colour", Color) = (1,1,1,1)
		_Height("Height", Range(0, 1)) = 0
		_MainTex ("Texture", 2D) = "Assets/Resources/Textures/sunTex2.jpg"
		_EmissionMap ("Emission Map", 2D) = "Assets/Resources/Textures/sunTex2.jpg" 
		[NoScaleOffset] _HeightMap ("Heights", 2D) = "gray" {}
    }
    SubShader
    {
		
		//TODO : Jouer un peu plus avec les offsets et le tiling, et la HeightMap
		// Ajouter une émission de lumière
		// Ajouter un post process, un halo/brouillard etc..

		//Nettoyer et choisir methode
        Tags { 
			"RenderType"="Opaque"
		}

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

			half4 _BaseColor;

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _HeightMap;
			sampler2D _EmissionMap;

			float _Height;

            #include "UnityCG.cginc"

            struct vertInput
            {
                float4 position : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
            };

			struct vertOutput
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
			};

			
            vertOutput vert (vertInput v)
            {
                vertOutput o;
                o.pos = UnityObjectToClipPos(v.position);
				o.normal = mul(unity_ObjectToWorld, float4(v.normal, 0));
				o.normal = normalize(o.normal);
                o.uv = v.uv * (1.3,1.3)*_MainTex_ST.xy + (1.7,1)*_MainTex_ST.zw;
				return o;
            }

			//Nettoyer ça
            half4 frag (vertOutput o) : SV_TARGET 
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, o.uv);
				//col *= _BaseColor;
				//return col;
				o.normal = normalize(o.normal);

				float3 viewDir = normalize(_WorldSpaceCameraPos - o.worldPos);

				float3 albedo = tex2D(_MainTex, o.uv).rgb * _BaseColor.rgb;
				albedo *= tex2D(_HeightMap, o.uv);

				return float4(albedo, 1);
            } 
            ENDCG
        }
    }
}
