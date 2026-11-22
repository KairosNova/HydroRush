using UnityEngine;
using UnityEngine.SceneManagement;// needed in order to manage all the scenes

public class MenuManager : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //by reading the current scene load the next scene on the scene list
    }

    
}
