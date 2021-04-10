cbuffer INPUT_BUFFER: register(b0)
{
	float2 size;
	float3 camDir;
	float3 camPos;
};

struct PS_IN
{
	float4 pos : SV_Position;
};

struct PS_OUT
{
	float4 color : SV_Target;
};

float sdfSphere(float3 pos, float4 sphere)
{
	return length(sphere.xyz - pos) - sphere.z;
}
float march(float3 dir, float3 pos)
{
	float cdist = 1;
	float3 cpos = pos;
	while (cdist > 0.01 && cdist < 10)
	{
		cdist = sdfSphere(cpos, float4(0,0,2,.5));
		cpos += cdist * dir;
	}
	if (cdist < 0.01)
	{
		return 1;
	}
	else
	{
		return 0;
	}
}


PS_OUT main(PS_IN psin)
{
	PS_OUT psout;

	float2 uv = psin.pos.xy / size;

	float2 uvn = uv * .5 - float2(1, 1);

	float3 dir = normalize(camDir + float3(uvn, 0));

	float hit = march(dir, camPos);

	psout.color = float4(hit, hit, hit, 255);
	
	return psout;
}