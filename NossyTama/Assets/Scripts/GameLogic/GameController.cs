using UnityEngine;
using System.Collections;
using UnityEngine.Events;


public class GameController : MonoBehaviour
{
    [Header("Day Night Cycle Variables")]
    public bool isNight;
    public int dayLength = 60;           
    private int halfDayLength;          
    public float dayNightTimer;       
    public GameObject skyObject;


    [Header("Curtain Variables")]
    public GameObject curtainObject;  
    public bool areCurtainsClosed;    
    public Animator curtainAnimator;

    [Header("Life Variables")]
    public bool isDead;

    [Header("Sun Death Variables")]
    public float maxTimeInSun = 10f;
    public float sunExposureTimer;

    [Header("Death event systems")]
    public UnityEvent OnDeath;

    public UnityEvent OnBurn;
    public UnityEvent OnStarve;
    public UnityEvent OnDepression;

    void Start()
    {
        halfDayLength = dayLength / 2;
        dayNightTimer = halfDayLength;
        isNight = false;

        sunExposureTimer = 0f;
        isDead = false;

        areCurtainsClosed = true;

        StartCoroutine(DayNightToggle());
    }

    void Update()
    {
        dayNightTimer -= Time.deltaTime;
        RotateSkyObjectOverDay();

        if (!isDead) 
        {

            if (!isNight && !areCurtainsClosed) 
            {
                sunExposureTimer += Time.deltaTime;

               
                if (sunExposureTimer >= maxTimeInSun)
                {
                    DieFromSunExposure();
                }
            }
            else if (isNight) 
            {
                sunExposureTimer = 0f;
            }
        }
    }

   
    IEnumerator DayNightToggle()
    {
        while (true)
        {
            if (dayNightTimer <= 0)
            {
                isNight = !isNight; 
                dayNightTimer = halfDayLength; 

                if (!isNight)
                {
                    OpenCurtains();
                    Debug.Log("Day has started, curtains are opening.");
                }
                else
                {
                    Debug.Log("Night has started.");
                }
            }
            yield return null;
        }
    }

    void RotateSkyObjectOverDay()
    {
        if (skyObject != null)
        {
            float rotationSpeed = 360f / dayLength;
            skyObject.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }

    public void ToggleCurtains()
    {
        if (!areCurtainsClosed)
        {
            CloseCurtains();
        }
        else
        {
            OpenCurtains();
        }
    }

    void OpenCurtains()
    {
        areCurtainsClosed = false;
        curtainAnimator.SetTrigger("CurtainOpen");
        Debug.Log("Curtains are opened.");
    }

    void CloseCurtains()
    {
        areCurtainsClosed = true;
        curtainAnimator.SetTrigger("CurtainClose");
        Debug.Log("Curtains are closed.");
    }

    void DieFromSunExposure()
    {
        isDead = true;
        Debug.Log("You have died from sun exposure.");
        Die();

        OnBurn.Invoke();

    }

    void Die()
    {
        OnDeath.Invoke();
    }
}