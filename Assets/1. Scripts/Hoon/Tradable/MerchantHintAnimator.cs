using System.Collections;
using UnityEngine;

public class MerchantHintAnimator : MonoBehaviour
{
    private static readonly int VictoryHash = Animator.StringToHash("Victory");
    private static readonly int StandingGreetingHash = Animator.StringToHash("Standing Greeting");
    private static readonly int SpinningHash = Animator.StringToHash("Spinning");
    private static readonly int IdleHash = Animator.StringToHash("SharkmanIdle");

    [SerializeField] private Animator animator;
    [SerializeField] private float transitionDuration = 0.1f;

    private Coroutine hintRoutine;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    public void PlayHintSequence()
    {
        if (animator == null) return;

        if (hintRoutine != null)
        {
            StopCoroutine(hintRoutine);
        }

        hintRoutine = StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        yield return PlayState(VictoryHash);
        yield return PlayState(StandingGreetingHash);
        yield return PlayState(SpinningHash);

        animator.CrossFadeInFixedTime(IdleHash, transitionDuration, 0, 0f);
        hintRoutine = null;
    }

    private IEnumerator PlayState(int stateHash)
    {
        animator.CrossFadeInFixedTime(stateHash, transitionDuration, 0, 0f);
        yield return null;

        while (!IsCurrentState(stateHash))
        {
            yield return null;
        }

        while (IsCurrentState(stateHash))
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (!animator.IsInTransition(0) && stateInfo.normalizedTime >= 0.98f)
            {
                yield break;
            }

            yield return null;
        }
    }

    private bool IsCurrentState(int stateHash)
    {
        return animator.GetCurrentAnimatorStateInfo(0).shortNameHash == stateHash;
    }
}
