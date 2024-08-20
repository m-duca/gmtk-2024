using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDroppingSFX : MonoBehaviour
{
    #region Fun��es Unity
    private void Start() => StartCoroutine(PlaySFX());
    #endregion

    #region Fun��es Pr�prias
    private IEnumerator PlaySFX() 
    {
        AudioManager.Instance.PlaySFX("water dropping");
        yield return new WaitForSeconds(22.5f);
        StartCoroutine(PlaySFX());
    }
    #endregion
}
