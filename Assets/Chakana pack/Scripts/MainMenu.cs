using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class MainMenu : MonoBehaviour
{
    public RectTransform homeMenu;
    public RectTransform pauseMenu;
    public RectTransform exitMenu;
    public RectTransform confirmQuitMenu;

    public RectTransform inventoryMenu;


    //public RectTransform exitMenu;
    //public Transform hoyustusGameObject;
    public Button btContinue;
    public Button btCancelExitGame;
    public Button btExitGame;
    public Button btLoadSlot;


    string escena;
    bool boolHomeMenuActive = true;

    public bool mouseMovido = false;
    public bool joystickIzquierdoMovido = false;

    private float joystickThreshold = 0.2f; // Umbral de sensibilidad del joystick
    private bool gamePadConectado = false;
    private bool botonGamePadPress = false;
    private bool anyKeyPress = false;
    private bool altKeyPress = false;

    [Header("LoadPanel")]
    [SerializeField] GameObject loadPanel;
    [SerializeField] Slider loadBar;

    void Start()
    {
        btContinue.Select();
        Time.timeScale = 1f;

        if (Input.GetJoystickNames().Length > 0 && !string.IsNullOrEmpty(Input.GetJoystickNames()[0]))
        {
            joystickIzquierdoMovido = true;
            //Debug.Log("Start Gamepad conectado");
        }else
            mouseMovido = true;

        //mouseMovido = true;

    }

    void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey)
        {
            anyKeyPress = true;
            
            //Debug.Log("Se ha presionado una tecla en el teclado: " + e.keyCode);
        }
    }

    void Update()
    {
        mouseMovido = true;

        if (Input.GetMouseButtonDown(0))
        {
            if (escena == "00- Main Menu 0")
            {
                if (homeMenu.gameObject.activeSelf)
                    btContinue.Select();
                else
                if (exitMenu.gameObject.activeSelf)
                    btExitGame.Select();
            }
            else
            {
                if (pauseMenu.gameObject.activeSelf)
                    btContinue.Select();
                else if (exitMenu.gameObject.activeSelf)
                    btExitGame.Select();
            }
        }
        //mouseMovido = true;
        //joystickIzquierdoMovido = true;


        //if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
        //{
        //    altKeyPress = true;
        //    //Debug.Log("Se ha presionado la tecla Alt en el teclado.");
        //}
        //if (altKeyPress && !Input.GetKey(KeyCode.LeftAlt) && !Input.GetKey(KeyCode.RightAlt))
        //{
        //    //Debug.Log("Se ha soltado la tecla Alt en el teclado.");
        //    altKeyPress = false;
        //}

        //if (anyKeyPress)
        //{
        //    mouseMovido = true;
        //    joystickIzquierdoMovido = false;
        //}

        //else

        //{
        //    mouseMovido = false;
        //    joystickIzquierdoMovido = true;
        //}


        // Validar movimiento del mouse
        if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            mouseMovido = true;
            joystickIzquierdoMovido = false;
        }

        //// Validar movimiento del joystick izquierdo del gamepad
        //float joystickX = Input.GetAxis("Horizontal");
        //float joystickY = Input.GetAxis("Vertical");
        //if (Mathf.Abs(joystickX) > joystickThreshold || Mathf.Abs(joystickY) > joystickThreshold)
        //{
            
           
           
        //}


        Gamepad gamepad = Gamepad.current;
        if (gamepad != null)
        {
            Vector2 leftStick = gamepad.leftStick.ReadValue();

            if (leftStick != Vector2.zero && !Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow) &&
                !Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow))
            {
                mouseMovido = false;
                joystickIzquierdoMovido = true;

                Debug.Log("El joystick izquierdo del gamepad se ha movido exclusivamente.");
                // Realiza las acciones que desees cuando solo el joystick izquierdo se haya movido
            }
        }



        Escape();
        Inventory();
    }
    public void Inventory()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryMenu.gameObject.SetActive(true);
            Debug.Log("Se ha presionado la tecla 'I'.");       
            
        }
    }
    public void Escape()
    {
        escena = SceneManager.GetActiveScene().name;
        if (Input.GetButtonDown("Cancel"))
        {
            if (escena != "00- Main Menu 0")
            {
                if (!pauseMenu.gameObject.activeSelf && !confirmQuitMenu.gameObject.activeSelf)
                {
                    pauseMenu.gameObject.SetActive(true);
                    btContinue.Select();
                    Time.timeScale = 0f;

                }
                else
                {
                    Time.timeScale = 1f;
                    if (pauseMenu.gameObject.activeSelf)
                    {
                        pauseMenu.gameObject.SetActive(false);
                    }
                    if (confirmQuitMenu.gameObject.activeSelf)
                    {
                        confirmQuitMenu.gameObject.SetActive(false);
                    }

                }

            }
            else
            {
                if (Input.GetKeyDown(KeyCode.Escape) && mouseMovido && !joystickIzquierdoMovido)
                {
                    
                   
                        EscapeHomeMenu();

                    //Debug.Log("Se ha presionado la tecla Escape en el teclado.");

                }
                else
                {
                   
                    Debug.Log("Se ha presionado el botón Escape (start) del gamepad porque mouseMovido es "+ mouseMovido);
                }
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire2"))
            {
                //Debug.Log("FIRE2 O ALT: joystickIzquierdoMovido>"+ joystickIzquierdoMovido+ " altKeyPress>"+ altKeyPress);

                if (escena != "00- Main Menu 0")
                {
                    Debug.Log("escena != 00 - Main Menu 0");
                }
                else
                {
                    if(!altKeyPress && joystickIzquierdoMovido)
                        EscapeHomeMenu();
                        
                   
                }
            }
            else
            {
                Debug.Log("Algun booleano en false BT FIRE, joystickIzquierdoMovido-->"+ joystickIzquierdoMovido);
            }


        }
    }
    public void EscapeHomeMenu()
    {

        if (boolHomeMenuActive)
        {
            homeMenu.gameObject.SetActive(false);
            boolHomeMenuActive = false;
            ActivateExitGame();

            btExitGame.Select();
        }
        else
        {
            homeMenu.gameObject.SetActive(true);
            boolHomeMenuActive = true;
            ActivateHomeMenu();

        }
    }
    public void ActivateSlot1()
    {

        btLoadSlot.Select();
    }
    public void ActivateStart()
    {

        btContinue.Select();
    }
    public void ActivateExitGame()
    {
        btExitGame.Select();

    }
    public void ActivateCancelExitGame()
    {
        btCancelExitGame.Select();

    }
    public void ActivateHomeMenu()
    {
        homeMenu.gameObject.SetActive(true);
        boolHomeMenuActive = true;
        btContinue.Select();

    }
    public void DeActivateHomeMenu()
    {
        homeMenu.gameObject.SetActive(false);
        boolHomeMenuActive = false;
        

    }
    public void Continue()
    {
        //hoyustusGameObject.gameObject.SetActive(true);
        //btYes.Select();
        //btContinue.Select();


    }
    public void PlayGame()
    {
        PlayerPrefs.DeleteAll();

        loadPanel.SetActive(true);

        //if (!corutinaIniciada)
        //{
        StartCoroutine(LoadAsyncScene(2));
        //corutinaIniciada = true;
        //}
        //SceneManager.LoadScene("00- StartRoom 1");
    }
    public void OpenMainMenu()
    {
        loadPanel.SetActive(true);

        //if (!corutinaIniciada)
        //{
        StartCoroutine(LoadAsyncScene(1));
        //corutinaIniciada = true;
        //}
    }

    IEnumerator LoadAsyncScene(int sceneIndex)
    {

        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneIndex);
        //SceneManager.LoadScene(9 + 1);
        //yield return new WaitForSeconds(1f);
        //while(!asyncOperation.isDone)
        //{
        //    Debug.Log("Progres: "+asyncOperation.progress);
        //    loadBar.value = asyncOperation.progress+0.05f;
        //    yield return null;
        //}

        for (int i = 0; i < 20; i++)
        {
            //Debug.Log("Progres: "+asyncOperation.progress);
            //loadBar.value = asyncOperation.progress+0.05f;
            if (i < 15)
            {
                loadBar.value = i * 0.01f;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                loadBar.value = i * 0.09f + 0.16f;
                yield return new WaitForSeconds(1.5f);
            }
        }

        //yield break;
    }
    public void OpenCredits()
    {
        loadPanel.SetActive(true);

        //if (!corutinaIniciada)
        //{
        StartCoroutine(LoadAsyncScene(16));
        //corutinaIniciada = true;
        //}
        //SceneManager.LoadScene("00- Credits 0");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void ActivePlayer(bool active)
    {

        //hoyustusGameObject.gameObject.SetActive(active);
        if (active)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0f;
    }

}