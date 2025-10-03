using UnityEngine;
using System.Collections;

public class InteractableStatic : MonoBehaviour
{
    [SerializeField] public char whichWayToOpen = 'a';
    [SerializeField] public bool minusDirection = true;

    private bool hasOpened = false;
    private float directionChangeMultiply = 1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Interact()
    {
        if (!hasOpened)
        {
            hasOpened = true;
            RotateWhichDirection(whichWayToOpen, true);
            //RotateInDirection(new Vector3(-90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 1f);
            //Debug.Log("Rotating into: -90f, " + transform.rotation.eulerAngles.y + ", " + transform.rotation.eulerAngles.z);
        }
        else
        {
            hasOpened = false;
            RotateWhichDirection(whichWayToOpen, false);
            //RotateInDirection(new Vector3(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 1f);
        }
    }

    private void RotateWhichDirection(char axis, bool opening)
    {
        if (minusDirection)
        {
            directionChangeMultiply = 1f;
        }
        else 
        {
            directionChangeMultiply = -1f;
        }
        switch (axis) {
            case 'x':
                if (opening)
                    RotateInDirection(new Vector3(transform.rotation.eulerAngles.x - (directionChangeMultiply * 90f), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 1f);
                else
                    RotateInDirection(new Vector3(transform.rotation.eulerAngles.x + (directionChangeMultiply * 90f), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z), 1f);
                break;
            case 'y':
                if (opening)
                    RotateInDirection(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y - (directionChangeMultiply * 90f), transform.rotation.eulerAngles.z), 1f);
                else
                    RotateInDirection(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y + (directionChangeMultiply * 90f), transform.rotation.eulerAngles.z), 1f);
                break;
            case 'z':
                if (opening)
                    RotateInDirection(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z - (directionChangeMultiply * 90f)), 1f);
                else
                    RotateInDirection(new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + (directionChangeMultiply * 90f)), 1f);
                break;
            default:
                Debug.Log("Direction axis not found");
                break;
        }
    }


    private void RotateInDirection(Vector3 eulerAngles, float duration = 1f)
    {
        StartCoroutine(RotateOverTime(eulerAngles, duration));
    }

    private IEnumerator RotateOverTime(Vector3 eulerAngles, float duration)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(eulerAngles);

        float time = 0f;
        while (time < duration)
        {
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // snap exactly to target at the end
        transform.rotation = targetRotation;
    }
}
