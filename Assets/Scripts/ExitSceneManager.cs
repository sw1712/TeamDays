using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitSceneManager : MonoBehaviour
{
    [Header(" UI 연결")]
    public GameObject exitPanel; // "나가시겠습니까?" 팝업 패널
    public Button yesButton;     // 예 버튼
    public Button noButton;      // 아니오 버튼

    private string previousScene; // 이전 씬 이름 저장용


    void Start()
    {
        // 1. 이전 씬 이름 불러오기
        previousScene = PlayerPrefs.GetString("PreviousScene", "Main");

        // 2. 씬이 시작되면 즉시 팝업 표시
        if (exitPanel != null)
            exitPanel.SetActive(true);

        // 3. 버튼 이벤트 연결
        yesButton.onClick.AddListener(OnYesClicked);
        noButton.onClick.AddListener(OnNoClicked);

    }
    private void OnYesClicked()
    {
        Debug.Log("게임 종료");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터에서 정지
#else
        Application.Quit(); // 실제 빌드된 게임 종료
#endif
    }
    private void OnNoClicked()
    {
        SceneManager.LoadScene(previousScene);
    }

}
