using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

namespace StarRagBrawl
{
    [Serializable]
    public struct Mass : IComponentData
    {
       public float Value;
    }
}