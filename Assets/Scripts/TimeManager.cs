using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] float decreaseSpeed;
    [SerializeField] AnimationCurve slowMotionValue;

    float slowMotion=0;

    public static TimeManager instance;
    public static TimeManager Instance { get { return instance; } }
    float time = 0;
    

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void SlowMotion(ref float slowMotionAmmount, bool isActivated)
    {
        
        if (isActivated)
        {
            if(time<1)
                time += Time.timeScale;
            slowMotion = slowMotionValue.Evaluate(time);
            slowMotionAmmount -= Time.deltaTime * decreaseSpeed;
        }
        else
        {
            if(time>0)
                time -= Time.timeScale /60;
            slowMotion = slowMotionValue.Evaluate(time);
        }
        Time.timeScale = slowMotion;
    }

}
