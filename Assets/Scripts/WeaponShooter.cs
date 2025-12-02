using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [Header("Bullet Prefabs (총알 종류 9개)")]
    public GameObject[] bulletPrefabs = new GameObject[9];

    [Header("Fire Settings")]
    public Transform firePoint;
    public float bulletSpeed = 200f;
    public float fireRate = 0.3f;

    private float nextFireTime = 0.1f;
    private int selectedBulletIndex = 0; // WeaponSelectScene에서 가져옴

    void Start()
    {
        
        // PlayerPrefs에서 선택된 무기 인덱스 가져오기
        selectedBulletIndex = PlayerPrefs.GetInt("SelectedWeapon", 0);

        // 범위 체크
        if (selectedBulletIndex < 0 || selectedBulletIndex >= bulletPrefabs.Length)
        {
            Debug.LogWarning("선택된 무기 인덱스가 범위를 벗어났습니다. 0번으로 초기화합니다.");
            selectedBulletIndex = 0;
        }

        Debug.Log($"현재 선택된 총알: {selectedBulletIndex}");
    }


    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Z) && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    void Shoot()
    {
        GameObject prefabToShoot = bulletPrefabs[selectedBulletIndex];

        if (prefabToShoot == null)
        {
            Debug.LogWarning("해당 총알 prefab이 비어 있습니다!");
            return;
        }

        GameObject bullet = Instantiate(prefabToShoot, firePoint.position, firePoint.rotation);

        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = firePoint.right * bulletSpeed;
        }
    }
}
