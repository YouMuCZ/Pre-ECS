using Unity.Entities;
using Unity.Mathematics;

namespace GamePlay
{
    public struct MovementComponent: IComponentData
    {
        public float MoveSpeed;
		public float SprintSpeed;
		public float RotationSmoothTime;
		public float SpeedChangeRate;
		public float JumpHeight;
		public float JumpTimeout;
    }

    public struct GravityComponent: IComponentData
    {
        public float3 Gravity;
        public float3 VerticalVelocity;
        public float3 TerminalVelocity;
        public float FallTimeout;
        public float FallTimeoutDelta;
    }

    public struct GroundCheckComponent : IComponentData
    {
        public bool Grounded;
        public float GroundedOffset;
        public float GroundedRadius;
    }
}
