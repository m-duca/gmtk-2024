using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    #region Vari�veis
    [Header("Configura��es:")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float lifeTime;

    // Componentes:
    private Rigidbody2D _rb;

    public Vector2 MoveDir;
    #endregion

    #region Fun��es Unity
    private void Start() => _rb = GetComponent<Rigidbody2D>();        

    private void FixedUpdate() => ApplyMovement();
    #endregion

    #region Fun��es Pr�prias
    private void ApplyMovement() => _rb.velocity = MoveDir * moveSpeed;
    #endregion
}
