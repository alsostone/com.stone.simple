using Hash128 = Unity.Entities.Hash128;
using Unity.Mathematics;
using Unity.Entities;

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

namespace Rukhanka
{
public struct SkinnedMeshBoneInfo
{
#if RUKHANKA_DEBUG_INFO
	public BlobString name;
#endif
	public Hash128 hash;
	public float4x4 bindPose;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public struct BlendShapeInfo
{
#if RUKHANKA_DEBUG_INFO
	public BlobString name;
#endif
	public uint hash;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public struct SkinnedMeshInfoBlob
{
#if RUKHANKA_DEBUG_INFO
	public BlobString skeletonName;
	public float bakingTime;
#endif
	public Hash128 hash;
	public BlobArray<SkinnedMeshBoneInfo> bones;
	public BlobArray<BlendShapeInfo> blendShapes;
}

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

public struct SkinnedMeshBlendShapeMap
{
	public BlobArray<int2> perfectHashSeeds;
	public BlobArray<int> blendShapeIndices;
}

}
