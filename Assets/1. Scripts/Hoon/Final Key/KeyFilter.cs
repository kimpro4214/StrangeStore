using UnityEngine;
using Oculus.Interaction; // Meta SDK 필수

public class KeyFilter : MonoBehaviour, IGameObjectFilter
{
    // 테이블 영역에 물건이 들어올 때마다 유니티가 이 함수를 실행합니다.
    public bool Filter(GameObject gameObject)
    {
        // 들어온 오브젝트에 TradableItem 컴포넌트가 붙어있는지 확인합니다.
        KeyItem item = gameObject.GetComponent<KeyItem>();

        return item != null;
    }
}