using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    //  총알이 날아가는 속도 (숫자가 클수록 빠르게 이동)
    public float speed = 10.0f;

    // 이 총알을 쏜 "주인(적)" 오브젝트를 저장하기 위한 변수
    private GameObject owner;

    // Rigidbody2D는 물리엔진을 사용해서 움직임을 계산해주는 컴포넌트
    private Rigidbody2D attackRb;

    void Start()
    {
        // 게임 시작 시, 이 오브젝트에 붙어 있는 Rigidbody2D 컴포넌트를 가져옴
        attackRb = GetComponent<Rigidbody2D>();

        // 만약 Rigidbody2D가 없으면 콘솔에 에러 메시지 출력
        if (attackRb == null)
        {
            Debug.LogError("Rigidbody2D가 EnemyAttack2D에 없습니다!");
        }

        //  발사 순간에 왼쪽 방향으로 속도를 주어 총알이 날아가게 함
        // Vector2.left = ( -1, 0 ) 방향 → 왼쪽
        attackRb.linearVelocity = Vector2.left * speed;
    }

    //  이 총알이 누가 쏜 것인지 알려주는 함수
    public void SetOwner(GameObject shooter)
    {
        // shooter(총 쏜 사람, 적 오브젝트)를 owner 변수에 저장
        owner = shooter;
    }

    void Update()
    {
        //  Rigidbody2D로 물리 이동을 처리하므로 별도로 이동 코드가 필요 없음
        // 만약 수동으로 움직이게 하고 싶다면 아래 주석을 풀면 됨 ↓
        // transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    //  다른 오브젝트와 부딪혔을 때 자동으로 실행되는 함수
    private void OnTriggerEnter2D(Collider2D other)
    {
        //  [1] 자신(owner)와 부딪히면 무시 (적 자신에게 맞지 않게 함)
        if (other.gameObject == owner)
            return;

        //  [2] 다른 적의 총알과 부딪히면 무시 (총알끼리 충돌 X)
        if (other.CompareTag("EnemyAttack"))
            return;

        //  [3] 다른 적(Enemy)과 부딪혀도 무시 (적들끼리는 맞지 않음)
        if (other.CompareTag("Enemy"))
            return;

        //  [4] 플레이어와 충돌한 경우
        if (other.CompareTag("Player"))
        {
            // 부딪힌 오브젝트에서 Player 스크립트를 가져옴
            Player player = other.GetComponent<Player>();

            // Player 스크립트가 있다면(즉, 진짜 플레이어라면)
            if (player != null)
            {
                //  플레이어 체력 1 감소
                player.health -= 1;

                // 콘솔창에 플레이어 체력 출력 (디버그용)
                Debug.Log($"플레이어가 공격을 받음! 현재 체력: {player.health}");
            }
        }
        if(other.CompareTag("PlayerAttack"))
        {
            // 부딪힌 오브젝트에서 PlayerAttack 스크립트를 가져옴
           WeaponBullet2D playerAttack = other.GetComponent<WeaponBullet2D>();
            // PlayerAttack 스크립트가 있다면(즉, 진짜 플레이어 공격이라면)
            if (playerAttack != null)
            {
                //  플레이어 공격 오브젝트 파괴
                Destroy(other.gameObject);
            }
        }

        //  어떤 것과 부딪히든, 마지막엔 총알을 제거함
        Destroy(gameObject);
    }
}
