using UnityEngine;
using System.Collections;


public class SpawnManager : MonoBehaviour
{
    [Header("Enemy Prefabs")] // 인스펙터에서 보기 쉽게 구분선 생성
    public GameObject enemyPrefab1;  // 적 1번 프리팹
    public GameObject enemyPrefab2;  // 적 2번 프리팹
    public GameObject enemyPrefab3;  // 적 3번 프리팹

    [Header("Player Reference")]
    public Player player; // 플레이어를 참조 (게임오버 여부 확인용)

    //웨이브(적이 한 번에 몰려오는 라운드) 관련 변수들
   
    public int enemiesPerWave = 10;  // 한 웨이브에서 소환할 적의 수
    private int enemiesSpawned = 0;   // 현재 웨이브에서 소환된 적의 수
    private bool isSpawning = false;  // 적 생성 중인지 여부 (중복 방지용)

    [Header("Spawn Area Settings")]
    public float topBound = 7.0f;     // 적이 나타날 수 있는 Y좌표의 최댓값
    public float bottomBound = -4.0f; // 적이 나타날 수 있는 Y좌표의 최솟값
    public float spawnPositionX = 10.0f; // 적이 나타날 X좌표 (보통 화면 오른쪽 밖)

    public int maxAliveEnemies = 20; // 필드에 존재 가능한 최대 적 수


    [Header("Background Change")]
    public SpriteRenderer backgroundRenderer;   // 현재 화면 배경
    public Sprite[] waveBackgrounds;            // 교체용 배경 목록
    private int currentBackgroundIndex = 0;


    private void Start()
    {
        //만약 인스펙터에서 Player를 지정하지 않았다면 자동으로 찾아서 연결
        if (player == null)
        {
            player = FindFirstObjectByType<Player>(); 
            if (player == null)
            {
                Debug.LogError("SpawnManager(2D): Player 오브젝트를 찾을 수 없습니다!");
            }
        }
    }

    void Update()
    {
        // 플레이어가 죽었다면(게임오버 상태라면) 적 소환 멈춤
        if (player != null && player.IsGameOver) return;

        // 적을 소환 중이 아니라면 새로운 웨이브 시작
        if (!isSpawning)
        {
            StartCoroutine(SpawnWave());
        }
    }

    // 한 웨이브의 적들을 일정 간격으로 계속 소환하는 코루틴
    IEnumerator SpawnWave()
    {
        isSpawning = true;     // 현재 스폰 중이라고 표시
        enemiesSpawned = 0;    // 이번 웨이브에서 소환된 수 초기화

        // 이번 웨이브에 사용할 적 종류 하나 선택 (랜덤)
        GameObject enemyToSpawn = GetEnemyForWave(GameManager.Instance.currentWave);

        if (enemyToSpawn == null)
        {
            Debug.LogWarning($"Wave {GameManager.Instance.currentWave} : 적 프리팹이 지정되지 않았습니다!");
            isSpawning = false;
            yield break;
        }

        while (enemiesSpawned < enemiesPerWave)
        {
            if (player.IsGameOver) yield break;

            if (GetAliveEnemyCount() >= maxAliveEnemies)
            {
                yield return new WaitForSeconds(0.5f);
                continue;
            }

            SpawnEnemy(enemyToSpawn);
            enemiesSpawned++;

            yield return new WaitForSeconds(1f); // 다음 소환까지 딜레이
        }
        
        GameManager.Instance.OnWaveCompleted();

        // 스폰 루틴 종료
        yield break;
    }


    // 실제로 적을 화면에 생성하는 함수
    void SpawnEnemy(GameObject enemy)
    {
        if (enemy == null)
        {
            Debug.LogWarning("SpawnEnemy(2D): prefab이 할당되지 않았습니다!");
            return;
        }

        // X는 고정, Y는 위아래 랜덤으로 생성 (Z는 2D이므로 0)
        Vector3 spawnPos = new Vector3(
            spawnPositionX,
            Random.Range(bottomBound, topBound),
            0
        );

        // 적을 해당 위치에 생성
        Instantiate(enemy, spawnPos, Quaternion.identity);
    }


    int GetAliveEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }

    public GameObject GetEnemyForWave(int wave)
    {
        switch (wave)
        {
            case 1:
                return enemyPrefab1;
            case 2:
                return enemyPrefab2;
            case 3:
                return enemyPrefab3;

            // wave 4부터는 다시 반복하고 싶으면
            default:
                int index = (wave - 1) % 3; // 0,1,2 반복
                if (index == 0) return enemyPrefab1;
                if (index == 1) return enemyPrefab2;
                return enemyPrefab3;
        }
    }
}


