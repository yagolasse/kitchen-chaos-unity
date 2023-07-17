using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField]
    private ClearCounter clearCounter;
    [SerializeField]
    private GameObject selectedCounterVisual;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChange += Player_OnSelectedCounterChange;
    }

    private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterEventArgs e)
    {
        selectedCounterVisual.SetActive(e.selectedCounter == clearCounter);
    }
}
