using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValveScript : MonoBehaviour
{
    #region Vari�veis
    [Header("Refer�ncias:")]
    [SerializeField] public GameObject connectedGate;
    [SerializeField] public GameObject activateGate;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] public Sprite openedGateSprite;
    #endregion
}
