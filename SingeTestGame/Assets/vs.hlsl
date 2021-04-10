cbuffer MATRIX_BUFFER : register(b0)
{
	float4x4 world;
	float4x4 view;
	float4x4 proj;
};

struct VS_IN
{
	float3 pos : POSITION;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
};

PS_IN main(VS_IN input)
{
	PS_IN output;

	output.pos = float4(input.pos.xyz, 1);
	output.pos = mul(world, output.pos);
	output.pos = mul(output.pos, view);
	output.pos = mul(output.pos, proj);

	return output;
}