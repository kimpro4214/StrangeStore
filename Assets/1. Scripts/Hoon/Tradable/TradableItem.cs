using Oculus.Interaction;
using UnityEngine;

public enum ItemType { Apple, Money, MusicBox, Dumbbell, Fish }

public class TradableItem : MonoBehaviour
{
    public ItemType type;

    [SerializeField] private GameObject particle;
    private GrabInteractable _grabInteractable;
    private void Awake()
    {
        _grabInteractable = GetComponent<GrabInteractable>();
    }
    private void OnEnable() => _grabInteractable.WhenStateChanged += OffParticle;
    private void OnDisable()
    {
        _grabInteractable.WhenStateChanged -= OffParticle;
    }

    public void OffParticle(InteractableStateChangeArgs args)
    {
        if (args.NewState != InteractableState.Select) return;
        if (particle != null) particle.SetActive(false);
        _grabInteractable.WhenStateChanged -= OffParticle;
    }
}