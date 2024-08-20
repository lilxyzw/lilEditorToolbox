Shader "Hidden/_lil/TexturePacker"
{
    Properties
    {
        _TextureR ("Texture", 2D) = "white" {}
        _TextureG ("Texture", 2D) = "white" {}
        _TextureB ("Texture", 2D) = "white" {}
        _TextureA ("Texture", 2D) = "white" {}
        _BlendR ("Color", Vector) = (0,0,0,0)
        _BlendG ("Color", Vector) = (0,0,0,0)
        _BlendB ("Color", Vector) = (0,0,0,0)
        _BlendA ("Color", Vector) = (0,0,0,0)
        _IgnoreTexture ("Color", Vector) = (1,1,1,1)
        _Invert ("Color", Vector) = (0,0,0,0)
        _DefaultR ("Float", Float) = 1
        _DefaultG ("Float", Float) = 1
        _DefaultB ("Float", Float) = 1
        _DefaultA ("Float", Float) = 1
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            Texture2D _TextureR;
            Texture2D _TextureG;
            Texture2D _TextureB;
            Texture2D _TextureA;
            float4 _BlendR;
            float4 _BlendG;
            float4 _BlendB;
            float4 _BlendA;
            float4 _IgnoreTexture;
            float4 _Invert;
            float _DefaultR;
            float _DefaultG;
            float _DefaultB;
            float _DefaultA;

            float4 vert(float4 vertex : POSITION) : SV_POSITION
            {
                return UnityObjectToClipPos(vertex);
            }

            float4 frag(float4 vertex : SV_POSITION) : SV_Target
            {
                float4 col;
                col.r = dot(_TextureR[vertex.xy], _BlendR);
                col.g = dot(_TextureG[vertex.xy], _BlendG);
                col.b = dot(_TextureB[vertex.xy], _BlendB);
                col.a = dot(_TextureA[vertex.xy], _BlendA);

                col = lerp(col, 1-col, _Invert);
                col = lerp(col, float4(_DefaultR,_DefaultG,_DefaultB,_DefaultA), _IgnoreTexture);
                //if(!IsGammaSpace()) col.rgb = LinearToGammaSpace(col.rgb);
                return col;
            }
            ENDCG
        }
    }
}
