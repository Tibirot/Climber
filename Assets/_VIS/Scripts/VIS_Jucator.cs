using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VirtualInfinityStudios;
using DigitalRubyShared;
/***********************************
 * CopyRight 2019
 * Programmer: Buraca Dorin
 * Programmer: Socea Tiberiu
 * Website: http://www.VirtualInfinityStudios.ro
 * Game: Climber
 *  ***********************************/
public class VIS_Jucator : MonoBehaviour
{

    public bool folosimShake = false;
    public bool esteInDreapta = true;
    public bool esteInStanga = false;

    public Vector3 rotDreapta = new Vector3(-27.0f, -90.0f, 0.0f);
    public Vector3 rotStanga = new Vector3(27.0f, -90.0f, 0.0f);
    public float viteza = 50.0f;
    public float pozitieCurentaY = 0.0f;
    public float distantaParcursa = 0.0f;
    public float pozitieInitiala = 0.0f;

    //ANIMATII & PHYSICS
    private Animator animCaracter;
    private Rigidbody rbCaracter;

    // TOCUH GESTURE RECOGNIZER
    private SwipeGestureRecognizer swipeGesture;
    private readonly List<Vector3> swipeLines = new List<Vector3>();
    private TapGestureRecognizer tapGesture;
    private TapGestureRecognizer doubleTapGesture;

    private void Awake()
    {
        if (animCaracter == null || rbCaracter == null)
        {
            animCaracter = GetComponent<Animator>();
            rbCaracter = GetComponent<Rigidbody>();
        }
    }


    private void Start()
    {
        //Handler Gesturi TouchScreen
        //CreateDoubleTapGesture();
        CreateTapGesture();
        CreateSwipeGesture();
    }

    private void Update()
    {


        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    MoarteCaracter();
        //}


    }

    private void FixedUpdate()
    {
        var directie = new Vector3(0, viteza, 0);
        transform.position += directie * Time.deltaTime;

        pozitieCurentaY = transform.position.y;
        distantaParcursa = Mathf.RoundToInt((pozitieCurentaY / 4) - pozitieInitiala);
    }

    private void LateUpdate()
    {
        int touchCount = Input.touchCount;
        if (FingersScript.Instance.TreatMousePointerAsFinger && Input.mousePresent)
        {
            touchCount += (Input.GetMouseButton(0) ? 1 : 0);
            touchCount += (Input.GetMouseButton(1) ? 1 : 0);
            touchCount += (Input.GetMouseButton(2) ? 1 : 0);
        }
        string touchIds = string.Empty;
        int gestureTouchCount = 0;
        foreach (GestureRecognizer g in FingersScript.Instance.Gestures)
        {
            gestureTouchCount += g.CurrentTrackedTouches.Count;
        }
        foreach (GestureTouch t in FingersScript.Instance.CurrentTouches)
        {
            touchIds += ":" + t.Id + ":";
        }

    }



    #region INPUT_CONTROL_PLAYER

    private void TapGestureCallback(GestureRecognizer gesture)
    {
        //if (gesture.State == GestureRecognizerState.Ended)
        //{
        //    DebugText("Tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
        //}

        if (gesture.State == GestureRecognizerState.Ended)
        {
            if (esteInStanga)
            {
                CaracterLaDreapta();
            }
            else if (esteInDreapta)
            {
                CaracterLaStanga();
            }
        }


    }

    private void CreateTapGesture()
    {
        tapGesture = new TapGestureRecognizer();
        tapGesture.StateUpdated += TapGestureCallback;
        tapGesture.RequireGestureRecognizerToFail = doubleTapGesture;
        FingersScript.Instance.AddGesture(tapGesture);
    }



