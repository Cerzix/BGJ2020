using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleAnimation : MonoBehaviour
{
    [SerializeField]
    private Image gameTitle;

    [SerializeField]
    private Vector3 punchScale;

    [SerializeField]
    private float duration;

    [SerializeField]
    private int vibrato;

    [SerializeField]
    private float randomness;

    private void Start()
    {
        InvokeRepeating("MenuBounce", 0, 1);        
    }

    private void MenuBounce()
    {
        gameTitle.transform.DOPunchScale(punchScale, duration, vibrato, randomness);
    }
}
