using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大砲オブジェクトのクラス
/// </summary>
public class CannonUnionObject : MonoBehaviour, IUnionObject
{
    [SerializeField] private UnionData UnionData;             // プロパティに設定するデータ
    public UnionData unionData { get; set; }                  // 継承プロパティ

    // インスペクター上で設定する変数
    [Header("発射物設定")]
    [SerializeField] private GameObject FiringPoint;          // 発射位置
    [SerializeField] private GameObject CannonObjectAmmo;     // 発射する弾
    [SerializeField] private float FireCooltime = 3f;         // 発射のクールタイム
    [SerializeField] private float GravityScale = 1f;         // 重力スケール

    [Header("索敵設定")]
    [SerializeField] private float InitialVelocityX = 8f;     // X軸方向の初速度
    [SerializeField] private float SearchDistance = 8f;       // 索敵範囲の半径
    [SerializeField] private float SearchPointDistance = 10f; // 索敵範囲の中心の距離
    [SerializeField] private LayerMask TargetLayerMask;       // 狙う対象のレイヤー

    // 内部処理する変数
    private GameObject targetEnemy;                       // 範囲内にいる狙う対象
    private bool isFired = false;                         // クールタイムが終わっているか判定
    private SpriteRenderer _spriteRenderer;               // 向き確認用
    private int direction;                                // 発射する向き

    // ====================================================================
    // デバッグ用軌跡表示フィールド
    // ====================================================================
    // [SerializeField] private LineRenderer trajectoryLine; // 軌跡表示用のLineRenderer
    // [SerializeField] private int trajectoryPoints = 50;   // 軌跡のポイント数
    // [SerializeField] private float trajectoryTime = 2f;   // 軌跡の表示時間

    void Awake()
    {
        // trajectoryLine = GetComponent<LineRenderer>();
        // LineRendererが設定されていない場合は自動生成
        // if (trajectoryLine == null)
        // {
        //     GameObject lineObj = new GameObject("TrajectoryLine");
        //     trajectoryLine = lineObj.AddComponent<LineRenderer>();
        //     trajectoryLine.startWidth = 0.1f;
        //     trajectoryLine.endWidth = 0.1f;
        //     trajectoryLine.material = new Material(Shader.Find("Sprites/Default"));
        //     trajectoryLine.startColor = Color.yellow;
        //     trajectoryLine.endColor = Color.yellow;
        // }
    }

    void Start()
    {
        // プロパティにデータを設定
        if (unionData == null)
        {
            unionData = new UnionData();
        }
        unionData.unionDataNeme = UnionData.unionDataNeme;

        _spriteRenderer = GetComponent<SpriteRenderer>();

        // クールタイムの計測を開始
        StartCoroutine(FireCoolTimeCount());

        // 発射する向きをSpriteRendererの向きから取得
        direction = _spriteRenderer.flipX ? -1 : 1;

        // 発射位置のX軸の位置を向きに応じて反転
        Vector3 localPosition = FiringPoint.transform.localPosition;
        localPosition.y = Mathf.Abs(localPosition.y) * -direction; // Y軸を反転
        FiringPoint.transform.localPosition = localPosition;
    }

    void FixedUpdate()
    {
        // 前方の敵を探す
        targetEnemy = EnemySearchRay();


        Debug.Log($"targetEnemy : {targetEnemy}");

        // 発射可能な状態なら
        if (isFired)
        {
            // ターゲットがいる場合
            if (targetEnemy != null)
            {
                // ターゲットの位置に発射する
                FireLaunch(targetEnemy.transform.position);

                // クールタイムの計測を開始
                StartCoroutine(FireCoolTimeCount());
            }
            else
            {
                // 索敵範囲の終点に向けて発射
                FireLaunch(new Vector3(this.transform.position.x + SearchDistance * 2 * direction, this.transform.position.y));
                Debug.Log($"発射位置のX軸 : {this.transform.position.x + SearchDistance * 2 * direction}");

                // クールタイムの計測を開始
                StartCoroutine(FireCoolTimeCount());
            }
        }
    }

    // =========================================================
    // 索敵処理
    // =========================================================

    // 敵をレイで探す
    private GameObject EnemySearchRay()
    {
        // 大砲の前方の位置
        Vector2 searchRadiusCenter = transform.position + transform.up * SearchPointDistance * -1 * direction;
        // 指定した範囲内の敵コライダーを検出
        Collider2D[] Enemies = Physics2D.OverlapCircleAll(searchRadiusCenter, SearchDistance, TargetLayerMask);

        // 一番近い敵の距離
        float closeEnemyDistance = float.MaxValue;

        // 敵の配列番号
        GameObject closeEnemy = null;

        // 敵の数だけ繰り返す
        foreach (Collider2D enemy in Enemies)
        {
            // 配列内の敵との距離を計算
            float distance = Vector2.Distance(this.transform.position, enemy.gameObject.transform.position);

            // 現在の一番近い敵より距離が短い場合
            if (distance < closeEnemyDistance)
            {
                // 現在の配列番号を保存
                closeEnemy = enemy.gameObject;

                // 一番近い距離を更新
                closeEnemyDistance = distance;
            }
        }

        // 最も近い敵を返す（居なければnull）
        return closeEnemy;
    }

