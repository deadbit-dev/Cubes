Shader "Universal Render Pipeline/Outline"
{
    Properties
    {
		_OutlineColor ("Outline Color", Color) = (1, 1, 1, 0.75)
		_OutlineThickness ("Outline Thickness", Range(0,1)) = 0.15
	}
    
    SubShader
    {
        Tags
        { 
            "RenderPipeline" = "UniversalRenderPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }

        Pass
        {
            Cull Front
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha

            HLSLPROGRAM

            #pragma target 4.5
            #pragma vertex VertexPass
            #pragma fragment FragmentPass

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            #define UNITY_MATRIX_MVP   mul(UNITY_MATRIX_VP, UNITY_MATRIX_M)

            struct MeshData
            {
                float4 vertex : POSITION;
				float4 normal : NORMAL;
            };

            struct FragmentData
            {
                float4 position : SV_POSITION;
            };

            CBUFFER_START(UnityPerMaterial)
                float4 _OutlineColor;
                float _OutlineThickness;
            CBUFFER_END

            FragmentData VertexPass(MeshData input)
            {
                FragmentData output;
                float3 pos = input.vertex.xyz + normalize(input.normal).xyz * _OutlineThickness;
                output.position = mul(UNITY_MATRIX_MVP, float4(pos, 1.0));
                return output;
            }

            float4 FragmentPass(FragmentData input) : SV_Target
            {
				return _OutlineColor;
            }

            ENDHLSL
        }
    }
}