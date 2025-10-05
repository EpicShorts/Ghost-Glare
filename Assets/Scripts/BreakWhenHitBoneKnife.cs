using UnityEngine;

public class BreakWhenHitBoneKnife : MonoBehaviour
{
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private bool hasFallen = false;

    [SerializeField] private FallWhenBoardsBreak fallWhenBoardsBreak;

    public AudioSource sawNoise;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "BoneKnife" && !hasFallen)
        {
            hasFallen = true;
            fallWhenBoardsBreak.numberOfBoardsLeft--;
            //other.rigidbody.freezeRotation = false;
            rb.constraints = RigidbodyConstraints.None;
            gameObject.layer = LayerMask.NameToLayer("Player");
            boxCollider.excludeLayers = LayerMask.GetMask("Player");
            sawNoise.Play();
            Destroy(gameObject, 5f);
        }
    }
}
