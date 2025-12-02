using TMPro;
using UnityEngine;
public class StageUI : MonoBehaviour
{
    public TextMeshProUGUI stageText;

    void Start()
    {
        if (stageText != null)
        {
            stageText.text = GameManager.Instance.GetStageText();
        }
    }
}
