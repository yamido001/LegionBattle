// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/OpFactorDiffuse" 
{
	Properties 
	{
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
	}

	SubShader 
	{
		Tags { "RenderType"="Opaque" "LightMode" = "ForwardBase"}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "Lighting.cginc"

			struct v2f
			{
				float4 vertex : SV_POSITION;
				// uv and cos_a
				fixed3 uv : TEXCOORD0;
			};

			sampler2D _MainTex;

			v2f vert (float4 vertex : POSITION, float2 uv : TEXCOORD0, float3 normal : NORMAL)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(vertex);

				float3 world_normal = UnityObjectToWorldNormal(normal);
				float3 world_pos = mul(unity_ObjectToWorld, vertex);
				float3 light_dir = normalize(UnityWorldSpaceLightDir(world_pos));

				o.uv = fixed3(uv, 0.5 + 0.5 * max(0, dot(world_normal, light_dir)));

				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 diffuse = tex2D(_MainTex, i.uv.xy);
				fixed3 albedo = diffuse.rgb;
				fixed4 col = 0;
				col.a = diffuse.a;

				col.rgb += _LightColor0.rgb * albedo * i.uv.z;

				return col;
			}
			ENDCG
		}
	} 
}
