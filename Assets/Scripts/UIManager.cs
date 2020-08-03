using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private CarController car;

    [SerializeField]
    private Image pullStrengthImg;

    private void Update() {
        pullStrengthImg.fillAmount = car.PullStrength;
    }
}
