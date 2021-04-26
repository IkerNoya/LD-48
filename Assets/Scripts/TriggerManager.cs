using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class TriggerManager : MonoBehaviour
{
    [SerializeField] public UnityEvent OnTriggerEvent;

    [SerializeField] private string compareTag;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(compareTag))
        {
            OnTriggerEvent?.Invoke();
        }
    }
}
