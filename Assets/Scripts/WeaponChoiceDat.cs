using UnityEngine;

[System.Serializable]
public struct WeaponChoiceData
{
    public string weaponName;       // 무기 이름 (예: 하얀 비둘기)
    [TextArea] // 유니티 Inspector에서 여러 줄 입력이 가능한 텍스트 상자를 제공합니다.
    public string description;      // 무기 설명
    public Sprite icon;             // 무기 아이콘
    public int weaponIndex;         // 실제 WeaponShooter에서 사용할 인덱스 (0, 1, 2...)
}
