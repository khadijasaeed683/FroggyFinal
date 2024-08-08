using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

//SceneManagement is Required at line#123
public class PlayerJump : MonoBehaviour
{
    public float jumpForceMultiplier = 0.1f; // Adjust the jump force multiplier
    public float dragThreshold = 1f; // Minimum drag distance to trigger a jump
    public float maxDragDistance = 100f; // Maximum drag distance
    public LineRenderer lineRenderer; // Reference to the LineRenderer
    public int trajectoryResolution = 30; // Number of points in the trajectory line
    public GameObject Player;
    public Animator anim;
    public  Animator PrAnim;
    public AudioSource audioSource;
    public AudioClip audioClip;
    public GameObject  losePanel;
    public GameObject  winPanel;
    
    public SliderEventHandler sliderHandler;
    public TextMeshProUGUI Scoretext;
    public TextMeshProUGUI WinScoretext;
    public TextMeshProUGUI LoseScoretext;


    private Rigidbody rb;
    private bool dragSoundController;
    private Vector2 dragStartPosition;
    private bool isDragging = false;
    private bool isColliding = false;
    private  int ScoreCount=0;
    private bool PrincessCol=false;



    void Start()

    {  
        
        Time.timeScale = 1.0f;
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        rb = GetComponent<Rigidbody>();
        lineRenderer.positionCount = trajectoryResolution;
        Scoretext.text="Score :"+ScoreCount.ToString();
        WinScoretext.text="Score :"+ScoreCount.ToString();
        LoseScoretext.text="Score :"+ScoreCount.ToString();


        if (sliderHandler == null)
        {
            sliderHandler = FindObjectOfType<SliderEventHandler>();
            if (sliderHandler == null)
            {
                Debug.LogWarning("SliderEventHandler not found in the scene.");
            }
        }
    }

    void Update()
    {
        if (sliderHandler != null && sliderHandler.sliderMoving) return; // Skip jump and trajectory logic if using the slider

        AddDragSound();

        // Jump animation
        anim.SetBool("jump", !isColliding);
        //Princess Animation 
        PrAnim.SetBool("Variable", PrincessCol);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    dragStartPosition = touch.position;
                    isDragging = true;
                    dragSoundController=false;
                    break;

                case TouchPhase.Moved:
                dragSoundController=true;
                    if (isDragging)
                    {
                        Vector2 dragCurrentPosition = touch.position;
                        Vector2 dragVector = dragStartPosition - dragCurrentPosition;

                        // Limit the drag distance
                        if (dragVector.magnitude > maxDragDistance)
                        {
                            dragVector = dragVector.normalized * maxDragDistance;
                        }

                        if (dragVector.magnitude > dragThreshold)
                        {
                            Vector3 jumpDirection = new Vector3(dragVector.x, dragVector.y, dragVector.y).normalized;
                            float jumpForce = dragVector.magnitude * jumpForceMultiplier;
                            ShowTrajectory(Player.transform.position, jumpDirection * jumpForce);
                        }
                    }
                    break;

                case TouchPhase.Ended:
                dragSoundController=false;
                    if (isDragging)
                    {
                        Vector2 dragEndPosition = touch.position;
                        Vector2 dragVector = dragStartPosition - dragEndPosition;

                        // Limit the drag distance
                        if (dragVector.magnitude > maxDragDistance)
                        {
                            dragVector = dragVector.normalized * maxDragDistance;
                        }

                        if (dragVector.magnitude > dragThreshold)
                        {
                            Vector3 jumpDirection = new Vector3(dragVector.x, dragVector.y, dragVector.y).normalized;
                            float jumpForce = dragVector.magnitude * jumpForceMultiplier;
                            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
                        }

                        lineRenderer.positionCount = 0; // Hide the trajectory line
                        isDragging = false;
                    }
                    break;
            }
        }
    }

    void ShowTrajectory(Vector3 start, Vector3 velocity)
    {
        Vector3[] points = new Vector3[trajectoryResolution];
        float timeStep = 0.1f;
        for (int i = 0; i < trajectoryResolution; i++)
        {
            float time = i * timeStep;
            points[i] = start + velocity * time + 0.5f * Physics.gravity * time * time;
        }
        lineRenderer.positionCount = points.Length;
        lineRenderer.SetPositions(points);
    }

    private void OnCollisionEnter(Collision other)
    {
        isColliding = true;
        if (other.gameObject.tag == "Water"|| other.gameObject.tag=="Bird" ){
            LoseScoretext.text = "Score :" + ScoreCount.ToString();
            losePanel.SetActive(true);
            winPanel.SetActive(false);
            Time.timeScale = 0f;
        }
        else if (other.gameObject.tag == "Princess"){
            winPanel.SetActive(true);
            WinScoretext.text = "Score :" + ScoreCount.ToString();
            losePanel.SetActive(false);
            PrincessCol=true;
            Time.timeScale = 0f;
        }
        
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Coin"){
            ScoreCount +=10;
            Scoretext.text="Score :"+ScoreCount.ToString();
            Destroy(other.gameObject);
        }
    }

    private void OnCollisionExit(Collision other)
    {
        isColliding = false;
    }

    public void OnCollisionStay(Collision other)
    {
        isColliding = true;
    }

    private void AddDragSound()
    {
        if (dragSoundController)
        {
            if (audioSource != null && audioClip != null)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
            }
        }
    }
}
