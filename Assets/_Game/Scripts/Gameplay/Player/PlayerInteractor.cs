// Assets/_Game/Scripts/Gameplay/Player/PlayerInteractor.cs
using UnityEngine;
using System;
using Game.Input;
using Game.Gameplay.Interaction;

namespace Game.Gameplay.Player
{
    public class PlayerInteractor : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private InputReader _inputReader;
        [SerializeField] private Transform _cameraRoot;

        [Header("Configuration")]
        [SerializeField] private float _interactionDistance = 3.0f;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private float _checkInterval = 0.05f; // Performans için her frame değil, belirli aralıkla Ray atacağız.

        // Events (UI'ın dinleyeceği olaylar)
        public event Action<IInteractable> OnInteractableFound; // Bir obje bulundu
        public event Action OnInteractableLost; // Obje kaybedildi (Uzaklaşıldı veya bakış kaçırıldı)

        private IInteractable _currentInteractable;
        private float _lastCheckTime;
        private RaycastHit _hitInfo;

        private void OnEnable()
        {
            if (_inputReader != null)
                _inputReader.InteractEvent += OnInteractPerformed;
        }

        private void OnDisable()
        {
            if (_inputReader != null)
                _inputReader.InteractEvent -= OnInteractPerformed;
        }

        private void Update()
        {
            // Raycast işlemini optimize etmek için zamanlayıcı kontrolü
            if (Time.time - _lastCheckTime > _checkInterval)
            {
                _lastCheckTime = Time.time;
                PerformRaycast();
            }
        }

        private void PerformRaycast()
        {
            // Kameranın merkezinden ileriye doğru ışın yolla
            Ray ray = new Ray(_cameraRoot.position, _cameraRoot.forward);

            // Raycast atıyoruz. Sadece _interactionLayer katmanındaki objelere çarpar.
            bool hitSomething = Physics.Raycast(
                ray, 
                out _hitInfo, 
                _interactionDistance, 
                _interactionLayer
            );

            if (hitSomething)
            {
                // Çarptığımız objede IInteractable arayüzü var mı?
                IInteractable interactable = _hitInfo.collider.GetComponent<IInteractable>();

                if (interactable != null)
                {
                    // Eğer baktığımız obje değiştiyse (yeni bir obje ise)
                    if (_currentInteractable != interactable)
                    {
                        _currentInteractable = interactable;
                        OnInteractableFound?.Invoke(_currentInteractable); // UI'a haber ver
                    }
                    return;
                }
            }

            // Buraya geldiysek: Ya hiçbir şeye çarpmadık ya da çarptığımız şey interactable değil.
            if (_currentInteractable != null)
            {
                _currentInteractable = null;
                OnInteractableLost?.Invoke(); // UI'a haber ver: "Yazıyı gizle"
            }
        }

        private void OnInteractPerformed()
        {
            if (_currentInteractable != null)
            {
                _currentInteractable.Interact(this);
            }
        }

        // Editor'de Ray'i görebilmek için Debug aracı
        private void OnDrawGizmos()
        {
            if (_cameraRoot != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawRay(_cameraRoot.position, _cameraRoot.forward * _interactionDistance);
            }
        }
    }
}