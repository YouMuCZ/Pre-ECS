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
        public bool    Jump;
        public bool    Sprint;
        public Vector2 Move;
        public Vector2 Look;

        public bool    AnalogMovement;
    }

    public class MInputSystem : SystemBase, InputSystemMaps.IPlayerActions
    {
        private bool            jumpInput;
        private bool            sprintInput;
        private Vector2         moveInput;
        private Vector2         lookInput;
        private InputSystemMaps ipmaps;

#if !UNITY_IOS || !UNITY_ANDROID
		private bool            cursorLocked = true;
		private bool            cursorInputForLook = true;
#endif

        protected override void OnCreate()
        {
            base.OnCreate();

            ipmaps = new InputSystemMaps();
            ipmaps.Player.SetCallbacks(this);

            OnApplicationFocus(true);
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
            moveInput = context.ReadValue<Vector2>();
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            if(cursorInputForLook)
			{
                lookInput = context.ReadValue<Vector2>();
			}
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            jumpInput = context.ReadValueAsButton();
        }
        
        public void OnSprint(InputAction.CallbackContext context)
        {
            sprintInput = context.ReadValueAsButton();
        }

#if !UNITY_IOS || !UNITY_ANDROID
		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
#endif

        protected override void OnUpdate()
        {
            var move = moveInput;
            var look = lookInput;
            var jump = jumpInput;
            var sprint = sprintInput;

            Entities.WithAll<PlayerTag>().WithName("Input").
            ForEach((ref InputData inputData) => {
                inputData.Move = move;
                inputData.Look = look;
                inputData.Jump = jump;
                inputData.Sprint = sprint;
            }).ScheduleParallel();
        }
    }
}

