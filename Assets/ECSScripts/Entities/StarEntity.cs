using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace StarRagBrawl
{
    public struct StarEntity : IComponentData
    {
        public Position position;
        public Speed speed;
        public Acceleration acceleration;
        public Mass mass;
    }

}