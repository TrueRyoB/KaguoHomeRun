using UnityEngine;

[ExecuteAlways]
public class lift_UI : MonoBehaviour
{
    [SerializeField]private Transform End; 

    void UpdateLiftPos()
    {
        //ensures that it only runs during the Scene mode
        if (!Application.isPlaying && End != null) 
            transform.position = End.position;
    }

    void OnDrawGizmos() => UpdateLiftPos();
    void OnValidate() => UpdateLiftPos();
}
