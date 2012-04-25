Texture green;
Texture back;
 
sampler GreenSampler = sampler_state
{
	texture = <green>;
	AddressU = Clamp;
	AddressV = Clamp;
};

sampler BackSampler = sampler_state
{
	texture = <back>;
	AddressU = Clamp;
	AddressV = Clamp;
};
 
struct VertexShaderInput
{
 float4 Position : POSITION0;
 float2 GreenCoord : TEXCOORD0;
 };
 
struct VertexShaderOutput
{
 float4 Position : POSITION0;
 float2 GreenCoord : TEXCOORD0;
 };
 
VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
 VertexShaderOutput output;
 
 output.Position = input.Position;
 output.GreenCoord = input.GreenCoord;
  
 return output;
}
 
float4 PSNormal(VertexShaderOutput input) : COLOR0
{
 return tex2D(GreenSampler, input.GreenCoord);
}

float4 PSChroma(VertexShaderOutput input) : COLOR0
{
 
// float threshold = 20.0;
 
 float3 color;
  
 float4 greenImg = tex2D(GreenSampler, input.GreenCoord);
 float4 backImg  = tex2D(BackSampler, input.GreenCoord);

 if(greenImg.g > (greenImg.r + greenImg.b))
	color.rgb = backImg.rgb;
 else
	color.rgb = greenImg.rgb;
 
 //float3 pixDist = abs(greenImg.rgb - backImg.rgb);
 //float3 dist = dot(pixDist, float3(1, 1, 1));

 //float alphaDist = smoothstep(0, threshold, dist);
 //float alpha = 1.0 - saturate(alphaDist);
  
  
 return float4(color, 1);
}

technique Chroma
{
 pass Pass1
 {
 VertexShader = compile vs_2_0 VertexShaderFunction();
 PixelShader = compile ps_2_0 PSChroma();
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