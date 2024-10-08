using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCollision : MonoBehaviour
{
    // ReferÍncias:
    private PlayerHeadMovement _playerHeadMovementScript;

    private void Start() => _playerHeadMovementScript = FindObjectOfType<PlayerHeadMovement>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6 || collision.gameObject.layer == 7) 
        {
            _playerHeadMovementScript.ResetStretch();
        }
    }
}
