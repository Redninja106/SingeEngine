struct PS_IN
{
};

struct PS_OUT
{
	float4 color : SV_Target;
};

PS_OUT main(PS_IN input)
{
	PS_OUT output;

	output.color = float4(1,0,0,1);

	return output;
}