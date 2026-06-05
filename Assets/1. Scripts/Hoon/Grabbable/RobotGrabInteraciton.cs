using Oculus.Interaction;
using UnityEngine;

public class RobotGrabInteraciton : MonoBehaviour
{
    [Header("Meta ½º³À ÄÄÆ÷³ÍÆ®")]
    [SerializeField] private GrabInteractable _grabInteractable;

    [Header("ÆÄÆ¼Å¬")]
    [SerializeField] GameObject particle;

    private Animator animator;
    private void OnEnable() => _grabInteractable.WhenStateChanged += OnGrab;
    private void OnDisable() => _grabInteractable.WhenStateChanged -= OnGrab;

    private bool _isGrabbed = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        particle.SetActive(true);
    }

    public void OnGrab(InteractableStateChangeArgs args)
    {
        _isGrabbed = (args.NewState == InteractableState.Select);
        if (_isGrabbed)
        {
            animator.SetTrigger("OnGrab");
            particle.SetActive(false);
        }
        else
        {
            animator.SetTrigger("OnRelease");
        }
    }
}
