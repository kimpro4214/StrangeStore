using UnityEngine;

public class LockItem : MonoBehaviour
{
    public bool unlocked = false;
    Animator animator;
    [HideInInspector]
    public KeyItem keyItem;

    public GameObject _explosionParticle;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayUnlockAnimation()
    {
        animator.SetTrigger("Unlock");
    }

    // 흔들리는 애니메이션 직후 발동
    public void DestroyLock()
    {
        AudioManager.Instance.Play3D(SoundName.lock_explode, transform.position);
        Destroy(Instantiate(_explosionParticle, transform.position, Quaternion.identity), 3f);
        if (keyItem != null) Destroy(keyItem.gameObject);
        Destroy(this.gameObject);
    }
}
