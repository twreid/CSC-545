#ifndef LCMBLUR_FX
#define LCMBLUR_FX

//--------------------------------------------------------------------------------------
// Global variables
//--------------------------------------------------------------------------------------
Texture2D g_LCMB;

SamplerState samLCMB
{
        Filter = ANISOTROPIC;
        MaxAnisotropy=16;
        AddressU = BORDER;
        AddressV = BORDER;
        BorderColor = float4(1.0f,1.0f,1.0f,1.0f);
};

//--------------------------------------------------------------------------------------
// Pixel shaders
//--------------------------------------------------------------------------------------
#define Size 0

float2 PS_LCMBLURH(in PS_INPUT_QUAD input) : SV_Target
{
        float depth=g_LCM.Sample(samLCMB,input.vTexUV).y;
        
        int n=0;
        
        float sum=0.0f;
        
        [unroll] for (int i=-1*Size;i<(Size+1);i++)
        {
                float2 s=g_LCM.Sample(samLCMB,input.vTexUV,int2(i,0)).xy;
                if (abs(depth-s.y)<1.0f)
                {
                        sum+=s.x;
                        n++;
                }
        }
        
        return float2(sum/n,depth);
}

float2 PS_LCMBLURV(in PS_INPUT_QUAD input) : SV_Target
{
        float depth=g_LCMB.Sample(samLCMB,input.vTexUV).y;
        
        int n=0;
        
        float sum=0.0f;
        
        [unroll] for (int i=-1*Size;i<(Size+1);i++)
        {
                float2 s=g_LCMB.Sample(samLCMB,input.vTexUV,int2(0,i)).xy;
                if (abs(depth-s.y)<1.0f)
                {
                        sum+=s.x;
                        n++;
                }
        }
        
        return float2(sum/n,depth);
}

//--------------------------------------------------------------------------------------
// Techniques
//--------------------------------------------------------------------------------------

technique10 LCMBLURH
{
        pass P0
        {
        SetVertexShader(CompileShader(vs_4_0,VS_QUAD()));
        SetGeometryShader(NULL);
        SetPixelShader(CompileShader(ps_4_0,PS_LCMBLURH()));
        }
}

technique10 LCMBLURV
{
        pass P0
        {
        SetVertexShader(CompileShader(vs_4_0,VS_QUAD()));
        SetGeometryShader(NULL);
        SetPixelShader(CompileShader(ps_4_0,PS_LCMBLURV()));
        }
}

#endif