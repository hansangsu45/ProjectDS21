using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    private bool isMoving = false;
    [SerializeField] private float speed = 10f;

    private void LateUpdate()
    {
        if(isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y, -10), Time.deltaTime * speed);
        }
    }

    public void SetMoveState(bool move) => isMoving = move;
}
