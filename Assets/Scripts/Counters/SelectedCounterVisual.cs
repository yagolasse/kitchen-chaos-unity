using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    [SerializeField]
    private BaseCounter counter;
    [SerializeField]
    private GameObject[] selectedCounterVisual;

    private void Start()
    {
        Player.Instance.OnSelectedCounterChange += Player_OnSelectedCounterChange;
    }

    private void Player_OnSelectedCounterChange(object sender, Player.OnSelectedCounterEventArgs e)
    {
        foreach (var item in selectedCounterVisual)
        {
            item.SetActive(e.selectedCounter == counter);
        }
    }
}
