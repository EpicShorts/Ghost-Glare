using System.Collections.Generic;
using UnityEngine;

public class HowManyObjectsInPulley : MonoBehaviour
{
    [Tooltip("Only objects with this tag will be counted.")]
    public string targetTag = "Meat";

    private readonly HashSet<GameObject> objectsInside = new HashSet<GameObject>();

    public int Count => objectsInside.Count;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            objectsInside.Add(other.gameObject);
            Debug.Log("Meat Found");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
            objectsInside.Remove(other.gameObject);
    }
}
