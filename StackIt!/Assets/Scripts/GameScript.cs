using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && PlattformController.gameOver)
        {
           PlattformController.score = 0;
           PlattformController.gameOver = false;
           SceneManager.LoadScene(0);
        }
    }
}
