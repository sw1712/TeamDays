using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
public class BackgroundSequence : MonoBehaviour
{
    public SpriteRenderer[] backgrounds; // 순차적으로 Fade-In 시킬 배경들
    public float fadeDuration = 1.0f;    // 페이드 시간
    public float delayBetween = 0.5f;    // 다음 배경까지 대기 시간
    public string nextSceneName = "WeaponSelectScene"; // 이동할 씬 이름

    void Start()
    {
        // 처음에는 모두 투명하게 설정 (Invisible)
        foreach (var bg in backgrounds)
        {
            Color c = bg.color;
            c.a = 0f;
            bg.color = c;
            bg.gameObject.SetActive(false);
        }

        // 씬 시작 시 바로 순차적으로 보이게 함
        StartCoroutine(ShowSpritesSequentially());
    }

    IEnumerator ShowSpritesSequentially()
    {
        foreach (var bg in backgrounds)
        {
            bg.gameObject.SetActive(true);

            float t = 0f;
            Color c = bg.color;

            // Fade-in 진행
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
                bg.color = c;
                yield return null;
            }

            // 다음 Sprite 등장까지 딜레이
            yield return new WaitForSeconds(delayBetween);
        }
        SceneManager.LoadScene(nextSceneName);
    }
}
