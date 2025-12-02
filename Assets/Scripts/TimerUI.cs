using UnityEngine;
using UnityEngine.UI;   // UI 사용하려면 필요


public class TimerUI : MonoBehaviour
{
    public Image barImage;    // Inspector에서 연결할 UI Image
    public float duration = 3f; // 바가 0이 될 때까지 걸리는 시간(초)
    public bool startOnAwake = true; // 시작하자마자 자동으로 카운트다운 할지

    float timer;             // 내부적으로 흐르는 시간
    bool running = false;    // 타이머 동작 상태

    void Start()
    {
        if (barImage == null)
        {
            Debug.LogError("barImage를 Inspector에서 연결하세요!");
            enabled = false; // 바가 없으면 이 스크립트는 멈춤
            return;
        }

        // 시작할 때 가득 채워진 상태로 설정
        barImage.fillAmount = 1f;
        timer = 0f;

        if (startOnAwake)
            StartTimer();
    }

    void Update()
    {
        if (!running) return;

        // 1초마다 흐르는 시간 더하기
        timer += Time.deltaTime;

        // 남은 비율 계산: 1 -> 0으로 줄어듦
        float ratio = 1f - (timer / duration);
        ratio = Mathf.Clamp01(ratio); // 0보다 작아지지 않게 막음

        // 실제로 이미지의 채워진 비율을 바꿔서 '깎이는' 효과 구현
        barImage.fillAmount = ratio;

        // 타이머가 끝나면 멈추기
        if (ratio <= 0f)
        {
            running = false;
            OnTimerFinished();
        }
    }

    // 외부에서 타이머 시작 가능
    public void StartTimer()
    {
        timer = 0f;
        barImage.fillAmount = 1f;
        running = true;
    }

    // 외부에서 타이머 정지 가능
    public void StopTimer()
    {
        running = false;
    }

    // 타이머가 끝났을 때 호출되는 함수 (원하면 수정)
    void OnTimerFinished()
    {
        Debug.Log("타이머 끝! 바가 0이 되었다.");
        // 여기에 '죽는 연출'이나 효과음 재생, 씬 전환 등 추가 가능
    }
}
