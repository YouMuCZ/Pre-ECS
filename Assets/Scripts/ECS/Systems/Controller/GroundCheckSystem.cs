using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Physics.GraphicsIntegration;
using Unity.Physics.Systems;
using Unity.Transforms;


namespace GamePlay
{
    [UpdateAfter(typeof(GravitySystem))]
    public class GroundCheckSystem : SystemBase
    {
        private BuildPhysicsWorld buildPhysicsWorld;

        protected override void OnCreate()
        {
            base.OnCreate();

            buildPhysicsWorld = World.GetOrCreateSystem<BuildPhysicsWorld>();
        }

        // /// <summary>
        // /// 1. 简单射线检测，从entity中心向下发射射线
        // /// </summary>
        // protected override void OnUpdate()
        // {
        //     var collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;
        //     var startOffset = new float3(0.0f, 0.9f, 0.0f);
        //     var endOffset = new float3(0.0f, 1.1f, 0.0f);
            
        //     Entities.ForEach((ref Translation translation, ref GroundCheckComponent groundCheck) => {

        //         var input = new RaycastInput
        //         {
        //             Start = translation.Value - startOffset,
        //             End = translation.Value - endOffset,
        //             Filter = PhysicsCollisionFilters.CharacterGroundCheckFilter,
        //         };

        //         // 射线检测
        //         if (collisionWorld.CastRay(input, out Unity.Physics.RaycastHit hit))
        //         {
        //             groundCheck.IsGrounded = true;
        //         }
        //         else
        //         {
        //             groundCheck.IsGrounded = false;
        //         }

        //     }).Run();
        // }

        /// <summary>
        /// 2. 多射线检测法，构建AABB盒，在盒子底部发射多条射线进行检测
        /// </summary>
        protected unsafe override void OnUpdate()
        {

            var collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;
            
            Entities.ForEach((ref Translation translation, ref PhysicsCollider collider, ref GroundCheckComponentData ground) => 
            {
                Aabb aabb   = collider.ColliderPtr->CalculateAabb();

                float3 cPos = translation.Value + new float3(0.0f, aabb.Min.y, 0.0f);
                float3 lPos = cPos - new float3(aabb.Extents.x * ground.Modify, 0.0f, 0.0f);
                float3 rPos = cPos + new float3(aabb.Extents.x * ground.Modify, 0.0f, 0.0f);
                float3 fPos = cPos + new float3(0.0f, 0.0f, aabb.Extents.z * ground.Modify);
                float3 bPos = cPos - new float3(0.0f, 0.0f, aabb.Extents.z * ground.Modify);

                ground.IsGrounded = 
                (
                    PhysicsUtils.CastRay(out RaycastHit cHit, cPos, cPos - ground.Offset, ref collisionWorld, Entity.Null, PhysicsCollisionFilters.CharacterGroundFilter) ||
                    PhysicsUtils.CastRay(out RaycastHit lHit, lPos, lPos - ground.Offset, ref collisionWorld, Entity.Null, PhysicsCollisionFilters.CharacterGroundFilter) ||
                    PhysicsUtils.CastRay(out RaycastHit rHit, rPos, rPos - ground.Offset, ref collisionWorld, Entity.Null, PhysicsCollisionFilters.CharacterGroundFilter) ||
                    PhysicsUtils.CastRay(out RaycastHit fHit, fPos, fPos - ground.Offset, ref collisionWorld, Entity.Null, PhysicsCollisionFilters.CharacterGroundFilter) ||
                    PhysicsUtils.CastRay(out RaycastHit bHit, bPos, bPos - ground.Offset, ref collisionWorld, Entity.Null, PhysicsCollisionFilters.CharacterGroundFilter)
                );
            }).Run();

        }

    }

}

