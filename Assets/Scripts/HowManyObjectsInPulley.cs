using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowManyObjectsInPulley : MonoBehaviour
{
    [Tooltip("Only objects with this tag will be counted.")]
    public string targetTag = "Meat";

    private readonly HashSet<GameObject> objectsInside = new HashSet<GameObject>();

    public PickUpScript pickUpScript;

    public BoxCollider boxCollider;

    public int Count => objectsInside.Count;

    private Vector3 startPosition;
    private Vector3 currentPosition;
    private Vector3 currentOtherChainPosition;
    private Vector3 startOtherChainPosition;

    public Transform otherChain;

    public AudioSource chainNoise;
    private int noiseCounter = 0;

    void Start()
    {
        startPosition = transform.position;
        startOtherChainPosition = otherChain.position;
    }
    void Update()
    {
        // change to be called from pickupscript
        if (pickUpScript.heldObj != null)
        {
            boxCollider.enabled = false;
        }
        else
        {
            boxCollider.enabled = true;
        }

        LowerChainRaiseChain();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            objectsInside.Add(other.gameObject);
            PlayNoiseCorrectly();
            StartCoroutine(UpdateChainPositionsGradually(2f));
            Debug.Log("Meat Found, Count: "+Count);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            objectsInside.Remove(other.gameObject);
            PlayNoiseCorrectly();
            StartCoroutine(UpdateChainPositionsGradually(2f));
            Debug.Log("Meat Lost, Count: " + Count);
        }
    }

    private void PlayNoiseCorrectly()
    {
        if (noiseCounter != Count)
        {
            noiseCounter = Count;
            chainNoise.Play();
        }
    }

    private void LowerChainRaiseChain()
    {
        //StartCoroutine(UpdateChainPositionsGradually(2f));
    }

    private IEnumerator UpdateChainPositionsGradually(float duration)
    {
        currentPosition = transform.position;
        currentOtherChainPosition = otherChain.position;
        float time = 0f;
        while (time < duration)
        {
            transform.position = Vector3.Slerp(currentPosition, new Vector3(startPosition.x,startPosition.y-Count,startPosition.z), time / duration);
            otherChain.position = Vector3.Slerp(currentOtherChainPosition, new Vector3(startOtherChainPosition.x, startOtherChainPosition.y + Count, startOtherChainPosition.z), time / duration);
            time += Time.deltaTime;
            yield return null;
        }
    }

}
