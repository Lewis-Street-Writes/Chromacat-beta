using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class player_mover : MonoBehaviour
{
    public gun gun;
    public gun_values gun_values;
    public powerup powerup;

    public int maxhealth=100;
    public int health;
    [SerializeField] public Text Currenthealth;
     GameObject settingpause;
    GameObject quitpause;
     GameObject pausecover;
     GameObject crosshair1;
    GameObject crosshair2;
    GameObject crosshair3;
    GameObject crosshair4;

    public bool isalive;

    public bool ispaused=false;

    Transform pseudocam;
    float currentcamy=0.0f;
    float currentcamx=0.0f;

    [SerializeField] public Transform CylinderCamera = null;
    [SerializeField] public Transform CylinderCube = null;
    [SerializeField] float mouseSensitivity = 5.0f;
    [SerializeField] float walkspeed= 8.0f;
    [SerializeField] float gravity=-9.8f;
    [SerializeField][Range(0.0f,0.5f)] float moveSmoothTime=0.3f;
    [SerializeField][Range(0.0f,0.5f)] float mouseSmoothTime=0.03f;

    [SerializeField] bool lockCursor= true;

    public float cameraPitch = 0.0f;
    float velocityY=0.0f;
    CharacterController controller = null;

    [SerializeField] AnimationCurve jumpFallOff;
    [SerializeField] float jumpPower=50.0f;
    bool isJumping;

    Vector2 currentDir = Vector2.zero;
    Vector2 currentDirVelocity = Vector2.zero;

    Vector2 currentMouseDelta = Vector2.zero;
    Vector2 currentMouseDeltaVelocity = Vector2.zero;

    public AudioSource music;

    // Start is called before the first frame update
    void Start()
    {
        music=gameObject.GetComponent<AudioSource>();
        music.pitch=0.7f;
        health=maxhealth;
        Currenthealth.text=health.ToString();
        isalive=true;
        gun=gun_values.gun;
        CylinderCube=gun_values.pistol.transform;
        controller = GetComponent<CharacterController>();
        cursorlock();
        settingpause=GameObject.Find("settingpause");
        quitpause=GameObject.Find("quitpause");
        pausecover=GameObject.Find("pausecover");
        crosshair1=GameObject.Find("crosshair1");
        crosshair2=GameObject.Find("crosshair2");
        crosshair3=GameObject.Find("crosshair3");
        crosshair4=GameObject.Find("crosshair4");
        pseudocam=gameObject.transform.GetChild(0);

        settingpause.SetActive(false);
        quitpause.SetActive(false);
        pausecover.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            pausemenu();
        }
        if (ispaused==false) {
        if (health<0) {
        playerdeath();
        }
        Mover();
        StartCoroutine(MouseLook());
        Jump();
        StartCoroutine(gun.shoot());
        }
    }
    public IEnumerator MouseLook() 
    {
        Vector2 targetMouseDelta= new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);
        cameraPitch -= currentMouseDelta.y * mouseSensitivity;
        cameraPitch = Mathf.Clamp(cameraPitch, -90.0f, 90.0f);
        currentcamy=cameraPitch;
        currentcamx+=currentMouseDelta.x * mouseSensitivity;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
        crosshair1.GetComponent<RectTransform>().anchoredPosition =new Vector3(15,15,0) + (new Vector3((200*gun_values.currentspread),(200*gun_values.currentspread),0)) ;
        crosshair2.GetComponent<RectTransform>().anchoredPosition =new Vector3(-15,15,0) + (new Vector3(-(200*gun_values.currentspread),(200*gun_values.currentspread),0)) ;
        crosshair3.GetComponent<RectTransform>().anchoredPosition =new Vector3(15,-15,0) + (new Vector3((200*gun_values.currentspread),-(200*gun_values.currentspread),0)) ;
        crosshair4.GetComponent<RectTransform>().anchoredPosition =new Vector3(-15,-15,0) + (new Vector3(-(200*gun_values.currentspread),-(200*gun_values.currentspread),0)) ;

        if(currentMouseDelta.x>= 0.05f * mouseSensitivity) {
         CylinderCube.localEulerAngles=new Vector3(0,0,-5f);
        };
        if(currentMouseDelta.x<= -0.05f * mouseSensitivity) {
         CylinderCube.localEulerAngles=new Vector3(0,0,5f);
        };
        if(currentMouseDelta.x>= -0.05f * mouseSensitivity && currentMouseDelta.x<= 0.05f * mouseSensitivity ) {
       CylinderCube.localEulerAngles=new Vector3(0,0,0);
        };
                CylinderCamera.transform.localEulerAngles = new Vector3 (currentcamy,currentcamx,0);
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

        Vector3 velocity= (transform.forward * currentDir.y + transform.right * currentDir.x) * (walkspeed + powerup.totalheat/100) + Vector3.up *velocityY;

        controller.Move(velocity * Time.deltaTime);
        CylinderCamera.position=pseudocam.position;
    }
    void Jump() 
    {
    if(Input.GetKeyDown(gun_values.jumpKey) && !isJumping){
            isJumping=true;
            StartCoroutine(Jumplist());
    }

    IEnumerator Jumplist() 
    {
         
            float timeInAir = 0.0f;
            do {
                if (ispaused==false) {
                float jumpForce= jumpFallOff.Evaluate(timeInAir);
                controller.Move(Vector3.up* jumpPower * Time.deltaTime);
                timeInAir+=Time.deltaTime;
                }
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
    void pausemenu() {
        if (ispaused) {
            ispaused=false;
            lockCursor=true;
            cursorlock();
            settingpause.SetActive(false);
            quitpause.SetActive(false);
            pausecover.SetActive(false);
            crosshair1.SetActive(true);
            crosshair2.SetActive(true);
            crosshair3.SetActive(true);
            crosshair4.SetActive(true);
        }
        else {
            ispaused=true;
            lockCursor=false;
            cursorlock();
            settingpause.SetActive(true);
            quitpause.SetActive(true);
            pausecover.SetActive(true);
            crosshair1.SetActive(false);
            crosshair2.SetActive(false);
            crosshair3.SetActive(false);
            crosshair4.SetActive(false);
        }
    }
    void cursorlock() {
        if(lockCursor) {
            Cursor.lockState=CursorLockMode.Locked;
            Cursor.visible=false;
        }
        else {
            Cursor.lockState=CursorLockMode.None;
            Cursor.visible=true;
        }
    }
    public void quit() {
        SceneManager.LoadSceneAsync("Titlescreen");
    }
}