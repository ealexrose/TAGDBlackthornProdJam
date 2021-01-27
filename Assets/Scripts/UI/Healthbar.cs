using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Healthbar : MonoBehaviour
{
    private float TotalHealth;      // ratio: Current Health / Total health and assign this ratio as the slider value.
    private Image health_UI;
    [Tooltip("Amount of time it takes to update the health")]
    [SerializeField] float Update_Health_Duration = 0.4f;
    float tmp_Duration = 0;
    float Health_Fade_Time;
    private float TargetRatio;

    private void Awake()
    {
        health_UI = GetComponent<Image>();
    }

    public void InitializeHealth(float total_Health)
    {
        TotalHealth = total_Health;
    }

    public void UpdateHealth_UI(float Target_Health)
    {
        if(TotalHealth == 0) { return; }
        if(Update_Health_Duration == 0) { Update_Health_Duration = 0.5f; }
        tmp_Duration = Update_Health_Duration;
        TargetRatio = Target_Health / TotalHealth;
        Health_Fade_Time = (health_UI.fillAmount - TargetRatio) / tmp_Duration;
    }

    private void Update()
    {

        if (tmp_Duration > 0)
        {
            tmp_Duration -= Time.deltaTime;
            float Current_Ratio = Mathf.MoveTowards(health_UI.fillAmount, TargetRatio, Health_Fade_Time * Time.deltaTime);
            health_UI.fillAmount = Current_Ratio;
        }
    }

}
