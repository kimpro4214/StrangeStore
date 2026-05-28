using Oculus.Interaction;
using UnityEngine;

public class GlassesController : MonoBehaviour
{
    [Header("Meta ±×·¦ ÄÄÆ÷³ÍÆ®")]
    [SerializeField] private GrabInteractable _grabInteractable;

    private bool hasDialogued = false;

    private void OnEnable() => _grabInteractable.WhenStateChanged += OnGrab;
    private void OnDisable() => _grabInteractable.WhenStateChanged -= OnGrab;


    public void OnGrab(InteractableStateChangeArgs args)
    {
        if (hasDialogued) return;
        if (args.NewState != InteractableState.Select) return;

        hasDialogued = true;
        DialogueGlasses.Instance.OnGrab();
    }
}
