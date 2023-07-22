using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField]
    private CuttingCounter cuttingCounter;
    [SerializeField]

    private Image barImage;

    private void Start()
    {
        cuttingCounter.OnProgressChange += CuttingCounter_OnProgressChange;
        barImage.fillAmount = 0;
    }

    private void CuttingCounter_OnProgressChange(object sender, CuttingCounter.OnProgressChangeArgs args)
    {
        barImage.fillAmount = args.progressNormalized;
    }
}
