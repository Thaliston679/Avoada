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

    public float swipeRange;
    //public float smallJumpRange;
    //public float largeJumpRange;
    public float tapRange;

    Rigidbody2D rb;

    public TextMeshProUGUI txt;

    private int varPN, varPA, varA, varR, varEA;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Swipe();
        TextMeshProGUI();
    }

    public void ResetTxt()
    {
        varPN = 0;
        varPA = 0;
        varA = 0;
        varR = 0;
        varEA = 0;
    }

    public void TextMeshProGUI()
    {
        txt.text = "Pulo normal: " + varPN + "\nPulo alto: " + varPA + "\nAbaixar: " + varA + "\nRolar: " + varR;
    }

    public void Swipe()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            startTouchPos = Input.GetTouch(0).position;
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            currentTouchPos = Input.GetTouch(0).position;
            Vector2 Distance = currentTouchPos - startTouchPos;

            if (!stopTouuch)
            {
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
            }
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            stopTouuch = false;
            endTouchPos = Input.GetTouch(0).position;
            Vector2 Distance = endTouchPos - startTouchPos;

            if(Mathf.Abs(Distance.x) < tapRange && Mathf.Abs(Distance.y) < tapRange)
            {
                Debug.Log("Toque");
                txt.text = "AAAAAAAAA";
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
