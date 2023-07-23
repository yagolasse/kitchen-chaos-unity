using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterFryingVisual : MonoBehaviour
{
    [SerializeField]
    private GameObject stoveOnVisual;
    [SerializeField]
    private GameObject sizzlingParticles;
    [SerializeField]
    private StoveCounter stoveCounter;

    private void Start()
    {
        stoveCounter.OnStateChange += StoveCounter_OnStateChange;
    }

    private void StoveCounter_OnStateChange(object sender, StoveCounter.OnStateChangeArgs args)
    {
        var active = args.state == StoveCounter.State.Frying || args.state == StoveCounter.State.Fried;

        stoveOnVisual.SetActive(active);
        sizzlingParticles.SetActive(active);
    }
}
