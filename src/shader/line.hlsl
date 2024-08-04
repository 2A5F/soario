struct VertexOut
{
   float4 PositionHS   : SV_Position;
   float3 PositionVS   : POSITION0;
   float3 Normal       : NORMAL0;
   uint   MeshletIndex : COLOR0;
};

cbuffer Foo
{       
    float4 foo;
}

[NumThreads(128, 1, 1)]
[OutputTopology("triangle")]
void mesh(
   uint gtid : SV_GroupThreadID,
   uint gid : SV_GroupID,
   out indices uint3 tris[126],
   out vertices VertexOut verts[64]
)
{
   SetMeshOutputCounts(0, 0);

   tris[gtid] = int3(0, 0, 0);
   VertexOut o;
   o.Normal = foo.xyz;
   verts[gtid] = o;
}

float4 pixel(VertexOut input) : SV_TARGET {
   return 1;
}
