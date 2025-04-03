using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.InputSystem;

public class ARBallController : MonoBehaviour
{
    private Camera arCamera;
    public GameObject ballPrefab;
    private GameObject currentBall;
    public float followSpeed = 5.0f;
    public float throwForceMultiplier = 0.05f;

    private Vector2 startInputPos;
    private float startTime;
    private bool isDragging = false;
    private bool isUsingMouse = false; // Permet de détecter si on utilise la souris

    private Mouse mouse;

    void Start()
    {
        arCamera = Camera.main;
        SpawnBall();
        mouse = Mouse.current; // Référence au système de la souris
    }

    void Update()
    {
        if (currentBall != null)
        {
            Vector3 targetPosition = arCamera.transform.position + arCamera.transform.forward * 0.5f;
            targetPosition.y = currentBall.transform.position.y; // Garder la position Y actuelle

            currentBall.transform.position = Vector3.Lerp(currentBall.transform.position, targetPosition, followSpeed * Time.deltaTime);
        }

        HandleInput();
    }

    void HandleInput()
    {
        // 🖱 Gestion de la souris avec le nouveau Input System
        if (mouse != null && mouse.leftButton.isPressed) // Vérifier si le bouton gauche de la souris est pressé
        {
            if (!isDragging) // Commencer à glisser si ce n'est pas déjà fait
            {
                startInputPos = mouse.position.ReadValue();
                startTime = Time.time;
                isDragging = true;
                isUsingMouse = true; // Mode souris activé
            }
        }
        else if (isDragging)
        {
            Vector2 endInputPos = mouse.position.ReadValue();
            Vector2 swipeDirection = endInputPos - startInputPos;

            float swipeTime = Time.time - startTime;
            float swipeSpeed = swipeDirection.magnitude / swipeTime;

            if (swipeDirection.magnitude > 50f) // Seuil pour éviter les faux lancers
            {
                ThrowBall(swipeDirection, swipeSpeed);
            }

            isDragging = false;
        }

        // 📱 Gestion du tactile avec le nouveau Input System
        if (Touchscreen.current != null && Touchscreen.current.primaryTouch != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.press.isPressed) // Détecter si le touché est appuyé
            {
                if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
                {
                    startInputPos = touch.position.ReadValue();
                    startTime = Time.time;
                    isDragging = true;
                    isUsingMouse = false; // Mode tactile activé
                }
                else if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended && isDragging)
                {
                    Vector2 endInputPos = touch.position.ReadValue();
                    Vector2 swipeDirection = endInputPos - startInputPos;

                    float swipeTime = Time.time - startTime;
                    float swipeSpeed = swipeDirection.magnitude / swipeTime;

                    if (swipeDirection.magnitude > 50f)
                    {
                        ThrowBall(swipeDirection, swipeSpeed);
                    }

                    isDragging = false;
                }
            }
        }
    }

    void ThrowBall(Vector2 swipeDirection, float swipeSpeed)
    {
        if (currentBall != null)
        {
            Rigidbody rb = currentBall.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 throwDirection = new Vector3(swipeDirection.x, 0, swipeDirection.y).normalized;
                rb.isKinematic = false;

                float throwForce = swipeSpeed * throwForceMultiplier;
                rb.AddForce(arCamera.transform.TransformDirection(throwDirection) * throwForce, ForceMode.Impulse);

                currentBall = null;

                // Réapparaître une nouvelle balle après 1 seconde
                Invoke(nameof(SpawnBall), 1.0f);
            }
        }
    }

    void SpawnBall()
    {
        currentBall = Instantiate(ballPrefab);
        currentBall.transform.position = arCamera.transform.position + arCamera.transform.forward * 0.5f + arCamera.transform.up * -0.1f;

        Rigidbody rb = currentBall.GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Détection de collision
    void OnCollisionEnter(Collision collision)
    {
        // Actions à effectuer lorsque la collision se produit
        Debug.Log("Collision détectée avec : " + collision.gameObject.name);

        // Exemple de réaction à la collision : changer la couleur de l'objet
        if (collision.gameObject.CompareTag("Target"))
        {
            Debug.Log("Objet touché a le tag 'Target'");
            // Vous pouvez faire d'autres actions, comme détruire l'objet ou changer sa couleur
            // collision.gameObject.GetComponent<Renderer>().material.color = Color.red;
        }
    }
}
