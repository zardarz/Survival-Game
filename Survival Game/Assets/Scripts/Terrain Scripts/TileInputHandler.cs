using UnityEngine;
using UnityEngine.Tilemaps;

public class TileInputHandler : MonoBehaviour
{

    [SerializeField] private Tilemap collideableTilemap;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePos = collideableTilemap.WorldToCell(mousePos);

            TileData tilePressed = collideableTilemap.GetTile(tilePos) as TileData;

            if(tilePressed == null) {
                return;
            }

            Placeable placeablePressed = PlaceableItems.placeables[tilePressed.tileName];

            if(placeablePressed is not Interactable) {
                return;
            }

            Interactable interactablePressed = placeablePressed as Interactable;

            interactablePressed.onRightClick.Invoke();
            print("invoked thingy");
        }
    }
}