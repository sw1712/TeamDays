using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


public class AudioSettings : MonoBehaviour
{
    [Header(" 오디오 믹서 연결")]
    [Tooltip("프로젝트 내 MainMixer를 드래그하여 연결하세요.")]
    public AudioMixer audioMixer;

    [Header(" UI 슬라이더")]
    public Slider bgmSlider; // 배경음 볼륨 슬라이더
    public Slider sfxSlider; // 효과음 볼륨 슬라이더

    // AudioMixer 파라미터 이름 (Mixer 안의 Exposed Parameter와 일치해야 함)
    private const string BGM_PARAM = "BGMVolume";
    private const string SFX_PARAM = "SFXVolume";

    void Start()
    {
        //  1. 저장된 볼륨 값을 불러오고 (없으면 기본값 0.75)
        float bgmValue = PlayerPrefs.GetFloat(BGM_PARAM, 0.75f);
        float sfxValue = PlayerPrefs.GetFloat(SFX_PARAM, 0.75f);

        //  2. 슬라이더 초기값 설정
        bgmSlider.value = bgmValue;
        sfxSlider.value = sfxValue;

        //  3. 실제 오디오 볼륨 적용
        ApplyVolume(BGM_PARAM, bgmValue);
        ApplyVolume(SFX_PARAM, sfxValue);

        //  4. 슬라이더 값이 변경될 때마다 자동 반영되도록 이벤트 등록
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

    }

    // 배경음 슬라이더가 조정될 때 호출됨
    public void SetBGMVolume(float value)
    {
        ApplyVolume(BGM_PARAM, value);
        PlayerPrefs.SetFloat(BGM_PARAM, value); // 변경값 저장
    }

    // 효과음 슬라이더가 조정될 때 호출됨
    public void SetSFXVolume(float value)
    {
        ApplyVolume(SFX_PARAM, value);
        PlayerPrefs.SetFloat(SFX_PARAM, value); // 변경값 저장
    }

    // 슬라이더(0~1) 값을 실제 데시벨(dB) 값으로 변환하여 믹서에 적용
    private void ApplyVolume(string param, float value)
    {
        // Mathf.Log10(value) * 20 = 데시벨 변환 공식
        float dB = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
        audioMixer.SetFloat(param, dB);
    }
    
    private void OnDisable()
    {
        //  씬이 전환되더라도 PlayerPrefs에 저장된 값 유지
        PlayerPrefs.Save();
    }

}
