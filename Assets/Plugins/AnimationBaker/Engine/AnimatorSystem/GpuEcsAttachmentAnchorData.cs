using System;
using Unity.Mathematics;
using UnityEngine;

namespace AnimationBaker.AnimatorSystem
{
    [Serializable]
    public class GpuEcsAttachmentAnchorData : ScriptableObject
    {
        public float4x4[] anchorTransforms;
    }
}