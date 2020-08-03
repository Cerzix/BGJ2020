using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CarController car;

    [SerializeField]
    private Image pullStrengthImg;

    [SerializeField]
    private TextMeshProUGUI pullStrengthText;

    [SerializeField]
    private TrailRenderer trail1;

    [SerializeField]
    private TrailRenderer trail2;

    private void Update() {
        pullStrengthImg.fillAmount = car.PullStrength / 100;
        pullStrengthText.text = Mathf.RoundToInt(car.PullStrength).ToString();

        if(car.PullStrength > 5 && trail1.time <= .3f && trail2.time <= .3f && car.currentVelocity > 0)
        {
            trail1.time += .05f;
            trail2.time += .05f;
        }
        if (car.PullStrength < 5 && trail1.time != 0f && trail1.time != 0f && trail1.time >= 0f && trail2.time >= 0f)
        {
            trail1.time -= .01f;
            trail2.time -= .01f;
        }

    }
}