    private void DoubleTapGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            DebugText("Double tapped at {0}, {1}", gesture.FocusX, gesture.FocusY);
        }
    }

    private void CreateDoubleTapGesture()
    {
        doubleTapGesture = new TapGestureRecognizer();
        doubleTapGesture.NumberOfTapsRequired = 2;
        doubleTapGesture.StateUpdated += DoubleTapGestureCallback;
        FingersScript.Instance.AddGesture(doubleTapGesture);
    }



    private void HandleSwipe(float endX, float endY)
    {
        Vector2 start = new Vector2(swipeGesture.StartFocusX, swipeGesture.StartFocusY);
        Vector3 startWorld = Camera.main.ScreenToWorldPoint(start);
        Vector3 endWorld = Camera.main.ScreenToWorldPoint(new Vector2(endX, endY));
        float distance = Vector3.Distance(startWorld, endWorld);
        startWorld.z = endWorld.z = 0.0f;

        swipeLines.Add(startWorld);
        swipeLines.Add(endWorld);

        if (swipeLines.Count > 4)
        {
            swipeLines.RemoveRange(0, swipeLines.Count - 4);
        }

        RaycastHit2D[] collisions = Physics2D.CircleCastAll(startWorld, 10.0f, (endWorld - startWorld).normalized, distance);

        if (collisions.Length != 0)
        {
            Debug.Log("Raycast hits: " + collisions.Length + ", start: " + startWorld + ", end: " + endWorld + ", distance: " + distance);

            Vector3 origin = Camera.main.ScreenToWorldPoint(Vector3.zero);
            Vector3 end = Camera.main.ScreenToWorldPoint(new Vector3(swipeGesture.VelocityX, swipeGesture.VelocityY, Camera.main.nearClipPlane));
            Vector3 velocity = (end - origin);
            Vector2 force = velocity * 500.0f;

            foreach (RaycastHit2D h in collisions)
            {
                h.rigidbody.AddForceAtPosition(force, h.point);
            }
        }
    }

    private void SwipeGestureCallback(GestureRecognizer gesture)
    {
        if (gesture.State == GestureRecognizerState.Ended)
        {
            HandleSwipe(gesture.FocusX, gesture.FocusY);
            //DebugText("Swiped from {0},{1} to {2},{3}; velocity: {4}, {5}", gesture.StartFocusX, gesture.StartFocusY, gesture.FocusX, gesture.FocusY, swipeGesture.EndDirection, swipeGesture.VelocityY);


            if (swipeGesture.EndDirection == SwipeGestureRecognizerDirection.Up)
            {
                viteza = 500.0f;
            }
            else if (swipeGesture.EndDirection == SwipeGestureRecognizerDirection.Down)
            {
                viteza = 200.0f;
            }


        }
    }

    private void CreateSwipeGesture()
    {
        swipeGesture = new SwipeGestureRecognizer();
        swipeGesture.Direction = SwipeGestureRecognizerDirection.Any;
        swipeGesture.StateUpdated += SwipeGestureCallback;
        swipeGesture.DirectionThreshold = 1.0f; // allow a swipe, regardless of slope
        FingersScript.Instance.AddGesture(swipeGesture);
    }



    private void DebugText(string text, params object[] format)
    {
        Debug.Log(string.Format(text, format));
    }



    private void CaracterLaDreapta()
    {
        transform.position = new Vector3(0.65f, pozitieCurentaY + 0.2f, 0.0f);
        transform.rotation = Quaternion.Euler(rotDreapta);
        esteInDreapta = true;
        esteInStanga = false;
        ShakeCamera();
    }


    private void CaracterLaStanga()
    {
        transform.position = new Vector3(0.0f, pozitieCurentaY + 0.2f, -1.0f);
        transform.rotation = Quaternion.Euler(rotStanga);
        esteInStanga = true;
        esteInDreapta = false;
        ShakeCamera();
    }


    private void ShakeCamera()
    {
        if (folosimShake)
        {
            VIS_ManagerGP.Instance.Zguduie("SchimbareDirectie");
        }
        else
        {
            return;
        }
    }

    #endregion





    private void MoarteCaracter()
    {
        VIS_ManagerGP.Instance.JucatorMort();
        animCaracter.SetBool("InCadere", true);

        //if (esteInDreapta)
        //{
        //    transform.position = new Vector3(0.9f, 5.0f, 0.0f);
        //    transform.rotation = Quaternion.Euler(-25.0f, -133.0f, 0.0f);
        //}

        //else if (esteInStanga)
        //{
        //    transform.position = new Vector3(0.9f, 5.0f, 0.0f);
        //    transform.rotation = Quaternion.Euler(25.0f, 33.0f, 0.0f);
        //}
        //rbCaracter.useGravity = true;

    }


    public void OnCollisionEnter(Collision obiect)
    {
        if (obiect.gameObject.CompareTag("VIS/Obstacole"))
        {
            MoarteCaracter();
        }
    }
}