    // 索敵範囲をGizmoで描画する
    private void OnDrawGizmosSelected()
    {
        // ギズモの色を設定
        Gizmos.color = Color.red;

        // 索敵範囲の中心を計算
        Vector2 searchRadiusCenter = transform.position + transform.up * SearchPointDistance * -1 * direction;

        // 索敵範囲をギズモで描画
        Gizmos.DrawWireSphere(searchRadiusCenter, SearchDistance);
    }

    // =========================================================
    // 発射処理
    // =========================================================

    // オブジェクトを生成して指定した初速度で放物運動させる
    private void FireLaunch(Vector2 targetPosition)
    {
        GameObject newCannonAmmo = Instantiate(CannonObjectAmmo, FiringPoint.transform.position, Quaternion.identity);
        Rigidbody2D rb = newCannonAmmo.GetComponent<Rigidbody2D>();

        // 重力スケールを設定
        rb.gravityScale = GravityScale;

        // 発射地点と目標地点の位置
        Vector2 startPosition = FiringPoint.transform.position;
        Vector2 targetPos = targetPosition;

        // 重力加速度
        float gravity = Mathf.Abs(Physics2D.gravity.y);

        // 目標地点までの水平距離と高さ差
        float distance = targetPos.x - startPosition.x;
        float height = targetPos.y - startPosition.y;

        // 飛行時間を計算
        float flightTime = distance / (InitialVelocityX * direction);

        // Y軸方向の初速度を計算
        float initialVelocityY = (height + 0.5f * gravity * flightTime * flightTime) / flightTime;

        // 初速度のベクトルを計算
        Vector2 initialVelocity = new Vector2(InitialVelocityX * direction, initialVelocityY);

        // 速度を設定
        rb.linearVelocity = initialVelocity;

        // // 軌跡を表示
        // ShowTrajectory(startPosition, initialVelocity, gravity);

        // 移動方向に回転するコルーチン開始
        StartCoroutine(RotateToMovementDirection(newCannonAmmo.transform, rb));

        // 4秒後に削除
        Destroy(newCannonAmmo, 4f);
    }

    // // 放物運動の軌跡を計算して表示する
    // private void ShowTrajectory(Vector2 startPos, Vector2 initialVelocity, float gravity)
    // {
    //     // 軌跡のポイントを計算
    //     Vector3[] points = new Vector3[trajectoryPoints];
    //     float timeStep = trajectoryTime / trajectoryPoints;

    //     for (int i = 0; i < trajectoryPoints; i++)
    //     {
    //         float time = timeStep * i;
    //         float x = startPos.x + initialVelocity.x * time;
    //         float y = startPos.y + initialVelocity.y * time - 0.5f * gravity * time * time;
    //         points[i] = new Vector3(x, y, 0);
    //     }

    //     // LineRendererにポイントを設定
    //     trajectoryLine.positionCount = trajectoryPoints;
    //     trajectoryLine.SetPositions(points);
    //     trajectoryLine.enabled = true;

    //     // 一定時間後に軌跡を非表示にする
    //     StartCoroutine(HideTrajectory());
    // }

    // // 軌跡を非表示にするコルーチン
    // private IEnumerator HideTrajectory()
    // {
    //     yield return new WaitForSeconds(trajectoryTime);
    //     // trajectoryLine.enabled = false;
    // }

    // 指定されたオブジェクトの角度がリジッドボディの移動方向を向き続ける
    private IEnumerator RotateToMovementDirection(Transform projectile, Rigidbody2D rb)
    {
        // オブジェクトが消去されるまで継続
        while (projectile != null)
        {
            Vector2 velocity = rb.linearVelocity;
            if (velocity.sqrMagnitude > 0.01f) // ほぼ静止状態でなければ回転
            {
                float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
                projectile.rotation = Quaternion.Euler(0, 0, angle);
            }
            yield return null; // 毎フレーム更新
        }
    }

    // =========================================================
    // 発射クールタイム処理
    // =========================================================

    // 発射のクールタイムを開始する
    private IEnumerator FireCoolTimeCount()
    {
        // 発射不可能
        isFired = false;

        float time = 0;
        // 計測時間がクールタイムの時間になるまで繰り返す
        while (time < FireCooltime)
        {
            time += Time.deltaTime;
            yield return null;
        }

        // 発射可能にする
        isFired = true;
    }
}
