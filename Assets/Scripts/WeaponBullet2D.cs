using UnityEngine;

public class WeaponBullet2D : MonoBehaviour
{
    public float speed = 20f;          // 총알이 날아가는 속도
    public float lifeTime = 3f;        // 총알이 자동으로 사라질 때까지 걸리는 시간 (초 단위)
    public GameObject hitEffectPrefab; // 충돌 시 생성할 파티클(이펙트) 프리팹

    // Rigidbody2D: 물리 엔진이 적용되는 2D 오브젝트에 사용
    private Rigidbody2D rb;

    void Start()
    {
        // 이 오브젝트(총알)에 붙은 Rigidbody2D 컴포넌트를 가져옴
        rb = GetComponent<Rigidbody2D>();

        // 총알이 바라보는 "오른쪽 방향(transform.right)"으로 순간적인 힘을 줘서 발사
        // ForceMode2D.Impulse = 즉시 힘을 주는 방식 (한 번에 팡!)
        rb.AddForce(transform.right * speed, ForceMode2D.Impulse);

        // 일정 시간이 지나면 총알 자동 삭제 (메모리 낭비 방지)
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 충돌 위치에 파티클(이펙트) 생성
        if (hitEffectPrefab != null)
        {
            // 충돌 지점(contact.point)과 표면의 방향(contact.normal)을 가져옴
            ContactPoint2D contact = collision.contacts[0];

            // 파티클의 회전 방향을 충돌 표면 기준으로 맞춰줌
            Quaternion rot = Quaternion.LookRotation(Vector3.forward, contact.normal);

            // 파티클 프리팹을 생성 (위치: 충돌 지점 / 회전: 표면 방향)
            Instantiate(hitEffectPrefab, contact.point, rot);
        }

        // 총알을 파괴 (충돌 후 사라짐)
        Destroy(gameObject);

        // 적에게 데미지를 주고 싶다면 아래 코드 사용
         if (collision.gameObject.CompareTag("Enemy"))
         {
             // Enemy 스크립트에 있는 TakeDamage() 함수 호출
             collision.gameObject.GetComponent<Enemy>().TakeDamage(1);
         }

         if(collision.gameObject.CompareTag("EnemyAttack"))
         {
             Destroy(collision.gameObject);
         }
    }

}
