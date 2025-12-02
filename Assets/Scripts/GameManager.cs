using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int currentWave = 1;

    // 최대 웨이브 수를 상수로 정의하여 관리하기 쉽게 합니다.
    private const int MAX_WAVE = 3;
    
    //추가: 게임 오버 상태를 추적하는 변수
    // 이제 이 변수가 게임 오버 상태를 알리는 유일한 역할을 합니다.
    public bool IsGameOver { get; private set; } = false;

    public Sprite[] waveBackgrounds; // Wave별 배경
    // 웨이브별 Stage 이름
    public string[] stageNames = { "Day", "Night", "Rain" };

    public SpriteRenderer backgroundRenderer;
    public void UpdateBackground()
    {
        SpriteRenderer bg = GameObject.Find("Background")?.GetComponent<SpriteRenderer>();

        if (bg == null)
        {
            Debug.LogWarning("배경 SpriteRenderer를 찾을 수 없습니다!");
            return;
        }

        if (waveBackgrounds == null || waveBackgrounds.Length == 0)
        {
            Debug.LogError("waveBackgrounds 배열이 비어있습니다! Inspector에서 Sprite를 넣으세요!");
            return;
        }

        int index = Mathf.Clamp(currentWave - 1, 0, waveBackgrounds.Length - 1);

        bg.sprite = waveBackgrounds[index];
        Debug.Log($"배경 변경 완료 → 인덱스: {index}");
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);   // 씬 넘어가도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void GameOver()
    {
       if(!IsGameOver)
       {
            IsGameOver = true;
            Debug.Log("GameManager: 플레이어 사망! 게임 오버 상태로 전환.");
       }
    }

    // 웨이브 완료 처리
    public void OnWaveCompleted()
    {
        if (IsGameOver) return;
        SceneManager.LoadScene("WeaponSelectScene");
    }

    // 무기 선택 완료 → 다음 Wave 시작
    public void StartNextWave()
    {
        if (IsGameOver) return;

        currentWave++;
        if (currentWave > MAX_WAVE)
        {
            Debug.Log("모든 웨이브(3 웨이브)를 완료했습니다. 게임을 종료하거나 승리 화면으로 전환합니다.");
            // 게임이 멈추도록 Time.timeScale을 0으로 설정
            Time.timeScale = 0f;

            SceneManager.LoadScene("Main");
            // TODO: 승리 화면으로 전환하거나, 게임 종료(Application.Quit()) 코드를 여기에 추가할 수 있습니다.
            return; // 메서드를 여기서 종료하여 SceneManager.LoadScene("Play")를 실행하지 않도록 합니다.
        }
        SceneManager.LoadScene("Play");
        StartCoroutine(ApplyBackgroundAfterLoad());
    }
    // 현재 Wave에 맞는 Stage 텍스트 받기
    public string GetStageText()
    {
        int index = currentWave - 1;

        if (index < 0 || index >= stageNames.Length)
            return $"STAGE {currentWave} : MAX CLEARED";

        return $"STAGE {currentWave} : {stageNames[index]}";
    }
   
    private IEnumerator ApplyBackgroundAfterLoad()
    {
        yield return null; // 1프레임 대기(씬 로드 완료)
        UpdateBackground();
    }

}
