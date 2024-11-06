using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CrazyGames24
{
    public class BTBInputManager : MonoBehaviour, BTB_InputSystem.IGameplayActions
    {
        private BTB_InputSystem _inputSystem;

        private void Awake()
        {
            _inputSystem = new BTB_InputSystem();
            _inputSystem.Gameplay.SetCallbacks(this);
            _inputSystem.Gameplay.Enable();
        }

        private void OnDestroy()
        {
            _inputSystem.Gameplay.Disable();
        }

        public Action OnTriggerBeatPerformed;

        public void OnTriggerBeat(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnTriggerBeatPerformed?.Invoke();
            }
        }
    }
}