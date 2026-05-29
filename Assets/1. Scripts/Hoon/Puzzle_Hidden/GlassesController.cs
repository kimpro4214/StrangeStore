using Oculus.Interaction;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class GlassesController : MonoBehaviour
{
    [Header("Meta ±×·¦ ÄÄÆ÷³ÍÆ®")]
    [SerializeField] private GrabInteractable _grabInteractable;

    [SerializeField] private GameObject particle;

    private bool hasDialogued = false;

    private void OnEnable() => _grabInteractable.WhenStateChanged += OnGrab;
    private void OnDisable() => _grabInteractable.WhenStateChanged -= OnGrab;


    public void OnGrab(InteractableStateChangeArgs args)
    {
        if (hasDialogued) return;
        if (args.NewState != InteractableState.Select) return;

        particle.SetActive(false);
        hasDialogued = true;
        DialogueGlasses.Instance.OnGrab();
    }
}
