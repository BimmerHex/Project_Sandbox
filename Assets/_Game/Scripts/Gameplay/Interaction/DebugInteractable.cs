// Assets/_Game/Scripts/Gameplay/Interaction/DebugInteractable.cs
using UnityEngine;

namespace Game.Gameplay.Interaction
{
    public class DebugInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string _promptMessage = "Press [E] to Change Color";
        
        private MeshRenderer _renderer;

        public string InteractionPrompt => _promptMessage;

        private void Awake()
        {
            _renderer = GetComponent<MeshRenderer>();
        }

        public bool Interact(MonoBehaviour interactor)
        {
            // Rastgele renk ata
            Color randomColor = new Color(Random.value, Random.value, Random.value);
            _renderer.material.color = randomColor;
            
            Debug.Log($"Interaction Successful! New Color: {randomColor}");
            return true;
        }
    }
}