using UnityEngine;

public class EnemySpawnPoint : MonoBehaviour
{
    [Header("Spawn Settings")]
    [Tooltip("The enemy prefab to instantiate. Must contain an AudioSource and enemy logic components.")]
    [SerializeField] private GameObject _enemyPrefab;

    [Tooltip("Distance from the player at which the enemy will be spawned.")]
    [SerializeField] private float _spawnRadius = 10f;

    [Tooltip("If true, the spawner destroys itself after the first spawn to avoid repeated instantiation.")]
    [SerializeField] private bool _spawnOnce = true;

    [Header("References")]
    [Tooltip("Explicit reference to the player Transform. If left null, the spawner will attempt to find the first object tagged 'Player'.")]
    [SerializeField] private Transform _player;

    private bool _hasSpawned;

    private void Awake()
    {
        if (_player == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning($"{nameof(EnemySpawnPoint)} could not find an object tagged 'Player'. Please assign the player manually.", this);
            }
        }
    }

    private void Update()
    {
        if (_hasSpawned && _spawnOnce) return;
        if (_player == null || _enemyPrefab == null) return;

        var distance = Vector2.Distance(_player.position, transform.position);
        if (distance <= _spawnRadius)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Instantiate(_enemyPrefab, transform.position, Quaternion.identity);
        _hasSpawned = true;

        if (_spawnOnce)
        {
            // Optionally destroy the spawner to clean up the scene hierarchy.
            Destroy(gameObject);
        }
    }

#if UNITY_EDITOR
    // Visualize the spawn radius in the Scene view.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _spawnRadius);
    }
#endif
}

