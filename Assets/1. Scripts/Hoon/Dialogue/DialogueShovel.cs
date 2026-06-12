using Oculus.Interaction.Samples;
using UnityEngine;

public class DialogueShovel : MonoBehaviour
{
    public static DialogueShovel Instance;
    public DialogueTyper typer;

    [Header("텍스트 꺼지는 딜레이")]
    public float enableDelay = 3f;

    [Header("삽 잡으면 보여줄 텍스트")]
    public string grabText = "트리거를 꾹 누르면 특정 땅이 파질 것 같은데..";
    public string failText = "이 곳은 파지지 않는 것 같다..";
    public string successText = "열쇠가 파졌다!!";

    public bool isFirst = true;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void OnGrab()
    {
        if (!isFirst) return;
        typer.ShowText(grabText, enableDelay);
        isFirst = false;
    }

    public void OnFail()
    {
        typer.ShowText(failText, enableDelay);
    }

    public void OnSuccess()
    {
        typer.ShowText(successText, enableDelay);
    }
}
