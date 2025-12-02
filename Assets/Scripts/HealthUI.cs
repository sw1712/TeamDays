using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("HP 이미지들 (왼쪽부터 1,2,3...)")]
    public Image[] hpImages;  // 체력 UI 이미지 배열
    public void UpdateHealth(int currentHP)
    {
        for (int i = 0; i < hpImages.Length; i++)
        {
            // 현재 체력보다 크거나 같으면 표시, 작으면 숨김
            hpImages[i].enabled = i < currentHP;
        }
    }
}
