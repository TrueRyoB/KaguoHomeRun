using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UpdateRespawn : MonoBehaviour
{
    //assuming each element of cameras corresponds to the respawn loc for the same index
    [SerializeField] private GameObject[] respawns;
    [SerializeField] private GameObject[] cameras;

    private int _index = 0;
    private int Index
    {
        get => _index;
        set {
            cameras[_index].GetComponent<ICinemachineCamera>().Priority = 0;
            _index = value;
            cameras[_index].GetComponent<ICinemachineCamera>().Priority = 10;
        }
    }

    void Start() {
        if(cameras.Length != respawns.Length)
            Debug.LogError("please add either camera or respawn locs so that the number is equal!");
        Respawn();
    }

    void Respawn()
    {
        transform.position = respawns[Index].transform.position;
        cameras[Index].GetComponent<ICinemachineCamera>().Priority = 10;//needs to make sure that the old camera is set its priority at 0
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var objName = other.gameObject.name;

        if(other.tag == "res")
        {
            int found = System.Array.IndexOf(respawns, other.gameObject);

            if (found == -1) {
                Debug.LogError("Either the respawner is not assigned to the vector or the tag is put to the wrong unit");
            }
            else if(Index < found){
                Index = found;
            }
        }
        else if(objName == "respawn")
        {
            Respawn();
        }        
    }

    public void Goaled()
    {
        Index = 0;
        Respawn();
    }


}
