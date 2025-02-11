Shader "Hidden/RectToggle"
{
    Properties
    {
        _ActiveColor("Active Color", Color) = (0.317647,0.317647,0.317647,1)
        _InactiveColor("Inactive Color", Color) = (0.1568627,0.1568627,0.1568627,1)
        _BorderColor("Border Color", Color) = (0.1411764,0.1411764,0.1411764,1)
        _Roundness("Roundness", float) = 11
        _BorderWidth("Border Width", float) = 2
        _AnimationPercentage("_AnimationPercentage", float) = 0
        _Width("_Width", float) = 100
        _Height("_Height", float) = 100
        _UsingLinearColorSpace("Using Linear Color Space", int) = 0
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

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

            fixed4 _ActiveColor;
            fixed4 _InactiveColor;
            fixed4 _BorderColor;
            float _Roundness;
            float _BorderWidth;

            float _AnimationPercentage;
            float _Width;
            float _Height;


            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            static float rectangle(float2 samplePosition, float2 halfSize, float2 rectPosition)
            {
                float2 offsetSamplePosition = samplePosition - rectPosition;
                float2 componentWiseEdgeDistance = abs(offsetSamplePosition) - halfSize;
                float outsideDistance = length(max(componentWiseEdgeDistance, 0));
                float insideDistance = min(max(componentWiseEdgeDistance.x, componentWiseEdgeDistance.y), 0);
                return outsideDistance + insideDistance;
            }
            //#include "Assets/SDF/SignedDistanceFields2D.hlsl"

            int _UsingLinearColorSpace;
            fixed4 GetColorInColorSpace(fixed4 color) 
            {
                if (_UsingLinearColorSpace == 1) 
                {
                    //color = fixed4(255*pow(color.r/255,2.2), 255*pow(color.g/255,2.2), 255*pow(color.b/255,2.2), 255*pow(color.a/255,2.2));
                    color = fixed4(pow(color.r,0.4545), pow(color.g,0.4545), pow(color.b,0.4545), pow(color.a,0.4545));
                }
                return color;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 size = float2(_Width,_Height);
                //float2 aspectRatioScale = _AspectRatio > 1 ? float2(1, 1 / _AspectRatio) : float2(1 / _AspectRatio, 1);
                float2 rescaledUVs = i.uv * size;//((i.uv - float2(0.5, 0.5)) * aspectRatioScale) + float2(0.5, 0.5);
                
                // Static Border/Background Box
                float outerSDF = rectangle(rescaledUVs, 0.5*size - _Roundness, 0.5*size);

                float bgMask = step(_Roundness, outerSDF);
                float borderMask = step(_Roundness - _BorderWidth, outerSDF) - bgMask;

                float alpha = 1-bgMask;

                fixed4 col = lerp(GetColorInColorSpace(_InactiveColor), GetColorInColorSpace(_BorderColor), borderMask);
                col.a = alpha;

                // Animated Box
                float boxPosition = lerp(0.25, 0.75, smoothstep(0, 1, _AnimationPercentage));

                float boxSDF = rectangle(rescaledUVs, float2(0.25, 0.5)*size - _Roundness, float2(boxPosition, 0.5)*size);

                float boxMask = 1 - step(_Roundness - _BorderWidth, boxSDF);

                col = lerp(col, GetColorInColorSpace(_ActiveColor), boxMask);

                return col;
            }
            ENDCG
        }
    }
}

