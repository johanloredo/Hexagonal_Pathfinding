using UnityEngine;

public class MoveTransformVelocity : MonoBehaviour, IMoveVelocity
{
    [SerializeField] private float moveSpeed = 10f;

    private Vector3 velocityVector;

    public void SetVelocity(Vector3 velocityVector)
    {
        this.velocityVector = velocityVector;
    }

    private void Update()
    {
        transform.position += velocityVector * moveSpeed * Time.deltaTime;
    }

    public void Disable()
    {
        this.enabled = false;
    }

    public void Enable()
    {
        this.enabled = true;
    }
}
