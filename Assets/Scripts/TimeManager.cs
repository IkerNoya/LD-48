using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] float decreaseSpeed;
    [SerializeField] AnimationCurve slowMotionValue;

    TimeManager instance;
    public TimeManager Instance { get { return instance; } }
    

    void Awake()
    {
        if (instance == null)
            instance = this;
    }

    void SlowMotion(ref float slowMotionAmmount)
    {
        slowMotionAmmount -= Time.deltaTime * decreaseSpeed;
        Time.timeScale = slowMotionValue.Evaluate(Time.deltaTime);
    }

}
