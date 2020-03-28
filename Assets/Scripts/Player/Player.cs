using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour, IReceiveDamage
{
    [Header("Player vars")]
    public float currentHp = 100;
    public float maxHp = 100;
    public int papersCollected = 0;

    [Header("Camera Stuff")]
    public float mouseSensitivity = 100f;
    public Transform player;
    public Transform playerCamera;
    float rotationX = 0f;

    [Header("Movement")]
    Vector3 movementInputVector;
    Rigidbody rb;
    public float playerMovementSpeed = 10;

    [Header("Needed components")]
    public Slider sliderHp;
    public TextMeshProUGUI rollsText;
    public GameObject vicScreen;
    public GameObject storeMusic;
    public GameObject deathscreen;
    public GameObject weapon;
    public AudioSource hurt;
    public AudioSource coin;
    public AudioSource walking;
    public TextMeshProUGUI waist;
    public TextMeshProUGUI record;
    public GameObject inventoryUI;
    public GameObject[] inventoryRolls;

    private float timeWaisted = 0;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        // Load timeWaisted recor
        record.text = "Record: " + PlayerPrefs.GetFloat("timeWaisted", 420).ToString() + " Seconds";
    }

    // Update is called once per frame
    void Update()
    {
        PlayerCameraMovement();
        PlayerMovementInput();

        // Update ui
        sliderHp.value = currentHp / 100;
        rollsText.text = "Toilet Rolls Collected: " + papersCollected.ToString() + " / 10";

        // update timer
        timeWaisted += Time.deltaTime;
        waist.text = "Seconds waisted on this game: " + timeWaisted;

        // Toggle inventory
        if(Input.GetButtonDown("Input TAB"))
        {
            inventoryUI.SetActive(!inventoryUI.activeSelf);
        }
    }

    private void FixedUpdate()
    {
        FixedPlayerMovement();
    }

    void PlayerCameraMovement()
    {
        // Inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        rotationX -= mouseY;
        // Clamp the camera rotation
        rotationX = Mathf.Clamp(rotationX, -90, 90f);

        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
    }

    void PlayerMovementInput()
    {
        movementInputVector = new Vector3(Input.GetAxis("Horizontal") * playerMovementSpeed, rb.velocity.y, Input.GetAxis("Vertical") * playerMovementSpeed);
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0 && Input.GetAxis("Vertical") != 0)
        {
            if(!walking.isPlaying)
                walking.Play();
        }
    }

    void FixedPlayerMovement()
    {

        Vector3 fm = (rb.transform.forward * movementInputVector.z);
        Vector3 sm = (rb.transform.right * movementInputVector.x);
        rb.MovePosition(transform.position + (fm + sm) * Time.fixedDeltaTime);
    }

    public void ReceiveDamage(float amount)
    {
        // Reduce hp
        currentHp -= amount;

        // Play hurt sound
        if (currentHp > 0)
        {
            if (!hurt.isPlaying)
                hurt.Play();
        }

        // Check if dead
        if(currentHp <= 0)
        {
            // Player is dead
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            deathscreen.SetActive(true);
            weapon.SetActive(false);
            enabled = false;
        }
    }

    public void CollectedPaper()
    {
        papersCollected++;
        coin.Play();
        inventoryRolls[papersCollected - 1].SetActive(true);
        if(papersCollected >= 10)
        {
            // Finished game
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            vicScreen.SetActive(true);
            storeMusic.SetActive(false);
            weapon.SetActive(false);
            enabled = false;
            // Update record
            if(timeWaisted < PlayerPrefs.GetFloat("timeWaisted", 420))
            {
                // New record
                PlayerPrefs.SetFloat("timeWaisted", timeWaisted);
                record.text = "NEW RECORD! Finished in " + timeWaisted + "Seconds!";
            }
        }
    }

    public void ReceiveHp()
    {
        currentHp = maxHp;
        coin.Play();
    }
}


