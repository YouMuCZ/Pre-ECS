using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GamePlay
{
    struct PlayerCameraRootTag : IComponentData {}

    [DisallowMultipleComponent]
    public class PlayerCameraRoot : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Header("Cinemachine")]
        [Tooltip("相机向上角度")]
        public float TopClamp = 70.0f;
        [Tooltip("相机向下角度")]
        public float BottomClamp = -30.0f;
        [Tooltip("覆写摄像头时的额外角度,用于在锁定时微调相机位置.")]
        public float CameraAngleOverride = 0.0f;
        [Tooltip("锁定相机位置")]
        public bool LockCameraPosition = false;
        [Tooltip("当输入大于该阈值，转动相机视角")]
        public float Threshold = 0.01f;


        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddComponents(entity, new ComponentTypes(
                typeof(PlayerCameraRootTag),
                typeof(CinemachineComponent)
            ));

            dstManager.SetComponentData(entity, new CinemachineComponent {
                TopClamp = this.TopClamp, 
                BottomClamp = this.BottomClamp, 
                CameraAngleOverride = this.CameraAngleOverride,
                LockCameraPosition = this.LockCameraPosition,
                Threshold = this.Threshold
            });
        }
    }
}

