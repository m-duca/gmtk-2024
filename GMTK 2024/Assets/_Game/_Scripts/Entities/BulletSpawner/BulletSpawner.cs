using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    #region Vari�veis
    [Header("Configura��es:")]
    [SerializeField] private float bulletRotationZ;
    [SerializeField] private Vector2 bulletMoveDir;
    [SerializeField] private float spawnInterval;

    [Header("Refer�ncias:")]
    [SerializeField] private BulletMovement bulletPrefab;
    [SerializeField] private Transform spawnPoint;
    #endregion

    #region Fun��es Unity
    private void Start() => StartCoroutine(SpawnBullet());
    #endregion

    #region Fun��es Pr�prias
    private IEnumerator SpawnBullet() 
    {
        var bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.Euler(0f, 0f, bulletRotationZ));
        bullet.MoveDir = bulletMoveDir.normalized;

        yield return new WaitForSeconds(spawnInterval);

        StartCoroutine(SpawnBullet());
    }
    #endregion
}