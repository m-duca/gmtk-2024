using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollision : MonoBehaviour
{
    [Header("Configura��es:")]

    [Header("Refer�ncias:")]
    [SerializeField] private PlayerHeadMovement _playerHeadMovementScript;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 7) 
        {
            _playerHeadMovementScript.ResetStretch();
        }
    }
}
