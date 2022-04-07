
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private Transform nextRoom;
    [SerializeField] private Transform previousRoom;
    [SerializeField] private CameraControl cam;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (collision.transform.position.x < transform.position.x)
                cam.MoveToNewRoom(nextRoom);
            //else
              //  cam.MoveToNewRoom(previousRoom);
        }
    }
}
