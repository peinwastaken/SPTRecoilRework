﻿using UnityEngine;

namespace PeinRecoilRework.Helpers
{
    public class RecoilSpring
    {
        public float Damping = 0.8f;
        public float Stiffness = 0.2f;
        public float Speed = 1.0f;

        public Vector3 Target = Vector3.zero;
        public Vector3 Position = Vector3.zero;
        public Vector3 Velocity = Vector3.zero;

        public Vector3 Update(float deltaTime)
        {
            float dtScaled = deltaTime * Speed;

            Vector3 displacement = Target - Position;
            Vector3 force = displacement * Stiffness;

            Velocity += force * dtScaled;
            Velocity *= Mathf.Pow(Damping, dtScaled);
            Position += Velocity * dtScaled;

            return Position;
        }

        public void SnapTo(Vector3 newPosition)
        {
            Position = newPosition;
            Velocity = Vector3.zero;
        }

        public void ApplyImpulse(Vector3 dir)
        {
            Velocity = dir;
        }
    }
}
