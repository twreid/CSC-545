Texture xRenderedScene;
 
sampler TextureSampler = sampler_state
{
 texture = <xRenderedScene>;
};
 
struct VertexShaderInput
{
 float4 Position : POSITION0;
 float2 TexCoord : TEXCOORD0;
};
 
struct VertexShaderOutput
{
 float4 Position : POSITION0;
 float2 TexCoord : TEXCOORD0;
};
 
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
 VertexShaderOutput output;
 
 output.Position = input.Position;
 output.TexCoord = input.TexCoord;
 
 return output;
}
 
float4 PSNormal(VertexShaderOutput input) : COLOR0
{
 return tex2D(TextureSampler, input.TexCoord);
}

float4 PSGreyScale(VertexShaderOutput input) : COLOR0
{
 float3 color;
 float4 original = tex2D(TextureSampler, input.TexCoord);
 float luminance = dot(original.rgb,  float3(0.30f, 0.59f, 0.11f));
 color.rgb = luminance;
 
 return float4(color, 1);
 
}

float4 PSBlackWhite(VertexShaderOutput input) : COLOR0
{
 float3 color;
 
 float4 original = tex2D(TextureSampler, input.TexCoord);
 float luminance = dot(original.rgb,  float3(0.30f, 0.59f, 0.11f));
 
 
 color.rgb = (luminance > 0.3f) ? 1.0f : 0.0f;
 
 return float4(color, 1);
}

float4 PSSmoothen(VertexShaderOutput input) : COLOR0
{
 float hPixel = 1.0f / 1280.0f;
 float vPixel = 1.0f / 720.0f;
 
 float3 color = float3(0, 0, 0);
 
 color += tex2D(TextureSampler, input.TexCoord) * 4.0f;
 color += tex2D(TextureSampler, input.TexCoord + float2(-hPixel, 0)) * 2.0f;
 color += tex2D(TextureSampler, input.TexCoord + float2(hPixel, 0)) * 2.0f;
 color += tex2D(TextureSampler, input.TexCoord + float2(0, -vPixel)) * 2.0f;
 color += tex2D(TextureSampler, input.TexCoord + float2(0, vPixel)) * 2.0f;
 
 color += tex2D(TextureSampler, input.TexCoord + float2(-hPixel, -vPixel));
 color += tex2D(TextureSampler, input.TexCoord + float2(hPixel, -vPixel));
 color += tex2D(TextureSampler, input.TexCoord + float2(-hPixel, vPixel));
 color += tex2D(TextureSampler, input.TexCoord + float2(hPixel, vPixel));
 
 color /= 16;
 
 return float4(color, 1);
 
}
float4 PSLaplacian(VertexShaderOutput input) : COLOR0
{
 float hPixel = 1.0f / 1280.0f;
 float vPixel = 1.0f / 720.0f;
 
 float3 color = float3(0, 0, 0);
 
 color += tex2D(TextureSampler, input.TexCoord) * 8.0f;
 color -= tex2D(TextureSampler, input.TexCoord + float2(-hPixel, 0));
 color -= tex2D(TextureSampler, input.TexCoord + float2(hPixel, 0));
 color -= tex2D(TextureSampler, input.TexCoord + float2(0, -vPixel));
 color -= tex2D(TextureSampler, input.TexCoord + float2(0, vPixel));
 color -= tex2D(TextureSampler, input.TexCoord + float2(-hPixel, -vPixel));
 color -= tex2D(TextureSampler, input.TexCoord + float2(hPixel, -vPixel));
 color -= tex2D(TextureSampler, input.TexCoord + float2(-hPixel, vPixel));
 color -= tex2D(TextureSampler, input.TexCoord + float2(hPixel, vPixel));
 
 float luminance = dot(color,  float3(0.30f, 0.59f, 0.11f));
 
 return (luminance > 0.1f) ? float4(1,1,1,1) : float4(0,0,0,0);
}

 
technique Laplacian
{
 pass Pass1
 {
 VertexShader = compile vs_2_0 VertexShaderFunction();
 PixelShader = compile ps_2_0 PSLaplacian();
 }
}
 
technique Smoothen
{
 pass Pass1
 {
 VertexShader = compile vs_2_0 VertexShaderFunction();
 PixelShader = compile ps_2_0 PSSmoothen();
 }
}
 
technique BlackWhite
{
 pass Pass1
 {
 VertexShader = compile vs_2_0 VertexShaderFunction();
 PixelShader = compile ps_2_0 PSBlackWhite();
 
}}

technique GreyScale
{
 pass Pass1
 {
 VertexShader = compile vs_2_0 VertexShaderFunction();
 PixelShader = compile ps_2_0 PSGreyScale();
 }
}
 
technique Normal
{
 pass Pass1
 {
 VertexShader = compile vs_2_0 VertexShaderFunction();
 PixelShader = compile ps_2_0 PSNormal();
 }
}