using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeaponSelectScene : MonoBehaviour
{
    //무기 버튼 관련 변수
    [Header("Weapon Buttons")]
    public Button[] weaponButtons; // 무기 선택 버튼들 (예: 9개 버튼)

    [Header("Weapon Images (Sprites)")]
    public Sprite[] weaponSprites;        // 각 무기 이미지 (버튼 인덱스와 동일하게 매칭)

    private int selectedWeaponIndex = -1; // 어떤 무기를 골랐는지 저장 (-1은 아직 선택 안 함)

    [Header("UI Elements")]
    public Image selectedWeaponImage;     // 선택된 무기 미리보기 이미지
    public GameObject selectedWeaponUI;   // 선택 UI 전체 (예: 프레임/박스)

    //씬 전환 관련 변수
    [Header("Scene Settings")]
    public string nextSceneName = "Play"; // 다음에 넘어갈 전투 씬 이름

    void Start()
    {
        // 모든 무기 버튼에 클릭 이벤트 연결
        // 버튼이 여러 개 있으므로 for문으로 자동 등록함
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            int index = i; //주의! for문 안에서 i를 바로 쓰면 이벤트가 꼬이므로 복사해둠
            weaponButtons[i].onClick.AddListener(() => SelectWeapon(index));
            // 위 한 줄: 버튼을 누를 때 SelectWeapon(index) 실행하도록 연결
        }

        // ▶ 처음에는 선택 UI 숨기기
        //    무기 선택 전에는 큰 이미지가 안 보이도록
        if (selectedWeaponUI != null)
            selectedWeaponUI.SetActive(false);
    }

    //무기를 선택했을 때 실행되는 함수
    public void SelectWeapon(int index)
    {
        // 선택한 무기 번호 저장
        selectedWeaponIndex = index;

        // PlayerPrefs → 간단한 데이터 저장소 (씬이 바뀌어도 유지됨)
        PlayerPrefs.SetInt("SelectedWeapon", selectedWeaponIndex);

        Debug.Log($" 선택된 무기 인덱스: {index}");

        // 1) 선택 UI 활성화
        if (selectedWeaponUI != null)
            selectedWeaponUI.SetActive(true);

        // 2) 선택된 무기의 스프라이트를 미리보기 Image에 설정
        if (selectedWeaponImage != null && weaponSprites.Length > index)
        {
            selectedWeaponImage.sprite = weaponSprites[index];
        }

        // 3) 버튼 UI 강조 효과 (선택된 버튼만 하이라이트)
        HighlightSelectedButton(index);
    }

    // 선택된 버튼만 시각적으로 강조하는 함수
    private void HighlightSelectedButton(int index)
    {
        for (int i = 0; i < weaponButtons.Length; i++)
        {
            // 버튼의 색을 꺼진 색으로 초기화
            ColorBlock cb = weaponButtons[i].colors;
            cb.normalColor = new Color(1f, 1f, 1f, 0.5f); // 흐린 색
            weaponButtons[i].colors = cb;
        }

        // 선택된 버튼만 강조 색상 적용
        ColorBlock selected = weaponButtons[index].colors;
        selected.normalColor = new Color(1f, 1f, 1f, 1f); // 진하게
        weaponButtons[index].colors = selected;
    }

    // "다음으로" 버튼을 눌렀을 때 실행

    public void GoToNextScene()
    {
        //  무기를 하나라도 골랐다면
        if (selectedWeaponIndex != -1)
        {
            // 저장된 씬 이름으로 전환
            GameManager.Instance.StartNextWave();
        }
        else
        {
            // 아무 무기도 선택하지 않았을 때 경고
            Debug.LogWarning(" 무기를 먼저 선택하세요!");
        }
    }
    
}
