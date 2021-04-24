using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TriggerManager : MonoBehaviour
{
    [SerializeField] public UnityEvent OnTriggerEvent;
    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEvent?.Invoke();
    }
}
