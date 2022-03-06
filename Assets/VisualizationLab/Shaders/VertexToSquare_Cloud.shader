Shader "VertexToSquare_Cloud"
{
    Properties
    {
        main_color ("MainColor", Color) = (0.5,0.5,1.0,1.0)
	    particle_size("ParticleSize",float) = 0.05
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
            #pragma geometry geom
            #pragma target 5.0
            #include "UnityCG.cginc"

            float screen_ratio;//height/width
            float4 main_color;
			StructuredBuffer<float3> buffer;

			float particle_size = 0.05f;

			struct v2g
			{
				float4 pos : SV_POSITION;
				float depth : TEXCOORD1;
			};

			struct g2f
			{
				float4 pos : SV_POSITION;
				float depth : TEXCOORD1;
			};

			struct v2f
			{
				float4 pos : SV_POSITION;
				float depth : TEXCOORD1;
			};

//			v2f vertex_shader(uint id : SV_VertexID)
//			{
//				v2f p;
//				float3 T = cloud[id];
//				p.pos = UnityObjectToClipPos(T);
//				p.depth = UnityObjectToViewPos(T).z * 10 + 600;
//				return p;
//			}
//
//			float4 fragment_shader(v2f p) : COLOR
//			{
//				return float4(main_color.rgb * p.depth / 1000, 0.4);
//			}

				v2g vert(uint id : SV_VertexID)
				{
					v2g p;
					float3 T = buffer[id];
					p.pos = UnityObjectToClipPos(T);
					p.depth = UnityObjectToViewPos(T).z * 10 + 600;
					return p;
				}

				[maxvertexcount(6)]
				void geom(point v2g input[1], inout TriangleStream<g2f> triangle_stream)
				{
					v2g v = input[0];
//					v.pos.z = 0.1;
					g2f o;
					//o.depth = v.depth;
					//o.pos = v.pos;
					//triangle_stream.Append(o);
					for (int i = 0; i < 3; i++)
					{
						o.pos = v.pos +
							float4(particle_size * float3(screen_ratio * ( i == 0), i == 1, 0), 1);
						o.depth = v.depth;
						triangle_stream.Append(o);
					}
					triangle_stream.RestartStrip();

					for (int j = 0; j < 3; j++)
					{
						o.pos = v.pos +
							float4(particle_size * float3(screen_ratio * (j != 2), j != 0, 0), 1);
						o.depth = v.depth;
						triangle_stream.Append(o);
					}
					triangle_stream.RestartStrip();
				}

				float4 frag(g2f p) : COLOR
				{
					return float4(main_color.rgb * p.depth / 1000, 0.4);
				}

				ENDCG
        }
    }
}
