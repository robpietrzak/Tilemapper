using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    private Rigidbody2D rd2d;
    public float speed;
    public Text score;
    private int scoreValue = 0;

    public Text lives;
    private int livesValue = 3;

    public GameObject winTextObject;
    public GameObject loseTextObject;

    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;

    private bool gameOver;
    private bool hasTeleported;
    private bool facingRight = true;
    private bool airborn;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        hasTeleported = false;
        gameOver = false;
        rd2d = GetComponent<Rigidbody2D>();
        score.text = "Score: " + scoreValue.ToString();
        lives.text = "Lives: " + livesValue.ToString();
        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);
        musicSource.clip = musicClipOne;
        musicSource.Play();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue++;
            score.text = "Score: " + scoreValue.ToString();
            Destroy(collision.collider.gameObject);
        }
        if(collision.collider.tag == "Enemy")
        {
            livesValue--;
            lives.text = "Lives: " + livesValue.ToString();
            Destroy(collision.collider.gameObject);
        }
        if(scoreValue == 4 && hasTeleported == false)
        {
            transform.position = new Vector2(50.0f, 1.0f);
            hasTeleported = true;
        }
        if(scoreValue == 8 && gameOver == false)
        {
            musicSource.loop = false;
            musicSource.Stop();
            musicSource.clip = musicClipTwo;
            musicSource.Play();
            winTextObject.SetActive(true);
            gameOver = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        float verMovement = Input.GetAxis("Vertical");
        if(collision.collider.tag == "Ground")
        {
            if(verMovement == 0)
            {
                airborn = false;
            }
            else if(verMovement > 0)
            {
                airborn = true;
            }
            if(Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    void Update()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float verMovement = Input.GetAxis("Vertical");
        if(facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if(facingRight == true && hozMovement < 0)
        {
            Flip();
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }
        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }
        if(Input.GetKeyUp(KeyCode.W) && airborn == false)
        {
            anim.SetInteger("State", 2);
        }     
        if(livesValue == 0)
        {
            Destroy(this);
            loseTextObject.SetActive(true);
        }
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }
}
