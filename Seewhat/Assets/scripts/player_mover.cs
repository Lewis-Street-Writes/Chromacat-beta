using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class player_mover : MonoBehaviour
{
    public gun gun;
    public gun_values gun_values;
    public powerup powerup;

    public int maxhealth=100;
    public int health;
    [SerializeField] public Text Currenthealth;
    public bool isalive;

    [SerializeField] public Transform CylinderCamera = null;
    [SerializeField] public Transform CylinderCube = null;
    [SerializeField] float mouseSensitivity = 5.0f;
    [SerializeField] float walkspeed= 8.0f;
    [SerializeField] float gravity=-9.8f;
    [SerializeField][Range(0.0f,0.5f)] float moveSmoothTime=0.3f;
    [SerializeField][Range(0.0f,0.5f)] float mouseSmoothTime=0.03f;

    [SerializeField] bool lockCursor= true;

    float cameraPitch = 0.0f;
    float velocityY=0.0f;
    CharacterController controller = null;

    [SerializeField] AnimationCurve jumpFallOff;
    [SerializeField] float jumpPower=50.0f;
    [SerializeField] KeyCode jumpKey;
    bool isJumping;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        health=maxhealth;
        Currenthealth.text=health.ToString();
        isalive=true;
        gun=gun_values.gun;
        CylinderCube=gun_values.pistol.transform;
        controller = GetComponent<CharacterController>();
        if(lockCursor) {
            Cursor.lockState=CursorLockMode.Locked;
            Cursor.visible=false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (health==0) {
        playerdeath();
        }
        Mover();
        StartCoroutine(MouseLook());
        Jump();
        StartCoroutine(gun.shoot());
    }
    public IEnumerator MouseLook() 
    {
        Vector2 targetMouseDelta= new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        CylinderCube.transform.localPosition = new Vector3(1.0f,(-cameraPitch/400*1.0f)+1.08f,-(Mathf.Abs(cameraPitch)/100*1.0f)+1.1f);

        CylinderCamera.transform.localEulerAngles = Vector3.right * cameraPitch;
        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);

        if(currentMouseDelta.x>= 0.05f * mouseSensitivity) {
         CylinderCube.localEulerAngles = new Vector3((cameraPitch/2.7f -gun_values.shootValueY),-gun_values.shootValueX -20,-3f); ;
        };
        if(currentMouseDelta.x<= -0.05f * mouseSensitivity) {
         CylinderCube.localEulerAngles = new Vector3((cameraPitch/2.7f -gun_values.shootValueY),-gun_values.shootValueX -20,3f); ;
        };
        if(currentMouseDelta.x>= -0.05f * mouseSensitivity && currentMouseDelta.x<= 0.05f * mouseSensitivity ) {
        CylinderCube.localEulerAngles = new Vector3((cameraPitch/2.7f -gun_values.shootValueY),-gun_values.shootValueX -20,0); ;
        };
        yield return null;
        
    }
    void Mover()
    {
        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"),Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        if(controller.isGrounded){
            velocityY=0.0f;
        }
        velocityY+= gravity* Time.deltaTime;
        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        Vector3 velocity= (transform.forward * currentDir.y + transform.right * currentDir.x) * (walkspeed + powerup.totalheat/50) + Vector3.up *velocityY;
        
        controller.Move(velocity * Time.deltaTime);
    }
    void Jump() 
    {
    if(Input.GetKeyDown(jumpKey) && !isJumping){
            isJumping=true;
            StartCoroutine(Jumplist());
    }

    IEnumerator Jumplist() 
    {
         
            float timeInAir = 0.0f;
            do {
                float jumpForce= jumpFallOff.Evaluate(timeInAir);
                controller.Move(Vector3.up* jumpPower * Time.deltaTime);
                timeInAir+=Time.deltaTime;
                yield return null;
            } while (!controller.isGrounded && controller.collisionFlags != CollisionFlags.Above);
            isJumping = false;
            }
    }  
     void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.collider.name=="plane") {
            return;
        }
        if (collision.collider.name=="heatpool") {
            powerup.increaseheat();
        }
        if (collision.collider.name=="coldpool") {
            powerup.decreaseheat();
        }
    }
    public void playertakeDamage(int enemydamage) {
        health+= -enemydamage;
        Currenthealth.text=health.ToString();
    }
    void playerdeath() {
        playerrespawn();
    }
    void playerrespawn() {
    health=100;
    Currenthealth.text=health.ToString();
    }
}