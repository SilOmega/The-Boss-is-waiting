using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    public FloatReference playerMaxHealth;
    public FloatReference playerHealth;
    
    private CanvasGroup healthUI;
    private float maskSize;

    
    void Start()
    {
        // Initialize component and values for Heath Bar
        healthUI = GetComponent<CanvasGroup>();
        maskSize = healthUI.transform.GetChild(0).GetComponent<Image>().rectTransform.rect.width;
        healthUI.alpha = 1;
    }
    
    private void Update()
    {
        healthUI.GetComponentInChildren<Text>().text = playerHealth.Value + "/" + playerMaxHealth.Value;
        healthUI.transform.GetChild(0).GetChild(0).GetComponent<Image>().color = Color.Lerp(Color.red, Color.green, (float)playerHealth.Value / (float)playerMaxHealth.Value);
        healthUI.transform.GetChild(0).GetComponent<Image>().rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, maskSize * (float)playerHealth.Value / (float)playerMaxHealth.Value);

        // UI disappears when player dies
        if (playerHealth.Value == 0)
            healthUI.alpha = 0;
    }
    
}
