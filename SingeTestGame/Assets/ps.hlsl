cbuffer colorBuffer : register(b0)
{
	float4 col;
}

struct PS_IN
{
	float4 pos: SV_POSITION;
};

float4 main(PS_IN input) : SV_Target
{
	return col;
}