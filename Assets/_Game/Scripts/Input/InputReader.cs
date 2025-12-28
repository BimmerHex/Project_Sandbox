using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace Game.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input/Input Reader")]
    public class InputReader : ScriptableObject, GameControls.IPlayerActions, GameControls.IUIActions
    {
        // --- GAMEPLAY EVENTS ---
        public event Action<Vector2> MoveEvent;
        public event Action<Vector2> LookEvent;
        public event Action AttackEvent;
        public event Action InteractEvent; // Performed (Hold bittiğinde veya tıklandığında)
        public event Action InteractCancelledEvent; // Hold sırasında bırakılırsa
        public event Action JumpEvent;
        public event Action JumpCancelledEvent;
        public event Action SprintEvent;
        public event Action SprintCancelledEvent;
        public event Action CrouchEvent;
        public event Action NextItemEvent;
        public event Action PreviousItemEvent;

        // --- UI EVENTS ---
        // UI eventlerini şimdilik genel tutuyoruz, genelde Unity EventSystem otomatik halleder
        // ama özel menü kontrolleri için burayı kullanabiliriz.
        public event Action PauseEvent; // Menu açma isteği (ESC/Start)

        private GameControls _gameControls;

        private void OnEnable()
        {
            if (_gameControls == null)
            {
                _gameControls = new GameControls();
                _gameControls.Player.SetCallbacks(this);
                _gameControls.UI.SetCallbacks(this);
            }
            
            // Varsayılan olarak Gameplay moduyla başla
            EnableGameplayInput();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        public void EnableGameplayInput()
        {
            _gameControls.UI.Disable();
            _gameControls.Player.Enable();
        }

        public void EnableUIInput()
        {
            _gameControls.Player.Disable();
            _gameControls.UI.Enable();
        }

        public void DisableAllInput()
        {
            _gameControls.Player.Disable();
            _gameControls.UI.Disable();
        }

        #region Player Actions Implementation

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            LookEvent?.Invoke(context.ReadValue<Vector2>());
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                AttackEvent?.Invoke();
            }
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            // Interact 'Hold' interaction içerdiği için:
            // Performed: Hold süresi doldu (Başarılı etkileşim)
            // Canceled: Hold süresi dolmadan bırakıldı
            
            if (context.phase == InputActionPhase.Performed)
            {
                InteractEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                InteractCancelledEvent?.Invoke();
            }
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                JumpEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                JumpCancelledEvent?.Invoke();
            }
        }

        public void OnSprint(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                SprintEvent?.Invoke();
            }
            else if (context.phase == InputActionPhase.Canceled)
            {
                SprintCancelledEvent?.Invoke();
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                CrouchEvent?.Invoke();
            }
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                NextItemEvent?.Invoke();
            }
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                PreviousItemEvent?.Invoke();
            }
        }

        #endregion

        #region UI Actions Implementation

        public void OnNavigate(InputAction.CallbackContext context) { }
        public void OnSubmit(InputAction.CallbackContext context) { }
        public void OnCancel(InputAction.CallbackContext context) 
        {
            // UI modunda 'Cancel' (ESC/B) genelde menüyü kapatır veya geri gider.
            // Ancak Gameplay modunda 'Pause' ihtiyacımız var.
            // GameControls dosyasında Player haritasında Pause yoktu.
            // İleride burayı güncelleyebiliriz.
        }
        public void OnPoint(InputAction.CallbackContext context) { }
        public void OnClick(InputAction.CallbackContext context) { }
        public void OnRightClick(InputAction.CallbackContext context) { }
        public void OnMiddleClick(InputAction.CallbackContext context) { }
        public void OnScrollWheel(InputAction.CallbackContext context) { }
        public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }
        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }

        #endregion
    }
}