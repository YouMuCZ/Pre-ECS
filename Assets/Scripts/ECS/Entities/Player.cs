using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace GamePlay
{
    // Tag used to quickly identify our Player Entity
    struct PlayerTag : IComponentData {}


    [DisallowMultipleComponent]
    public class Player : MonoBehaviour, IConvertGameObjectToEntity
    {
        [Header("Controller")]
		[Tooltip("Move speed of the character in m/s")]
        public float MoveSpeed = 5.0f;
		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 5.335f;
		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;
        [Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;
        [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.50f;

        [Header("Gravity")]
		public float3 Gravity = new float3(0.0f, -15f, 0.0f);
        public float3 VerticalVelocity = new float3(0.0f, 0.0f, 0.0f);
        public float3 TerminalVelocity = new float3(0.0f, 53.0f, 0.0f);
        [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;
        
        [Header("Grounded")]
		[Tooltip("角色是否在地面")]
		public bool IsGrounded = true;
		[Tooltip("粗糙地面,偏置值")]
		public float Epsilon = 0.001f;
        [Tooltip("碰撞检测盒修正比率")]
        public float Modify = 0.5f;
        [Tooltip("射线长度偏置值")]
        public float3 Offset = new float3(0.0f, 0.1f, 0.0f);
        
        /// <summary>
        /// A function which converts our Player authoring GameObject to a more optimized Entity representation
        // Call methods on 'dstManager' to create runtime components on 'entity' here. Remember that:
        //
        // * You can add more than one component to the entity. It's also OK to not add any at all.
        //
        // * If you want to create more than one entity from the data in this class, use the 'conversionSystem'
        //   to do it, instead of adding entities through 'dstManager' directly.
        //
        // For example,
        //   dstManager.AddComponentData(entity, new Unity.Transforms.Scale { Value = scale });
        /// </summary>
        /// <param name="entity">A reference to the entity this GameObject will become</param>
        /// <param name="dstManager">The EntityManager is used to make changes to Entity data.</param>
        /// <param name="conversionSystem">Used for more advanced conversion features. Not used here.</param>
        public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            if (!enabled) return;

            // Here we add all of the components needed by the player
            dstManager.AddComponents(entity, new ComponentTypes(
                typeof(PlayerTag)
            ));

            dstManager.AddComponents(entity, new ComponentTypes(
                typeof(InputData),
                typeof(GravityComponentData),
                typeof(GroundCheckComponentData),
                typeof(CinemachineComponent),
                typeof(MovementComponentData)
            ));

            // Set the movement data from the authoring component
            var movementComponent = new MovementComponentData
            {
                MoveSpeed = MoveSpeed,
                SprintSpeed = SprintSpeed,
                RotationSmoothTime = RotationSmoothTime,
                SpeedChangeRate = SpeedChangeRate,
                JumpHeight = JumpHeight,
                JumpTimeout = JumpTimeout,
            };

            var gravityComponent = new GravityComponentData
            {
                Gravity = Gravity,
                FallTimeout = FallTimeout,
                FallTimeoutDelta = FallTimeout,
                VerticalVelocity = VerticalVelocity,
                TerminalVelocity = TerminalVelocity,
            };

            var groundCheckComponent = new GroundCheckComponentData
            {
                IsGrounded = IsGrounded,
                Epsilon = Epsilon,
                Modify = Modify,
                Offset = Offset,
            };
            
            dstManager.SetComponentData(entity, gravityComponent);
            dstManager.SetComponentData(entity, movementComponent);
            dstManager.SetComponentData(entity, groundCheckComponent);
        }
    }
}

