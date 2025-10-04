using UnityEngine;

public class FallWhenBoardsBreak : MonoBehaviour
{
    public int numberOfBoardsLeft = 5;

    private Rigidbody rb;
    private BoxCollider boxCollider;
    private bool hasFallen = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (numberOfBoardsLeft == 0 && !hasFallen)
        {
            hasFallen = true;
            rb.constraints = RigidbodyConstraints.None;
            gameObject.layer = LayerMask.NameToLayer("Player");
            boxCollider.excludeLayers = LayerMask.GetMask("Player");
            Destroy(gameObject, 5f);
        }
    }
}
