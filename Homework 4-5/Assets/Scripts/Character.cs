using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.UI;


public class Character : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float gravity = 9.81f;
    [SerializeField] private float jumpPower = 10f;
    [SerializeField] private GameObject Bullet;
    [SerializeField] private GameObject Grenade;
    [SerializeField] private GameObject Ball;
    [SerializeField] private Button Button;
    private Transform cameraTransform;
    private CharacterController Controller;
    private Vector2 walkDirection;
    private Vector3 velocity;
    private GameObject Round;
    private float Power = 10;
    private Vector2 lastInput;
    float camSens = 5f;
    private PlayerInput playerInput;
    
    void Start() 
    {
        Controller = GetComponent<CharacterController>();    
        Round = Bullet;
        
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;
        Button.onClick.AddListener(()=> 
        {
            GameObject b = Instantiate(Round, transform.position + transform.forward * 2 + transform.up * 2, transform.rotation);
            b.GetComponent<Rigidbody>().AddForce(Quaternion.AngleAxis(-30f, transform.right) * transform.forward * Power, ForceMode.Impulse);
            Destroy(b, 10);
        });
    }
    
    void Update()
    {
        Jump(playerInput.actions["Jump"].triggered && Controller.isGrounded);
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();
        Vector3 move = new Vector3(input.x, 0, input.y);  
        move = move.x * cameraTransform.right + move.z * cameraTransform.forward;
        move.y = 0f;

        lastInput = playerInput.actions["Look"].ReadValue<Vector2>()  ;
        lastInput = new Vector3(-lastInput.y * camSens, lastInput.x * camSens, 0 );
        lastInput = new Vector3(transform.eulerAngles.x + lastInput.x , transform.eulerAngles.y + lastInput.y, 0);

        if (lastInput.x > 180 && lastInput.x < 355 )
        {
            lastInput.x = 355;
        }

        if (lastInput.x < 180 && lastInput.x > 50 )
        {
            lastInput.x = 50;
        }
        

        transform.eulerAngles = lastInput;
        
        


        Controller.Move(move * speed * Time.deltaTime);
        
        Gravity(Controller.isGrounded); 

        
        Debug.DrawLine(transform.position, transform.position + Quaternion.AngleAxis(-30f, transform.right) * transform.forward * 500f, Color.red);
    }

   

    void Gravity(bool isGrounded)
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f;
        }
        velocity.y -= gravity * Time.deltaTime;
        Controller.Move(velocity * Time.deltaTime);
    }

    void Jump(bool canJump)
    {
        if (canJump)
        {
            velocity.y = jumpPower;
        }
    }

    void OnTriggerEnter(Collider collider) 
    {
        switch (collider.gameObject.name)
        {
            case "TriggerBullets":  
                Round = Bullet;
                break;
            case "TriggerGrenade": 
                Round = Grenade;
                break;
            case "TriggerBalls": 
                Round = Ball;
                break;        
        }
    }

    
    
    
}
