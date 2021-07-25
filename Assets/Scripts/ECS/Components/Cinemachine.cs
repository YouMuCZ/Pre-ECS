using Unity.Entities;
using Unity.Mathematics;

namespace GamePlay
{
    /// <summary>
    /// 相机参数
    /// </summary>
    public struct CinemachineComponent : IComponentData
    {
        public float TopClamp;
		public float BottomClamp;
		public float CameraAngleOverride;
		public bool  LockCameraPosition;

        public float CinemachineTargetYaw;
        public float CinemachineTargetPitch;
        public quaternion CinemachineTargetRotation;

        public float Threshold;
    }
}

