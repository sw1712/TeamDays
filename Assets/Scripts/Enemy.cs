using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{ //  적의 이동 속도 (숫자가 높을수록 더 빨리 오른쪽으로 이동)
    public float speed = 5.0f;

    // Rigidbody2D는 물리 효과(충돌, 중력 등)를 담당하는 컴포넌트
    private Rigidbody2D enemyRb;

    // 플레이어 오브젝트를 담아둘 변수 (게임 시작 시 찾아서 저장함)
    private GameObject player;

    //  적이 발사할 총알(발사체) 프리팹
    public GameObject attackPrefab;

    // 총알이 나가는 위치를 표시할 트랜스폼 (빈 오브젝트 등으로 설정)
    public Transform firePoint;

    //  최대 체력 (시작 시 3으로 설정)
    public int maxHP = 5;

    // 현재 체력 (게임 시작 시 maxHP로 초기화됨)
    private int currentHP;

    //  위아래로 움직이는 거리 (예: 2라면 위로 2, 아래로 2)
    public float verticalRange = 2.0f;

    // 위아래로 움직이는 속도
    public float verticalSpeed = 2.0f;

    // 시작할 때의 Y(높이) 위치를 기억
    private float startY;

    // 현재 위로 이동 중인지 아닌지를 판단 (true면 위로 이동)
    private bool movingUp = true;


    public float hitFlashTime = 3f;   // 피격 시 변경될 시간
    public Sprite hitSprite;            // 피격 시 잠시 바꿀 스프라이트
    private Sprite originalSprite;      // 원래 스프라이트 저장용
    private SpriteRenderer sr;          // 스프라이트 렌더러
    private bool isFlashing = false;    // 중복 깜빡임 방지


    void Start()
    {

        // 내 오브젝트에서 Rigidbody2D 컴포넌트를 찾아서 enemyRb에 저장
        enemyRb = GetComponent<Rigidbody2D>();

        // "Player"라는 이름의 오브젝트를 찾아서 player 변수에 저장
        player = GameObject.Find("Player");

        // 현재 체력을 최대 체력으로 설정
        currentHP = maxHP;

        // 시작 시 현재 높이(Y좌표)를 저장 (위아래 움직임 기준점)
        startY = transform.position.y;

        Destroy(gameObject, 9f);

        // SpriteRenderer 가져오기 + 원래 스프라이트 저장
        sr = GetComponentInChildren<SpriteRenderer>();
        if (sr != null)
            originalSprite = sr.sprite;
    }

    void Update()
    {
        // 매 프레임마다 실행됨 (매 순간마다 실행된다고 보면 됨)

        // 만약 플레이어가 존재하면 (null = 없음)
        if (player != null)
        {
            // 플레이어의 Player 스크립트를 가져옴
            Player PlayerScript = player.GetComponent<Player>();

            // 만약 플레이어가 게임오버 상태면 적은 멈춤
            if (PlayerScript != null && PlayerScript.IsGameOver)
            {
                if(enemyRb != null)
                {
                    enemyRb.linearVelocity = Vector2.zero;
                }
                    return; 
            }
        }

        //  적이 오른쪽으로 계속 이동
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        //  위아래로 움직이는 함수 실행
        MoveUpDown();
    }

    //  적이 무언가와 부딪혔을 때 실행되는 함수
    void OnTriggerEnter2D(Collider2D other)
    {
        // 같은 적이나 적의 총알끼리 부딪히면 아무 일도 일어나지 않음
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyAttack"))
            return;

        // 플레이어가 존재하고 게임오버 상태라면 충돌 무시
        if (player != null)
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null && playerScript.IsGameOver)
                return;
        }

        //  플레이어와 부딪혔을 경우
        Player hitPlayer = other.GetComponent<Player>();
        if (hitPlayer != null)
        {
            // 플레이어에게 피해 1을 줌
            hitPlayer.TakeDamage(1);

            // 적은 즉사 처리 (데미지 999)
            TakeDamage(999);
            return;
        }

        //  플레이어의 총알에 맞았을 경우
        if (other.CompareTag("PlayerAttack"))
        {
            // 체력을 1 줄임
            TakeDamage(1);
            if (!isFlashing)
            {
                StartCoroutine(HitFlash());
                Debug.Log("SR found on : " + sr.gameObject.name);
            }
        }
    }
    private IEnumerator HitFlash()
    {
        if (sr == null || hitSprite == null)
            yield break;

        isFlashing = true;

        // 피격 스프라이트로 변경
        sr.sprite = hitSprite;

        // 설정된 시간만큼 대기
        yield return new WaitForSeconds(hitFlashTime);

        // 원래 스프라이트로 되돌리기
        sr.sprite = originalSprite;

        isFlashing = false;
    }


    // 데미지를 입었을 때 실행되는 함수
    public void TakeDamage(int damage)
    {
        // 현재 체력에서 damage만큼 감소
        currentHP -= damage;


        // 체력이 0 이하라면 오브젝트(적) 삭제 = 사망 처리
        if (currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    //  위아래로 움직이는 함수
    private void MoveUpDown()
    {
        // 현재 위치의 Y(높이) 좌표를 y 변수에 저장
        float y = transform.position.y;

        if (movingUp)
        {
            // 위로 이동 (verticalSpeed 속도로 올라감)
            y += verticalSpeed * Time.deltaTime;

            // 위쪽 끝까지 올라가면 방향 전환 (위로 올라간 만큼의 2배 아래로 내려옴)
            if (y >= startY + verticalRange)
            {
                movingUp = false; // 이제 내려가게 전환
            }
        }
        else
        {
            // 내려올 때는 2배 빠르게 내려감
            y -= verticalSpeed * 2f * Time.deltaTime;

            // 아래쪽 범위까지 도달하면 다시 위로 이동
            if (y <= startY - verticalRange)
            {
                movingUp = true;
            }
        }

        // 이동한 y값을 실제 적의 위치에 반영
        transform.position = new Vector2(transform.position.x, y);
    }

    //  총알(발사체)을 발사하는 함수
    public void Shoot()
    {
        // 총알 프리팹이나 발사 위치가 없으면 그냥 나감
        if (attackPrefab == null || firePoint == null)
            return;

        // firePoint 위치에서 총알 프리팹 생성
        GameObject attack = Instantiate(attackPrefab, firePoint.position, firePoint.rotation);

        // 생성된 총알에서 EnemyAttack 스크립트를 찾아서 연결
        EnemyAttack attackScript = attack.GetComponent<EnemyAttack>();

        // 총알에 "이건 이 적이 쏜 거야"라고 알려줌
        if (attackScript != null)
        {
            attackScript.SetOwner(gameObject);
        }
    }
}
