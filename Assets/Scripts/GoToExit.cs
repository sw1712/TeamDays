using UnityEngine;
using UnityEngine.SceneManagement;



public class GoToExit : MonoBehaviour
{
    public void GoToExitScene()
    {
        // 현재 씬 이름 저장
        PlayerPrefs.SetString("PreviousScene", SceneManager.GetActiveScene().name);

        // ExitScene으로 이동
        SceneManager.LoadScene("ExitScene");

        Time.timeScale = 0f;

    }
    void OnEnable()
    {
        //  씬이 로드될 때 (ExitScene이 아닌 다른 씬에서 돌아온 경우)
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    // 씬이 새로 로드될 때 호출됨
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //  ExitScene이 아닐 경우 (즉, 원래 씬으로 돌아왔을 때)
        if (scene.name != "Exit")
        {
            // 일시정지 해제
            Time.timeScale = 1f;
        }
    }
}

