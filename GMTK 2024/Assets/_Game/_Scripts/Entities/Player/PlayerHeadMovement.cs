using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHeadMovement : MonoBehaviour
{
    #region Vari�veis
    [Header("Configura��es:")]
    [SerializeField] private float moveForce;
    [SerializeField] private float maxDistance;
    [SerializeField] private float resetCanMoveInterval;

    [Header("Refer�ncias:")]
    [SerializeField] private ScreenShake screenShakeScript;
    [SerializeField] private GameObject playerButt;
    [SerializeField] private GameObject playerMiddlePrefab;
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform playerParent;
    [SerializeField] private EdgeCollider2D lineCollider;

    // Componentes:
    private Rigidbody2D _rb;
    private ValveScript _valveScript;
    private MenuManager _menuManager;

    // Inputs:
    private Vector2 _moveInput;
    private Vector2 _lastMoveInput;
    private bool _canMove = true;

    private Vector3[] _linePoints;

    private int _colliderStartPoint;

    private int _lastTargetDistance = 0;

    private bool _changedPos = false;

    // Knots:
    private BoxCollider2D[] _knotsColliders;
    #endregion

    #region Fun��es Unity
    private void Start()
    {
        _linePoints = new Vector3[line.positionCount];

        _knotsColliders = new BoxCollider2D[line.positionCount - 2];

        ResetLineMiddlePoints();

        _rb = GetComponent<Rigidbody2D>();
        _menuManager = FindObjectOfType<MenuManager>();
    }

    private void Update()
    {
        UpdateLinePoints();

        UpdateLineCollider();

        GetMoveInput();

        HasReachMaxDistance();
    }

    private void FixedUpdate()
    {
        if (_moveInput != Vector2.zero && _canMove) 
            ApplyMove();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Knot"))
        {
            if (_lastTargetDistance < _linePoints.Length - 2)
            {
                var knot = Instantiate(playerMiddlePrefab, collision.gameObject.transform.position, Quaternion.identity);

                if (_colliderStartPoint != _linePoints.Length - 2)
                    _colliderStartPoint++;

                StoreKnotCollider(collision.gameObject.GetComponent<BoxCollider2D>());

                SetNewDistance(knot);
            }
        }
        else if (collision.gameObject.CompareTag("NewBase") && !_changedPos)
        {
            var newButtPos = collision.gameObject.transform.Find("New Butt Pos").transform.position;
            if (playerButt.transform.position != newButtPos)
            {
                _colliderStartPoint = 0;
                SetButtPos(newButtPos);
                playerParent.transform.parent = collision.transform;
                _changedPos = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("NewBase"))
        {
            Invoke("ResetChangedPos", 1f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.layer) 
        {
            // Bala
            case 6:
                ResetStretch();
                break;

            // Espinho
            case 7:
                ResetStretch();
                break;

            // Valvula
            case 8:
                _valveScript = collision.gameObject.GetComponent<ValveScript>();
                OpenGate();
                break;


            // Portal FAZER VARIAVEL ESTATICA
            case 9:
                SceneManager.LoadScene("CENA CAIQUE");
                break;

            // SelecionarFases
            case 15:
                if (_menuManager != null)
                {
                    ResetStretch();
                    _menuManager.OpenStageSelection();
                }
                break;

            // Sair
            case 16:
                if (_menuManager != null)
                {
                    ResetStretch();
                    _menuManager.QuitGame();
                }
                break;

            // Voltar
            case 17:
                if (_menuManager != null)
                {
                    ResetStretch();
                    _menuManager.ReturnToMenu();
                }
                break;

            // Fase1
            case 18:
                if (_menuManager != null)
                {
                    _menuManager.OpenStage("CENA CAIQUE");
                }
                break;

            // Fase2
            case 19:
                if (_menuManager != null)
                {
                    _menuManager.OpenStage("CENA CAIQUE");
                }
                break;

            // Fase3
            case 20:
                if (_menuManager != null)
                {
                    _menuManager.OpenStage("CENA CAIQUE");
                }
                break;

            // Fase4
            case 21:
                if (_menuManager != null)
                {
                    _menuManager.OpenStage("CENA CAIQUE");
                }
                break;

            // Fase5
            case 22:
                if (_menuManager != null)
                {
                    _menuManager.OpenStage("CENA CAIQUE");
                }
                break;
        }
    }
    #endregion

    #region Fun��es Pr�prias
    private void GetMoveInput()
    {
        var x = Input.GetAxisRaw("Horizontal");
        var y = Input.GetAxisRaw("Vertical");

        if (x != 0)
            _moveInput = new Vector2(x, 0);
        else if (y != 0)
            _moveInput = new Vector2(0, y);
        else
            _moveInput = new Vector2(0, 0);
    }

    private void ApplyMove() => _rb.AddForce((Vector3) _moveInput * moveForce, ForceMode2D.Impulse);

    private void HasReachMaxDistance() 
    {
        if (Vector3.Distance(gameObject.transform.position, _linePoints[_lastTargetDistance]) >= maxDistance) 
            ResetStretch();
    }

    private void ResetCanMove() => _canMove = true;

    private void UpdateLinePoints() 
    {
        for (int i = 0; i < line.positionCount; i++)
        {
            if (i == 0)
            {
                var buttPos = playerButt.transform.position;
                line.SetPosition(i, new Vector3(buttPos.x, buttPos.y, 0f));
                _linePoints[i] = buttPos;
            }
            else if (i > _lastTargetDistance)
            {
                var headPos = gameObject.transform.position;
                line.SetPosition(i, new Vector3(headPos.x, headPos.y, 0f));
                _linePoints[i] = headPos;
            }
        }
    }

    private void ResetLineMiddlePoints() 
    {
        for (int i = 0; i < _linePoints.Length; i++)
        {
            if (i != 0 && i != _linePoints.Length - 1)
                _linePoints[i] = playerButt.transform.position;
        }

        _lastTargetDistance = 0;
    }

    public void ResetStretch()
    {
        screenShakeScript.ApplyScreenShake();

        _rb.velocity = Vector2.zero;
        gameObject.transform.position = playerButt.transform.position + Vector3.up * 1f;

        ResetLineMiddlePoints();
        ClearKnotsColliders();
        _lastTargetDistance = 0;

        _canMove = false;
        Invoke("ResetCanMove", resetCanMoveInterval);

        var playerMiddlePoints = GameObject.FindGameObjectsWithTag("Player Middle");

        foreach (GameObject point in playerMiddlePoints)
            Destroy(point);
    }

    private void UpdateLineCollider()
    {
        var startPoint = _linePoints[_colliderStartPoint];
        var endPoint = _linePoints[_linePoints.Length - 1];

        var points = new Vector2[] { startPoint, endPoint };

        lineCollider.points = points;
    }

    private void OpenGate() => _valveScript.connectedGate.SetActive(false);
    
    private void SetNewDistance(GameObject knot) 
    {
        _lastTargetDistance++;
        _linePoints[_lastTargetDistance] = knot.transform.position;

        /*
        for (int i = 0; i < _linePoints.Length; i++)
        {
            if (i != 0 && i != _linePoints.Length - 1)
            {
                if (i == _lastTargetDistance)
                    _linePoints[i] = knot.transform.position;
            }
        }
        */
    }

    private void SetButtPos(Vector3 basePos) 
    {
        playerButt.transform.position = basePos;
        ResetStretch();
    }

    private void ResetChangedPos() => _changedPos = false;

    private void StoreKnotCollider(BoxCollider2D collider) 
    {
        for (int i = 0; i < _knotsColliders.Length; i++) 
        {
            if (_knotsColliders[i] == null) 
            {
                _knotsColliders[i] = collider;
                _knotsColliders[i].enabled = false;
            }
        }
    }

    private void ClearKnotsColliders()
    {
        for (int i = 0; i < _knotsColliders.Length; i++)
        {
            if (_knotsColliders[i] != null)
            {
                _knotsColliders[i].enabled = true;
                _knotsColliders[i] = null;
            }
        }
    }
    #endregion
}
