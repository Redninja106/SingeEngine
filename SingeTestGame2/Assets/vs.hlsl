struct VS_IN
{
	float2 pos : POSITION;
};

struct PS_IN
{
	float4 pos : SV_Position;
};

PS_IN main(VS_IN vsin)
{
	PS_IN psin;
	psin.pos = float4(vsin, 0, 1);
	return psin;
}