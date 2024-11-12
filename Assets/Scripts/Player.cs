using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed,
                  turnSpeed;

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        transform.Rotate(horizontal * turnSpeed * Vector3.up * Time.deltaTime);

        float vertical = Input.GetAxis("Vertical");
        transform.Translate(vertical * moveSpeed * Vector3.forward * Time.deltaTime);

    }
}
