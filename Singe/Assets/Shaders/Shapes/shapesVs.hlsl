cbuffer ProjectionMatrix
{
	float3x2 proj;
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

	output.position = float4(mul(input.position, proj), 1, 1);
	//output.uv = input.position % 1.0;

	return output;
}