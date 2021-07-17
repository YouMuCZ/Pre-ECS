using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace GamePlay
{
    [UpdateAfter(typeof(MovementSystem))]
    public class PlayerCameraRootFollowSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var playerPosition = new float3(0.0f, 0.0f, 0.0f);
            
            // 1. 获取玩家实时位置
            Entities.WithAll<PlayerTag>().ForEach((ref Translation translation) => {
                playerPosition = translation.Value;
            }).Run();

            // 2. 设置Root的Entity位置
            Entities.WithAll<PlayerCameraRootTag>().ForEach((ref Translation translation) => {
                translation.Value = playerPosition;
            }).Run();

            // 3. 设置Root的Gameobject位置
            Entities.WithoutBurst().WithAll<PlayerCameraRootTag>().ForEach((Transform transform) => {
                transform.position = playerPosition;
            }).Run();
        }
    }
}

