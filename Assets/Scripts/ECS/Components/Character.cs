using Unity.Entities;
using Unity.Mathematics;

namespace GamePlay
{
    public struct MovementComponentData: IComponentData
    {
        public float MoveSpeed;
		public float SprintSpeed;
		public float RotationSmoothTime;
		public float SpeedChangeRate;
		public float JumpHeight;
		public float JumpTimeout;
    }

    public struct GravityComponentData: IComponentData
    {
        public float3 Gravity;
        public float3 VerticalVelocity;
        public float3 TerminalVelocity;
        public float FallTimeout;       // 下落延迟重置值
        public float FallTimeoutDelta;  // 下落延迟，主要用于动画机表现
    }

    /// <summary>
    /// 地面检测组件
    /// </summary>
    public struct GroundCheckComponentData : IComponentData
    {
        public bool IsGrounded;
        public float Epsilon;
        public float Modify;
        public float3 Offset;
    }
}
