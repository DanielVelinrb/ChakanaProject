using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ControladorTesting : MonoBehaviour
{
    private GameObject canvas;
    [SerializeField] private GameObject input;

    void Start()
    {
        canvas = transform.GetChild(0).gameObject;
        canvas.SetActive(false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T)) {
            canvas.SetActive(!canvas.activeSelf);
        }
    }

    public void cambiarEscena() {
        string nivelTexto = input.GetComponent<TextMeshProUGUI>().text.Trim();
        SceneManager.LoadScene(int.Parse(nivelTexto.Substring(0, nivelTexto.Length - 1)));
    }

}
