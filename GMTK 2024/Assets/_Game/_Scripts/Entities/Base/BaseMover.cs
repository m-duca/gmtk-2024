using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMover : MonoBehaviour
{
    #region Vari�veis
    [Header("Configura��es:")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private Vector2 initialMoveDir;

    // Componentes:
    private Rigidbody2D _rb;

    // Dire��o:
    private Vector2 _currentMoveDir;
    private bool _canChangeDir = true;
    #endregion

    #region Fun��es Unity
    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        SetNewDirection(initialMoveDir);
    }

    private void FixedUpdate() => ApplyMove();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BaseMoverChanger") && _canChangeDir) 
        {
            _canChangeDir = false;
            Invoke("ResetCanChangeDir", 0.25f);
            SetNewDirection(collision.gameObject.GetComponent<BaseMoverChanger>().NewDirection.normalized);
        }
    }
    #endregion

    #region Fun��es Pr�prias
    private void ApplyMove() => _rb.velocity = _currentMoveDir * moveSpeed;

    private void ResetCanChangeDir() => _canChangeDir = true;

    private void SetNewDirection(Vector2 newDir) => _currentMoveDir = newDir;
    #endregion
}
