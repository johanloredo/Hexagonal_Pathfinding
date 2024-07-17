Shader "Custom/MobileTextureColor"
{
    Properties
    {
        _Color("Color",Color) = (1.0,1.0,1.0,1.0)
        _MainTex("Base (RGB)", 2D) = "white" {}
    }

    SubShader
    {
        Tags 
        {
            "Queue" = "Transparent" 
            "IgnoreProjector" = "True" 
            "RenderType" = "Transparent"
        }
        LOD 150

        CGPROGRAM
        #pragma surface surf Lambert fullforwardshadows alpha

        sampler2D _MainTex;
        float4 _Color;

        struct Input
        {
            float2 uv_MainTex;
        };

        void surf(Input IN, inout SurfaceOutput o)
        {
            float4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c.rgb;
            o.Alpha = c.a;
        }
        ENDCG
    }
    Fallback "Mobile/VertexLit"
}