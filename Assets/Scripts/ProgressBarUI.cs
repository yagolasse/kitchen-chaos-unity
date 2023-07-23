using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField]
    private GameObject hasProgressGameObject;
    [SerializeField]
    private Image barImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        
        if (hasProgress == null)
        {
            Debug.LogError("Game Object " + hasProgressGameObject + " doesn't have a component that implements IHasProgress");
        }

        hasProgress.OnProgressChange += IHasProgress_OnProgressChange;
        barImage.fillAmount = 0;
        Hide();
    }

    private void IHasProgress_OnProgressChange(object sender, IHasProgress.OnProgressChangeArgs args)
    {
        var progress = args.progressNormalized;

        barImage.fillAmount = progress;
        if (progress == 0 || progress == 1) Hide();
        else Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
