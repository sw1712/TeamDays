using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WeaponSelectPanelController : MonoBehaviour
{
    [Header("UI References")]
    public GameObject selectionPanel;           // 이 스크립트가 붙어있는 메인 패널 (활성화/비활성화용)
    public WeaponChoiceDisplay[] choiceDisplays; // 3개의 무기 선택 슬롯 (아래 3번 스크립트)

    [Header("Available Weapons")]
    public WeaponChoiceData[] allAvailableWeapons; // Inspector에서 모든 무기 목록을 채웁니다.

    [Header("Player Reference")]
    public WeaponShooter playerWeaponShooter; // 플레이어의 무기 발사 스크립트 참조

    private void Start()
    {
        //// 시작 시 패널은 비활성화 상태여야 합니다.
        //if (selectionPanel != null)
        //{
        //  //  selectionPanel.SetActive(false);
        //}

        //// 플레이어 무기 슈터 자동 찾기 (Player 오브젝트에 WeaponShooter가 있다고 가정)
        //if (playerWeaponShooter == null)
        //{
        //    playerWeaponShooter = FindFirstObjectByType<WeaponShooter>();
        //}

        //if (playerWeaponShooter == null)
        //{
        //    Debug.LogError("WeaponSelectPanelController: WeaponShooter를 찾을 수 없습니다!");
        //}
    }

    // GameManager.OnWaveCompleted()에서 이 함수를 호출합니다.
    public void ShowWeaponChoices()
    {
        Debug.Log($"[WSPC] ShowWeaponChoices 호출됨. TimeScale: {Time.timeScale}");
        if (selectionPanel == null)
        {
            Debug.LogError("[WSPC] selectionPanel이 Inspector에 연결되지 않았습니다! UI를 띄울 수 없습니다.");
            return;
        }
        // 게임 정지 및 UI 활성화
        selectionPanel.SetActive(true);

        Debug.Log("[WSPC] selectionPanel 활성화 명령 실행됨!");
        // 3개의 무기를 랜덤하게 선택합니다. (중복 방지 로직은 단순화함)
        if (allAvailableWeapons.Length < choiceDisplays.Length)
        {
            Debug.LogError("선택 가능한 무기 수가 슬롯 수보다 적습니다.");
            return;
        }

        // 선택할 3가지 무기의 인덱스를 랜덤으로 가져옵니다. (실제 게임에서는 중복 없이 복잡하게 가져와야 함)
        int index1 = Random.Range(0, allAvailableWeapons.Length);
        int index2 = Random.Range(0, allAvailableWeapons.Length);
        int index3 = Random.Range(0, allAvailableWeapons.Length);

        // UI에 정보를 설정하고 버튼 이벤트 연결
        SetupChoiceDisplay(choiceDisplays[0], allAvailableWeapons[index1]);
        SetupChoiceDisplay(choiceDisplays[1], allAvailableWeapons[index2]);
        SetupChoiceDisplay(choiceDisplays[2], allAvailableWeapons[index3]);
    }

    // 각 선택 슬롯에 데이터를 바인딩하고 클릭 이벤트를 연결합니다.
    private void SetupChoiceDisplay(WeaponChoiceDisplay display, WeaponChoiceData data)
    {
        display.fullDescriptionText.text =
        $"<size=180%>{data.weaponName}</size>\n" + // 이름을 더 크게 표시
        $"\n{data.description}";

        display.iconImage.sprite = data.icon;

        // 버튼 이벤트 초기화 및 새로운 선택 로직 연결
        Button button = display.GetComponent<Button>();
        button.onClick.RemoveAllListeners(); // 기존 이벤트 제거

        // 이 버튼을 클릭하면 OnWeaponSelected 함수를 호출하고, 선택된 무기의 인덱스를 전달합니다.
        button.onClick.AddListener(() => OnWeaponSelected(data.weaponIndex));
    }

    // 플레이어가 무기를 선택했을 때 호출되는 함수
    public void OnWeaponSelected(int selectedIndex)
    {
        // 1. 플레이어의 무기 발사 스크립트(WeaponShooter)에 선택된 무기 인덱스 적용
        if (playerWeaponShooter != null)
        {
            playerWeaponShooter.selectedBulletIndex = selectedIndex;
            Debug.Log($"무기 선택 완료! 새로운 무기 인덱스: {selectedIndex}");
        }

        // 2. UI 패널 비활성화 및 게임 재개
        selectionPanel.SetActive(false);
        Time.timeScale = 1f;

        // 3. GameManager에게 다음 웨이브를 시작하라고 알림
        GameManager.Instance.StartNextWave();
    }
}
