using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [Header("Screen Shake Settings:")]
    [SerializeField]
    private float animationSpeed;

    private Animator camAnim;

    #region Fun��es Unity
    private void Awake()
    {
        camAnim = gameObject.GetComponent<Animator>();
    }

    private void Start()
    {
        camAnim.speed = animationSpeed;
    }
    #endregion

    #region Fun��es Pr�prias
    public void ApplyScreenShake()
    {
        camAnim.SetTrigger("shake");
        camAnim.SetInteger("random", Random.Range(1, 3));
    }
    #endregion
}
