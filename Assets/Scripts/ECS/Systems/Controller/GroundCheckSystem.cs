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

        protected override void OnUpdate()
        {
            var collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;
            var startOffset = new float3(0.0f, 0.9f, 0.0f);
            var endOffset = new float3(0.0f, 1.1f, 0.0f);
            
            Entities.ForEach((ref Translation translation, ref GroundCheckComponent groundCheck) => {

                var input = new RaycastInput
                {
                    Start = translation.Value - startOffset,
                    End = translation.Value - endOffset,
                    Filter = PhysicsCollisionFilters.GroundCheckFilter,
                };

                // 射线检测
                if (collisionWorld.CastRay(input, out Unity.Physics.RaycastHit hit))
                {
                    groundCheck.IsGrounded = true;
                }
                else
                {
                    groundCheck.IsGrounded = false;
                }

            }).Run();
        }

        // protected override void OnUpdate()
        // {

        //     var collisionWorld = buildPhysicsWorld.PhysicsWorld.CollisionWorld;
            
        //     Entities.ForEach((ref Translation translation, ref PhysicsCollider collider, ref GroundCheckComponent groundCheck, in GravityComponent gravityData) => {
        //         // 位置修正
        //         // var curPosition = translation.Value + new float3(0.0f, groundCheck.Epsilon, 0.0f)*-math.normalize(gravityData.Gravity);

        //         // CheckGrounded(ref collider, ref collisionWorld, ref groundCheck, translation.Value, gravityData);
        //     }).Run();
        // }

        // private unsafe static void CheckGrounded(ref PhysicsCollider collider, ref CollisionWorld collisionWorld, ref GroundCheckComponent groundCheck ,in float3 curPosition, in GravityComponent gravityData)
        // {
        //     // 画个盒子
        //     // var aabb = collider.ColliderPtr->CalculateAabb();
        //     // float mod = 0.15f;

        //     // float3 samplePos = curPosition + new float3(0.0f, aabb.Min.y, 0.0f);
        //     // float3 gravity = math.normalize(gravityData.Gravity);
        //     // float3 offset = (gravity * 0.1f);

        //     // float3 posLeft = samplePos - new float3(aabb.Extents.x * mod, 0.0f, 0.0f);
        //     // float3 posRight = samplePos + new float3(aabb.Extents.x * mod, 0.0f, 0.0f);
        //     // float3 posForward = samplePos + new float3(0.0f, 0.0f, aabb.Extents.z * mod);
        //     // float3 posBackward = samplePos - new float3(0.0f, 0.0f, aabb.Extents.z * mod);

        //     float3 samplePos = curPosition - new float3(0.0f, 0.9f, 0.0f);
        //     float3 offset = new float3(0.0f, 0.1f, 0.0f);

        //     var input = new RaycastInput
        //     {
        //         Start = samplePos,
        //         End = samplePos - offset,
        //         Filter = PhysicsCollisionFilters.GroundCheckFilter,
        //     };

        //     NativeList<Unity.Physics.RaycastHit> allHits = new NativeList<Unity.Physics.RaycastHit>(Allocator.Temp);

        //     if (collisionWorld.CastRay(input, ref allHits))
        //     {
        //         foreach (var entity in allHits)
        //         {
        //             groundCheck.IsGrounded = true;
        //             return;
        //         }
        //     }
        //     allHits.Dispose();

        //     groundCheck.IsGrounded = false;
        // }

    }

}

