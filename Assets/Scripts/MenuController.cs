using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField]
    private Button startButton;

    private void Start()
    {
        startButton.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SceneManager.LoadScene(1);
    }
}
