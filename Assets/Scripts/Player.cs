using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("플레이어 설정")]
    public int health = 300;          // 플레이어 체력
    public float speed = 5.0f;      // 이동 속도

    // 외부 UI Manager 연결
    public HealthUI healthUI;

    private Rigidbody2D playerRb;   // Rigidbody2D 컴포넌트 저장용
    private bool gameOver = false;  //게임 종료 여부

    // 외부에서 게임 오버 상태를 확인할 수 있는 프로퍼티
    public bool IsGameOver
    {
        get { return gameOver; }
    }

    private float minY = -393.0f; // 플레이어가 이동할 수 있는 최소 Y 좌표
    private float maxY = 175.0f;  // 플레이어가 이동할 수 있는 최대 Y 좌표

    private Vector2 newPos;
    void Start()
    {
        // Rigidbody2D 가져오기
        playerRb = GetComponent<Rigidbody2D>();

        // Rigidbody2D가 없을 경우 경고 출력
        if (playerRb == null)
        {
            Debug.LogError("Rigidbody2D가 Player2D에 없습니다!");
        }

        // 이전 씬(WeaponSelectScene)에서 PlayerPrefs에 저장한 무기 인덱스를 불러옴
        // "SelectedWeapon"이라는 키로 저장된 값을 가져오는데, 
        // 만약 저장된 값이 없으면 기본값으로 -1을 반환함 (즉, 선택 안 된 상태)
        int selectedWeapon = PlayerPrefs.GetInt("SelectedWeapon", -1);

        // 불러온 무기 인덱스를 콘솔에 출력해 확인함
        // 예: "이전 씬에서 선택된 무기: 2"
        Debug.Log($"이전 씬에서 선택된 무기: {selectedWeapon}");

        // 시작할 때 UI 초기 체력 설정
        if (healthUI != null)
            healthUI.UpdateHealth(health);
    }

    void Update()
    {
        // 게임 오버 상태일 경우 입력 및 이동 중단
        if (gameOver) return;

        //입력 (W, S 또는 방향키)
        float moveY = Input.GetAxis("Vertical");

        //이동 벡터 계산 (Y축 기준)
        Vector2 movement = new Vector2(0, moveY) * speed * Time.deltaTime;

        // Rigidbody2D가 있으면 물리 기반 이동
        if (playerRb != null)
        {
            // 1) 이동하려는 목표 위치 계산
            newPos = playerRb.position + movement;

            // 2) Y좌표 제한
            newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

            // 3) 최종 이동
            playerRb.MovePosition(newPos);
        }
        else
        {
            Vector3 newPos3 = transform.position + (Vector3)movement;
            newPos3.y = Mathf.Clamp(newPos3.y, minY, maxY);

            transform.position = newPos3;
        }

    }

    // 적 공격 등으로 피해를 입을 때 호출
    public void TakeDamage(int damage)
    {
        // 체력 감소
        health -= damage;
        Debug.Log("플레이어 체력: " + health);

        // UI 업데이트 호출
        if (healthUI != null)
            healthUI.UpdateHealth(health);


        // 체력이 0 이하가 되면 게임 오버 처리
        if (health <= 0 && !gameOver)
        {
            gameOver = true;
            Debug.Log("플레이어 사망");

            // Rigidbody의 속도를 0으로 만들어 움직임 중단
            playerRb.linearVelocity = Vector2.zero;

            // 이후 게임 오버 UI 표시나 리트라이 기능 추가 가능
        }
    }

}
