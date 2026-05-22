using UnityEngine;

public class LockItem : MonoBehaviour
{
    public bool unlocked = false;
    Animator animator;
    public KeyItem keyItem;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayUnlockAnimation()
    {
        animator.SetTrigger("Unlock");
    }

    public void DestroyLock()
    {
        if (keyItem != null) Destroy(keyItem);
        Destroy(this.gameObject);
    }
}
