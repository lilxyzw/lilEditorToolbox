Shader "Hidden/_lil/FixColor"
{
    Properties
    {
        [MainTexture] _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            Texture2D _MainTex;

            float4 vert(float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }

            float4 frag(float4 vertex : SV_POSITION) : SV_Target
            {
                float4 col = _MainTex[vertex.xy];
                if(col.a != 0) col.rgb /= col.a;
                if(!IsGammaSpace()) col.rgb = LinearToGammaSpace(col.rgb);
                return col;
            }
            ENDCG
        }
    }
}
