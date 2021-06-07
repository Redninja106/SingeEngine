cbuffer ProjectionMatrix
{
	float3x2 proj;
	float2 filler;
};

struct VS_IN
{
	float2 position : POSITION;
};

struct PS_IN
{
	float4 position : SV_Position;
};

PS_IN main(VS_IN input)
{
	PS_IN output;

	//float3x2 identity = { 1, 0, 0, 1, 0, 0 };
	input.position = mul(proj, input.position);
	output.position = float4(input.position, 1, 1);
	//output.uv = input.position % 1.0;

	return output;
}