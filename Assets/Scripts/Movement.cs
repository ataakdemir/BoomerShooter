using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float moveSpeed;
    public Transform orientation;

    [Header("Ground")]
    public float playerHeight;
    public LayerMask groundMask;
    bool grounded;

    public float groundDrag;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public Rigidbody rb;

    public float jumpForce;
    public float jumpCoolDown;
    bool readyToJump = true;
    public float airMultiplier;

    public float fallMultiplier;

    public float slowWalkSpeed;

    [Header("Health")]
    public float maxHealth = 100f;
    private float currentHealth;

    public TMP_Text healthText;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        currentHealth = maxHealth;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.75f + 0.2f, groundMask);

        MyInput();
        SpeedControl();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;

        if (Input.GetKey(KeyCode.LeftShift) && grounded)
        {
            moveSpeed = slowWalkSpeed; 
        }
        else
        {
            moveSpeed = 17f; 
        }

    }
    private void FixedUpdate()
    {
        MovePlayer();

        if (!grounded)
        {
            rb.AddForce(Vector3.down * fallMultiplier, ForceMode.Acceleration); 
        }

    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCoolDown);
        }
    }
    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(grounded)
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // rb.velocity = new Vector3(moveDirection.normalized.x * moveSpeed, rb.velocity.y, moveDirection.normalized.z * moveSpeed); // ivmesiz hareket

        else if (!grounded)
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
      
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)         //limit velocity if needed
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }

    public void PlayerTakesDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthText.text = "Health: " + currentHealth.ToString("0");
        Debug.Log("Player Health:" + currentHealth);
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void HealPlayer(float healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        healthText.text = "Health: " + currentHealth.ToString("0");
    }

    void Die()
    {
        Debug.Log("Player died!");
        DOTween.KillAll(); // Tüm animasyonlarý durdur
        SceneManager.LoadScene("GameOver");
    }
}
