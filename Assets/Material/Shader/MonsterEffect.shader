Shader "Custom/Test/Pulsate"{

Properties{

	_MainTex("MainTex", 2D) = "white" {}
	_Color("Color", Color) = (1,1,1,1)
	_Scale("Scale", Range(-1,0)) = 0
	_RotationSpeed("RotationSpeed", Range(0,10)) = 5
	//Cheese shit
	_DissolveTexture("Cheese", 2D) = "white" {}
	_DissolveAmount("Cheese Cut Out", Range(0,1)) = 1

	

}

	SubShader{

		Pass{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag_img
			#include "UnityCG.cginc" 

			float _RotationSpeed;
			float _Scale;
			float4 _Color;
			sampler2D _MainTex;


			sampler2D _DissolveTexture;
			float _DissolveAmount;


			struct appData{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			//BUILDS OBJECT
			v2f vert_img(appData IN){
				
				v2f OUT;

				IN.vertex.xyz += IN.normal.xyz * _Scale * sin(_Time);
				
				//Rotation
				float sinX = sin ( _RotationSpeed * _Time );
				float cosX = cos ( _RotationSpeed * _Time );
				float sinY = sin ( _RotationSpeed * _Time );
				float2x2 rotationMatrix = float2x2( cosX, -sinX, sinY, cosX);
				IN.uv.xy = mul ( IN.uv.xy, rotationMatrix );

				OUT.position = UnityObjectToClipPos(IN.vertex);//UnityObjectToClipPos(IN.vertex);
				OUT.uv = IN.uv;

				return OUT;
			}

			//COLORS OBJECT
			fixed4 frag_img(v2f IN) : SV_TARGET{
			
				float4 textureColor = tex2D(_MainTex, IN.uv);
				float4 dissolveColor = tex2D(_DissolveTexture, IN.uv);
				float newDissolve = _DissolveAmount * sin(_Time.y);
				clip(dissolveColor.rgb - newDissolve);

				return textureColor * _Color;
			}
			ENDCG
		}

	}
}