// Assets/_Game/Scripts/Gameplay/Interaction/IInteractable.cs
using UnityEngine;

namespace Game.Gameplay.Interaction
{
    public interface IInteractable
    {
        // Oyuncu objeye baktığında UI'da çıkacak metin (Örn: "Kapıyı Aç", "İncele")
        string InteractionPrompt { get; }
        
        // Etkileşim gerçekleştiğinde çalışacak ana fonksiyon
        // 'interactor' parametresi, etkileşimi kimin başlattığını (Player) bildirir.
        bool Interact(MonoBehaviour interactor);
    }
}