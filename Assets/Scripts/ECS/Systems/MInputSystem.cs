using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace GamePlay
{
    public struct InputData: IComponentData
    {
        public float3 Move;
        public float3 Look;
        public bool Jump;
        public bool Sprint;
    }


    public class MInputSystem : SystemBase, InputSystemMaps.IPlayerActions
    {
        private InputSystemMaps ipmaps;
        // private EntityQuery inputData;
        private Vector2 move;
        private Vector2 look;
        private bool jump;
        private bool sprint;

        protected override void OnCreate()
        {
            base.OnCreate();

            ipmaps = new InputSystemMaps();
            ipmaps.Player.SetCallbacks(this);

            // inputData = GetEntityQuery(typeof(InputData));
        }

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            ipmaps.Enable();
        }

        protected override void OnStopRunning()
        {
            base.OnStopRunning();
            ipmaps.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            move = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            look = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            jump = context.ReadValueAsButton();
        }
        
        public void OnSprint(InputAction.CallbackContext context)
        {
            sprint = context.ReadValueAsButton();
        }

        protected override void OnUpdate()
        {
            // if (inputData.CalculateChunkCount() != 0)
            // {
            //     inputData.SetSingleton(
            //         new InputData
            //         {
            //             Move = new float3(move.x, 0.0f, move.y),
            //             Look = new float3(look.x, 0.0f, look.y),
            //             Jump = jump,
            //             Sprint = sprint,
            //         }
            //     );
            // }
            // var horizontal = Input.GetAxis("Horizontal");
            // var vertical = Input.GetAxis("Vertical");
            var _move = this.move;
            var _look = this.look;
            var _jump = this.jump;
            var _sprint = this.sprint;

            Entities.
            WithAll<PlayerTag>().
            WithName("Input").
            ForEach((ref InputData inputData) => {
                inputData.Move = new float3(_move.x, 0.0f, _move.y);
                inputData.Look = new float3(_look.x, 0.0f, _look.y);
                inputData.Jump = _jump;
                inputData.Sprint = _sprint;
            }).ScheduleParallel();
        }
    }
}

