using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenController : MonoBehaviour
{
    public Text scoreText;

    private bool screenPressed = false;

    private void Awake()
    {
        if (Input.anyKeyDown)
        {
            screenPressed = true;
        }
        scoreText.text = "Last Score: " + StaticClass.CrossSceneInformation;

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !screenPressed)
        {
            SceneManager.LoadScene(1);
        } else
        {
            screenPressed = false;
        }

    }
}
