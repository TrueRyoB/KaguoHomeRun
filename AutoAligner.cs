using UnityEngine;

[ExecuteAlways]
public class AutoAligner : MonoBehaviour
{
    private void Update()
    {
        if (Application.isPlaying)
            return;

        transform.position = Round2Four(transform.position);
    }

    private Vector3 Round2Four(Vector3 position)
    {
        int x = (int)(Mathf.Round(position.x / 4f) * 4);
        int y = (int)(Mathf.Round(position.y / 4f) * 4);

        return new Vector3(x, y, position.z);
    }
}
