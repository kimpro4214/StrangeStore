using UnityEngine;

public class SecurityRoomTrigger : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";   // 플레이어 식별

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        SecurityCinemachine.Instance.StartSequence();
    }
}