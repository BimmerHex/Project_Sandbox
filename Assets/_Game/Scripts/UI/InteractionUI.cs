// Assets/_Game/Scripts/UI/InteractionUI.cs
using UnityEngine;
using TMPro; // TextMeshPro kullanımı zorunludur
using Game.Gameplay.Interaction;
using Game.Gameplay.Player;

namespace Game.UI
{
    public class InteractionUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInteractor _playerInteractor;
        [SerializeField] private GameObject _interactionPanel;
        [SerializeField] private TextMeshProUGUI _promptText;

        private void Awake()
        {
            // Başlangıçta paneli gizle
            _interactionPanel.SetActive(false);
        }

        private void OnEnable()
        {
            if (_playerInteractor != null)
            {
                _playerInteractor.OnInteractableFound += ShowPrompt;
                _playerInteractor.OnInteractableLost += HidePrompt;
            }
        }

        private void OnDisable()
        {
            if (_playerInteractor != null)
            {
                _playerInteractor.OnInteractableFound -= ShowPrompt;
                _playerInteractor.OnInteractableLost -= HidePrompt;
            }
        }

        private void ShowPrompt(IInteractable interactable)
        {
            _promptText.text = interactable.InteractionPrompt;
            _interactionPanel.SetActive(true);
        }

        private void HidePrompt()
        {
            _interactionPanel.SetActive(false);
        }
    }
}