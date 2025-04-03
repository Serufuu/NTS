using UnityEngine;
using UnityEngine.InputSystem;

public class BallThrower : MonoBehaviour
{
    public GameObject ballPrefab;
    public float throwForceMultiplier = 10f;

    private GameObject currentBall;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isTouching = false;

    void Update()
    {
        HandleTouch();
    }

    void HandleTouch()
    {
        // Utilisation de InputSystem pour détecter les clics ou les touches tactiles
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            startTouchPosition = Mouse.current.position.ReadValue();
            isTouching = true;
        }

        // Détection de la fin du clic ou du toucher
        if (Mouse.current.leftButton.wasReleasedThisFrame && isTouching)
        {
            endTouchPosition = Mouse.current.position.ReadValue();
            ThrowBall();
            isTouching = false;
        }
    }



    void ThrowBall()
    {
        Vector2 swipeDirection = endTouchPosition - startTouchPosition;
        Vector3 forceDirection = new Vector3(swipeDirection.x, swipeDirection.y, swipeDirection.y).normalized;

        Rigidbody ballRigidbody = currentBall.GetComponent<Rigidbody>();
        ballRigidbody.isKinematic = false;
        ballRigidbody.AddForce(forceDirection * throwForceMultiplier, ForceMode.Impulse);
    }
}
