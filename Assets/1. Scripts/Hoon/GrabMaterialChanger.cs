using UnityEngine;
using Oculus.Interaction;

public class GrabMaterialChanger : MonoBehaviour
{
    [SerializeField] private GrabInteractable _grabInteractable;
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Material _normalMaterial;
    [SerializeField] private Material _grabbedMaterial;

    private void OnEnable() => _grabInteractable.WhenStateChanged += HandleStateChanged;
    private void OnDisable() => _grabInteractable.WhenStateChanged -= HandleStateChanged;

    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        if (args.NewState == InteractableState.Select)
        {
            _renderer.material = _grabbedMaterial;
            _grabInteractable.WhenStateChanged -= HandleStateChanged;  // 한 번 후 구독 해제
        }
    }
}