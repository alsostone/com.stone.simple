using Unity.Entities;

namespace AnimationBaker.AnimatorSystem
{
    public struct GpuEcsAttachmentComponent : IComponentData
    {
        public Entity gpuEcsAnimatorEntity;
        public int attachmentAnchorId;
    }
}