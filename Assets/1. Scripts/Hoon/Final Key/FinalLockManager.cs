using Oculus.Interaction;
using UnityEngine;

public class FinalLockManager : MonoBehaviour
{
    public static FinalLockManager instance;
    public LockItem[] _finalKeys;

    [Header("클리어 시 열릴 문")]
    public LeftDoor leftDoor;
    public RightDoor rightDoor;


    private void Awake()
    {
        if (instance == null) instance = this;
    }

    // 열기 요청 받은 자물쇠 열기
    public void Unlock(LockItem lockItem, KeyItem key)
    {
        Debug.Log("KeyLock Unlock");
        if (lockItem != null)
        {
            lockItem.unlocked = true;
            lockItem.keyItem = key;
            lockItem.PlayUnlockAnimation();
            CheckAllUnlocked();
        }
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
