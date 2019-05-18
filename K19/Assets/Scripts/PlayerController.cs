using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject camera;
    public Text gun;
    public Text lives;

    public float speed;
    public float mouseSens;
    public float mouseSmooth;

    private Vector2 smoother;
    private Vector2 mouseLook;
    private float shotCD;
    private int health = 3;

    RaycastHit shot;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        lives.text = "Health: "+ health;
    }

    // Update is called once per frame
    void Update()
    {
        GunUI();
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), 0 , Input.GetAxis("Vertical"));
        movement = transform.TransformDirection(movement);
        // Vector3.MoveTowards(transform.position, transform.position + movement, speed*Time.deltaTime);
        transform.position += movement * speed * Time.deltaTime;

        MouseMovement();

        if (Input.GetKey(KeyCode.Mouse0) && shotCD <=0)
        {
            Physics.Raycast(transform.position, camera.transform.forward*100, out shot);
            shotCD = 1;
            Debug.DrawRay(transform.position, camera.transform.forward * 10, Color.red, 1);
            print(shot.collider.name);
            if (shot.collider.name == "Enemy")
            {
                Enemy enemy = shot.collider.GetComponent<Enemy>();
                enemy.TookHit();
            }
        }

    }

    void MouseMovement()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        mouseDelta = Vector2.Scale(mouseDelta, new Vector2(mouseSens * mouseSmooth, mouseSens * mouseSmooth));
        smoother.x = Mathf.Lerp(smoother.x, mouseDelta.x, 1f / mouseSmooth);
        smoother.y = Mathf.Lerp(smoother.y, mouseDelta.y, 1f / mouseSmooth);

        mouseLook += smoother;

        camera.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
        transform.localRotation = Quaternion.AngleAxis(mouseLook.x, transform.up);
    }

    void GunUI()
    {
        if (shotCD > 0)
        {
            shotCD -= Time.deltaTime;
            int percent = 100 - (int)Mathf.RoundToInt(shotCD*100);
            gun.text = "Charging... " + percent + "%";
        }
        else
        {
            gun.text = "";
        }
    }

    public void TakeHit()
    {
        health -= 1;
        lives.text = "Health: " + health;
        if(health <= 0)
        {
            print("dead");
            this.enabled = false;
        }
    }
}
