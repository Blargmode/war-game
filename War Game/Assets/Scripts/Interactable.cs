using UnityEngine;

public class Interactable : MonoBehaviour {

    public float radius = 3f;

    bool isFocus = false;
    Transform player;

    public void Update()
    {
        if (isFocus)
        {
            float distance = Vector3.Distance(player.position, transform.position);
            if(distance <= radius)
            {

            }
        }
    }

    public void OnFocus(Transform playerTransform)
    {
        isFocus = true;
        player = playerTransform;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
