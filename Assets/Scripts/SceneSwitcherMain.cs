using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcherMain : MonoBehaviour
{
    [Header("씬 설정")]
    public string sceneName = "Start";
    public int sceneBuildIndex = -1;

    public string sceneName1 = "WeaponSelectScene";
    public int sceneBuildIndex1 = -1;

    public string sceneName2 = "Setting";
    public int sceneBuildIndex2 = -1;

    public string sceneName3 = "Exit";
    public int sceneBuildIndex3 = -1;

    // UI버튼의 OnClick() 이벤트에 연결 → 첫 번째 씬으로 이동
    public void SwitchToScene1()
    {
        LoadScene(sceneName1, sceneBuildIndex1);
    }

    // UI 버튼의 OnClick() 이벤트에 연결 → 두 번째 씬으로 이동
    public void SwitchToScene2()
    {
        LoadScene(sceneName2, sceneBuildIndex2);
    }

    // UI 버튼의 OnClick() 이벤트에 연결 → 세 번째 씬으로 이동
    public void SwitchToScene3()
    {
        LoadScene(sceneName3, sceneBuildIndex3);
    }


    public void SwitchToScene0()
    {
        LoadScene(sceneName, sceneBuildIndex);
    }
    // 공통된 씬 로드 처리 로직
    private void LoadScene(string name, int buildIndex)
    {

        if (!string.IsNullOrEmpty(name))
        {
            // Build Settings에 해당 씬이 포함되어 있는지 검사
            if (Application.CanStreamedLevelBeLoaded(name))
            {
                SceneManager.LoadScene(name); // 정상적으로 씬 로드
            }
            else
            {
                // 씬이 빌드 세팅에 없거나 오타일 때 경고 출력
                Debug.LogError($"SceneSwitcher : '{name}' 씬이 Build Settings에 없거나 로드할 수 없습니다.");
            }
        }

        else if (buildIndex >= 0 && buildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(buildIndex);
        }
        //  씬 이름도 없고 인덱스도 올바르지 않은 경우
        else
        {
            Debug.LogWarning("SceneSwitcher : 씬 이름 또는 인덱스를 올바르게 설정하세요.");
        }
    }
}
