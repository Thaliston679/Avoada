using System.Collections;
using System.Collections.Generic;
using UnityEngine;  
using UnityEngine.UI;
using TMPro;

public class PlayerMove : MonoBehaviour
{
    //Método 1 (Mais complexo mas com mais informações)
    private Vector2 startTouchPos;
    private Vector2 currentTouchPos;
    private Vector2 endTouchPos;
    private bool stopTouuch = false;

    private float swipeRangeS;
    private float swipeRangeL;
    //public float smallJumpRange;
    //public float largeJumpRange;
    public float tapRange;

    Rigidbody2D rb;

    public TextMeshProUGUI txt;

    private int varPN, varPA, varA, varR, varB, varC;

    public float jumpForce;
    public float multiplicador;

    public bool condicao = false;

    List<float> numbers = new List<float>() {-3f, 0, 3};
    List<float> coins = new List<float>() { -2.5f, -1.5f, 0.5f, 1.5f, 3.5f };

    public GameObject inimigoObj;
    public GameObject coinsObj;

    private GameObject currentPlat;
    [SerializeField] private BoxCollider2D playerCol;

    public GameObject cam;
    private Animator camAnim;

    private Animator playerAnim;

    private int screenHeight;

    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        condicao = true;
        camAnim = cam.GetComponent<Animator>();
        playerAnim = GetComponent<Animator>();
        screenHeight = Screen.height;
        swipeRangeL = screenHeight * 0.25f;
        swipeRangeS = screenHeight * 0.05f;
    }

    private void Update()
    {
        Swipe();
        TextMeshProGUI();
        if (condicao)
        {
            StartCoroutine(Aaaa());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("platform"))
        {
            currentPlat = collision.gameObject;
        }

        if (collision.gameObject.CompareTag("platform") || collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("platform"))
        {
            currentPlat = null;
        }

        if (collision.gameObject.CompareTag("platform") || collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            varB++;
            camAnim.SetTrigger("shake");
            Handheld.Vibrate();
        }

        if (collision.gameObject.CompareTag("Coins"))
        {
            varC++;
            Destroy(collision.gameObject);
            float a = Screen.width;
        }
    }

    private IEnumerator DisableColPlat()
    {
        BoxCollider2D platCol = currentPlat.GetComponent<BoxCollider2D>();
        Physics2D.IgnoreCollision(playerCol, platCol);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(playerCol, platCol, false);
    }

    IEnumerator Aaaa()
    {
        condicao = false;

        int rnd = Random.Range(0, 3);
        float a = numbers[rnd];

        int rnd2 = Random.Range(0, 3);
        float a2 = numbers[rnd2];

        if(rnd != rnd2)
        {
            Instantiate(inimigoObj, new(5, a2, 0), Quaternion.identity);
        }

        Instantiate(inimigoObj, new(5, a, 0), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);
        Debug.Log("Entrou");

        int rndC = Random.Range(0, 5);
        float aC = coins[rndC];

        Instantiate(coinsObj, new(5, aC, 0), Quaternion.identity);

        float rndS = Random.Range(0.5f, 2);

        yield return new WaitForSeconds(rndS);
        condicao = true;
        Debug.Log("Saiu");
    }

    public void ResetTxt()
    {
        varPN = 0;
        varPA = 0;
        varA = 0;
        varR = 0;
        varB = 0;
        varC = 0;
        transform.position = new(-1.86f, -2.87f, 1);
    }

    public void TextMeshProGUI()
    {
        txt.text = "Pulo normal: " + varPN + "\nPulo alto: " + varPA + "\nRolar: " + varA + "\nDescer: " + varR + "\nBateu: " + varB + "\nMoedas: " + varC;
        //txt.text = "Pulo normal: " + varPN + "\nPulo alto: " + varPA + "\nAbaixar: " + varA + "\nRolar: " + varR + "\nBateu: " + varB + "\nMoedas: " + varC;
    }
    
    public void Swipe()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPos = Input.GetTouch(0).position;
            Debug.Log(Input.GetTouch(0).position.y);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentTouchPos = Input.GetTouch(0).position;
            Vector2 Distance = currentTouchPos - startTouchPos;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stopTouuch = false;
            endTouchPos = Input.GetTouch(0).position;
            Vector2 Distance = endTouchPos - startTouchPos;

            Debug.Log(Distance.y);

            if((Mathf.Abs(Distance.x) < tapRange && Mathf.Abs(Distance.y) < tapRange) && isGrounded == true)//Toque = pulo
            {
                stopTouuch = true;
                rb.velocity = Vector2.up * jumpForce;
                varPN++;
            }
            if ((Distance.y > swipeRangeS) && isGrounded == true)//Swipe Up = pulo alto 
            {
                stopTouuch = true;
                rb.velocity = Vector2.up * (jumpForce * multiplicador);
                varPA++;
            }
            else if ((Distance.y < -swipeRangeS && currentPlat == null) && isGrounded == true)//Swipe Down (no chao) = rolar
            {
                stopTouuch = true;
                varA++;

                playerAnim.SetTrigger("Roll");
            }
            else if ((Distance.y < -swipeRangeS && currentPlat != null) && isGrounded == true)//Swipe Down (na plataforma) = descer
            {
                stopTouuch = true;
                varR++;

                if(currentPlat != null)
                {
                    StartCoroutine(DisableColPlat());
                    rb.velocity = -Vector2.up * jumpForce;
                }
            }
        }
    }

    public void Swipe1()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPos = Input.GetTouch(0).position;
            Debug.Log(Input.GetTouch(0).position.y);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentTouchPos = Input.GetTouch(0).position;
            Vector2 Distance = currentTouchPos - startTouchPos;

            if (!stopTouuch)
            {
                /*
                if (Distance.y > (swipeRange * 2))
                {
                    Debug.Log("pulo duplo");
                    stopTouuch = true;
                    //transform.position += (Vector3)Vector2.up*4;
                    rb.velocity = Vector2.up * 6;
                    varPA++;
                }
                else if(Distance.y > swipeRange && Distance.y < (swipeRange * 2))
                {
                    Debug.Log("pulou");
                    stopTouuch = true;
                    //transform.position += (Vector3)Vector2.up;
                    rb.velocity = Vector2.up * 3;
                    varPN++;
                }
                else if (Distance.y < -swipeRange && Distance.y > (-swipeRange * 2))
                {
                    Debug.Log("abaixou");
                    stopTouuch = true;
                    //transform.position += (Vector3)Vector2.up;
                    //rb.velocity = Vector2.up * 3;
                    varA++;
                }
                else if (Distance.y < (-swipeRange * 2))
                {
                    Debug.Log("rolou");
                    stopTouuch = true;
                    //transform.position += (Vector3)Vector2.up;
                    //rb.velocity = Vector2.up * 3;
                    varR++;
                }
                //*/
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stopTouuch = false;
            endTouchPos = Input.GetTouch(0).position;
            Vector2 Distance = endTouchPos - startTouchPos;

            Debug.Log(Distance.y);

            if (Mathf.Abs(Distance.x) < tapRange && Mathf.Abs(Distance.y) < tapRange)
            {
                Debug.Log("Toque");
                txt.text = "AAAAAAAAA";
            }
            if (Distance.y > swipeRangeL)
            {
                Debug.Log("pulo duplo");
                stopTouuch = true;
                //transform.position += (Vector3)Vector2.up*4;
                rb.velocity = Vector2.up * (jumpForce * multiplicador);
                varPA++;
            }
            else if (Distance.y > swipeRangeS && Distance.y < swipeRangeL)
            {
                Debug.Log("pulou");
                stopTouuch = true;
                //transform.position += (Vector3)Vector2.up;
                rb.velocity = Vector2.up * jumpForce;
                varPN++;
            }
            else if (Distance.y < -swipeRangeS && Distance.y > -swipeRangeL)
            {
                Debug.Log("abaixou");
                stopTouuch = true;
                //transform.position += (Vector3)Vector2.up;
                //rb.velocity = Vector2.up * 3;
                varA++;
                playerAnim.SetTrigger("Roll");
            }
            else if (Distance.y < -swipeRangeL)
            {
                Debug.Log("rolou");
                stopTouuch = true;
                //transform.position += (Vector3)Vector2.up;
                //rb.velocity = Vector2.up * 3;
                varR++;

                if (currentPlat != null)
                {
                    StartCoroutine(DisableColPlat());
                }
            }
        }
    }


    /* Método 2 (Menos complexo mas com menos informações)
    private void Update()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if(t.phase == TouchPhase.Moved)
            {
                if (t.deltaPosition.y > 75)
                {
                    //transform.position += Vector3.up * 2;
                    rb.velocity = Vector2.up * 8;
                    Debug.Log("pulo duplo");
                }
                else if (t.deltaPosition.y > 25 && t.deltaPosition.y < 75)
                {
                    //transform.position += Vector3.up / 2;
                    rb.velocity = Vector2.up * 4;
                    Debug.Log("pulou");
                }
            }
        }
    }
    */
}
