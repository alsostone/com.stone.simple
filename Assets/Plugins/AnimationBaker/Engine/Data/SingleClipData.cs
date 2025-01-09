using System;
using UnityEngine;

namespace AnimationBaker.Data
{
    [Serializable]
    public struct SingleClipData
    {
        [Tooltip("Only used to determine the length of the animation")]
        public AnimationClip animationClip;
    }
}