using Oculus.Interaction.Samples;
using UnityEngine;

public class DialogueGlasses : MonoBehaviour
{
    public static DialogueGlasses Instance;
    public DialogueTyper typer;

    [Header("텍스트 꺼지는 딜레이")]
    public float enableDelay = 2.2f;

    [Header("돋보기 잡으면 보여줄 텍스트")]
    public string grabText = "돋보기 잡음";

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void OnGrab()
    {
        typer.ShowText(grabText, enableDelay);
    }
}
