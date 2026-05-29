using UnityEngine;
using Oculus.Interaction;

public class GrabMaterialChanger : MonoBehaviour
{
    [SerializeField] private GrabInteractable _grabInteractable;
    [SerializeField] private SkinnedMeshRenderer _renderer;
    [SerializeField] private Material[] _grabbedMaterial;

    private void OnEnable() => _grabInteractable.WhenStateChanged += HandleStateChanged;
    private void OnDisable() => _grabInteractable.WhenStateChanged -= HandleStateChanged;

    private void HandleStateChanged(InteractableStateChangeArgs args)
    {
        if (args.NewState == InteractableState.Select)
        {
            _renderer.materials = _grabbedMaterial;  // 배열 통째로 교체
            _grabInteractable.WhenStateChanged -= HandleStateChanged;
        }
    }
}