using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{

    public RectTransform pauseMenu;
    public RectTransform confirmQuitMenu;
    public Transform hoyustusGameObject;
    public Button btContinue;
    string escena;

    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            escena = SceneManager.GetActiveScene().name;

            if (escena != "00- Main Menu 0")
            { 

                if (!pauseMenu.gameObject.activeSelf && !confirmQuitMenu.gameObject.activeSelf)
                {
                    pauseMenu.gameObject.SetActive(true);
                    btContinue.Select();
                    hoyustusGameObject.gameObject.SetActive(false);
                    
                }
                else
                {
                    if(pauseMenu.gameObject.activeSelf)
                        {
                        pauseMenu.gameObject.SetActive(false);
                        hoyustusGameObject.gameObject.SetActive(true);
                    }
                    if (confirmQuitMenu.gameObject.activeSelf)
                    {
                        confirmQuitMenu.gameObject.SetActive(false);
                        hoyustusGameObject.gameObject.SetActive(true);
                    }
                }
            }

        }




    }

    public void Continue()
    {
        hoyustusGameObject.gameObject.SetActive(true);
        //btYes.Select();
        //btContinue.Select();
       

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("00- StartRoom 1");
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("00- Main Menu 0");
    }
    public void QuitGame()
    {
        Application.Quit();
    }

}