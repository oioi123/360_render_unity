Shader "Hidden/CubemapToEquirectangular"
{
    Properties
    {
        _Cubemap ("Cubemap", CUBE) = "" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            samplerCUBE _Cubemap;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = i.uv;
                float3 dir;

                // Convert UV to spherical coordinates
                float theta = (uv.x - 0.5) * 2.0 * UNITY_PI;
                float phi = (1.0 - uv.y) * UNITY_PI;

                // Convert spherical coordinates to Cartesian coordinates
                dir.x = -sin(phi) * sin(theta);
                dir.y = cos(phi);
                dir.z = -sin(phi) * cos(theta);

                // Sample the cubemap
                fixed4 col = texCUBE(_Cubemap, dir);
                return col;
            }
            ENDCG
        }
    }
}