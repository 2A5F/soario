#include "./bindless.hlsl"

struct RootData 
{
    float4x4 mat;
};

struct VertexOut
{
    float4 PositionHS : SV_Position;
};

[NumThreads(4, 1, 1)]
[OutputTopology("triangle")]
void mesh(
    uint gtid : SV_GroupThreadID,
    uint gid : SV_GroupID,
    out indices uint3 tris[2],
    out vertices VertexOut verts[4]
)
{
    SetMeshOutputCounts(4, 2);

    if (gtid < 2) 
    {
        float3 is[2] = { float3(1, 2, 0), float3(2, 1, 3) };

        tris[gtid] = is[gtid];
    }

    if (gtid < 4) 
    {
        ConstantBuffer<RootData> root = LoadRoot< ConstantBuffer<RootData> >();

        VertexOut o;

        float3 vs[4] = { float3(0, 0, 0), float3(1, 0, 0), float3(0, 1, 0), float3(1, 1, 0) };
        float3 pos = vs[gtid];
        o.PositionHS = mul(root.mat, float4(pos, 1));

        verts[gtid] = o;
    }
}

float4 pixel(VertexOut input) : SV_TARGET {
    return float4(0, 0, 1, 1);
}
