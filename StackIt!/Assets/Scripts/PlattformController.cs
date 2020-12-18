using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public static class StaticClass
{
    public static string CrossSceneInformation { get; set; } = "0";
}

public class PlattformController : MonoBehaviour
{

    public Rigidbody2D rb;
    public GameObject plattform;
    public SpriteRenderer spriteRenderer;
    public Text scoreText;
    public Text scoreTextEnd;
    public float moveSpeed;
    public float togglePoint;
    public float gravityScale;
    public float plattformHeight;
    public float spawnHeigt;
    public static bool gameOver = false;
    public static float score = 0;


    private float plattformNumber;
    private bool hMove;
    private bool screenPressed;
    private bool isFrozen;
    private bool plattformPlaced;
    private bool toggleDirection;
    private Transform cameraTransform;
    

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        rb.gravityScale = 0;
        plattformNumber = score;

        hMove = true;
        isFrozen = false;
        screenPressed = false;

        if (Input.anyKeyDown)
        {
            screenPressed = true;
        }
        if (Random.Range(-1,1) < 0)
        {
            toggleDirection = true;
        }
        else
        {
            toggleDirection = false;
        }
        
        plattformPlaced = false;
        
        cameraTransform = Camera.main.gameObject.transform;

        StaticClass.CrossSceneInformation = score.ToString();

        scoreTextEnd.gameObject.SetActive(false);
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
            if (!plattformPlaced)
            {
                hMove = false;
                rb.gravityScale = gravityScale;
            } else if (gameOver)
            {
                score = 0;
                gameOver = false;
                SceneManager.LoadScene(0);
            }
        } else
        {
            screenPressed = false;
        }
        if(gameOver && plattformNumber == score)
        {
            rb.gameObject.SetActive(false);
        }
        if(!isFrozen && plattformNumber + 4 < score)
        {
            isFrozen = true;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            spriteRenderer.color = new Color(0.67451f, 1, 0.67451f, 1);
        }
    }

    private void FixedUpdate()
    {
        if (hMove)
        {
            horizontalMove();
        }
    }

    void horizontalMove()
    {
        Vector2 plattformPosition = rb.position;

        if(plattformPosition.x > togglePoint || plattformPosition.x < -togglePoint)
        {
            if (toggleDirection)
            {
                toggleDirection = false;
            }
            else
            {
                toggleDirection = true;
            }
        }

        if (toggleDirection)
        {
            rb.velocity = Vector2.left * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.right * moveSpeed;
        }
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MainSquare") && !plattformPlaced)
        {
            scoreUp();
            LoadNewPlattform();

        }
        else if (collision.gameObject.CompareTag("Border"))
        {
            rb.gameObject.SetActive(false);

            if (!gameOver)
            {
                plattformPlaced = true;
                gameOver = true;
                cameraOnTower();
            }
            
        }
        else if (collision.gameObject.CompareTag("Plattform") && !plattformPlaced)
        {
            scoreUp();
            LoadNewPlattform();
        }
    }

    private void LoadNewPlattform()
    {
        plattformPlaced = true;

        float randomX = Random.Range(-3, 3);

        GameObject gameObject = Instantiate(plattform) as GameObject;

        gameObject.transform.position = new Vector2(randomX, gameObject.transform.position.y + spawnHeigt);
        gameObject.transform.rotation = new Quaternion(0,0,0,0);
    }

    private void scoreUp()
    {
        score++;
        scoreText.text = "Score: " + score.ToString();
        StaticClass.CrossSceneInformation = score.ToString();
        cameraTransform.Translate(0, plattformHeight, 0);
    }

    private void cameraOnTower()
    {
        scoreText.gameObject.SetActive(false);
        scoreTextEnd.gameObject.SetActive(true);
        scoreTextEnd.text = "Score: " + score.ToString();

        float yPos = (2 + score * 0.25f) - 1.5f;
        cameraTransform.Translate(0, -yPos, 0);

        float height = score * 0.25f + 5;
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, height, 100);
    }
}
