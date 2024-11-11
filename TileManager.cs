using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    [SerializeField]private Tilemap tilemap;
    [SerializeField]private TileBase box;
    [SerializeField]private TileBase cymbal_purple;
    [SerializeField]private TileBase cymbal_gold;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 contactPoint = collision.contacts[0].point;
            
            Vector3Int tilePosition = tilemap.WorldToCell(new Vector3(contactPoint.x, contactPoint.y, 0));
            
            TileBase tile = tilemap.GetTile(tilePosition);
            
            if (tile == null)
            {
                Debug.LogError("Tile doesn't exist here");
                return; 
            }

            if (tile == box)
            {
                tilemap.SetTile(tilePosition, null);
                Debug.Log("Tile destroyed at position: " + tilePosition);
            }
        }
    }


    // private void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         Vector3Int tilePosition = tilemap.WorldToCell(collision.transform.position);
    //         TileBase tile = tilemap.GetTile(tilePosition);
    //         if(tile == null)
    //             Debug.LogError("Tile doesnt exist here");

    //         if (tile == box)
    //         {
    //             tilemap.SetTile(tilePosition, null);
    //             Debug.Log("Tile destroyed at position: " + tilePosition);
    //         }
    //     }
    // }
}
