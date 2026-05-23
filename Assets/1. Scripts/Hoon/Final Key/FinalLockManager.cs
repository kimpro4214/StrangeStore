using Oculus.Interaction;
using UnityEngine;

public class FinalLockManager : MonoBehaviour
{
    public static FinalLockManager instance;
    public LockItem[] _finalKeys;

    [Header("클리어 시 열릴 문")]
    public Door leftDoor;
    public Door rightDoor;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // 열기 요청 받은 자물쇠 열기
    public void Unlock(LockItem lockItem, KeyItem key)
    {
        Debug.Log("KeyLock Unlock");
        lockItem.unlocked = true;
        lockItem.keyItem = key;
        lockItem.PlayUnlockAnimation();
        if (key != null) AudioManager.Instance.Play3D(SoundName.lock_open, key.transform.position);
        CheckAllUnlocked();
    }

    private void CheckAllUnlocked()
    {
        bool isAll = true;
        foreach (LockItem lockItem in _finalKeys)
        {
            if (!lockItem.unlocked) isAll = false;
        }

        if (isAll)
        {
            OnClear();
        }
    }

    private void OnClear()
    {
        Debug.Log("Clear");
        StartCoroutine(leftDoor.OpenDoor());
        StartCoroutine(rightDoor.OpenDoor());
    }
}
