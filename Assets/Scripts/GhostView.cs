using Unity.VisualScripting;
using UnityEngine;

public class GhostView : MonoBehaviour
{
    [SerializeField] private PlayerInputHandler playerInputHandler;

    public GameObject normalPostProcess;
    public GameObject GhostPostProcess;
    private bool normalViewActive = true;

    public GameObject bloodDecals;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleGhostView();
    }
    private void HandleGhostView()
    {
        if (playerInputHandler.GhostTriggered && normalViewActive)
        {
            normalViewActive = false;
            GhostPostProcess.SetActive(true);
            normalPostProcess.SetActive(false);
            bloodDecals.SetActive(true);
        }
    }
}
