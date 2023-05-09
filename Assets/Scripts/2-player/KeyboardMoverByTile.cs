using System;
using UnityEngine;
using UnityEngine.Tilemaps;

/**
 * This component allows the player to move by clicking the arrow keys,
 * but only if the new position is on an allowed tile.
 */
public class KeyboardMoverByTile: KeyboardMover {
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;

    private bool pickAxe;
    [SerializeField] private TileBase grassTile;

    private TileBase TileOnPosition(Vector3 worldPosition) {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);
        return tilemap.GetTile(cellPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // When triggered with one of the tools in the map, adjust it in the Allowed tilemap.
        // Goat and Pick handles the mountain interaction with the player.
        // Boat handle sea interaction with the player.
        // all object that is triggered will be set to false as they no longer needed in the map.
        switch (other.tag)
        {
            case "Goat":
                allowedTiles.mountain = true;
                other.gameObject.SetActive(false);
                break;
            case "Boat" :
                allowedTiles.sea = true;
                other.gameObject.SetActive(false);
                break;
            case "Pick":
                // pickAxe flag in this class, handle the conversion of the current tile position.
                pickAxe = true;
                allowedTiles.mountain = true;
                other.gameObject.SetActive(false);
                break;
        }
    }

    private bool MountainCheck(Vector3 newPosition, TileBase tileOnNewPosition)
    {
        // function on AllowedTiles.
        if (!allowedTiles.CanGoOnMountain(tileOnNewPosition)) return false;
        // check if pickAxe has taken' and make sure grassTile is assigned.
        if (pickAxe && grassTile)
        {
            tilemap.SetTile(tilemap.WorldToCell(newPosition), grassTile);
        }
        return true;

    }

    void Update()  {
        Vector3 newPosition = NewPosition();
        TileBase tileOnNewPosition = TileOnPosition(newPosition);
        // if this is one of the regular/Sea allowed tiles -> checks if boat is activated and other regular ones.
        /* mountain tiles gets two checks,
        1. if its allowed, handles in trigger.
        2. check if the player posses pick axe, if so, turn mountain into grass (provided in serializedField)
        */
        if (allowedTiles.Contain(tileOnNewPosition) || MountainCheck(newPosition, tileOnNewPosition)) {
            transform.position = newPosition;
        } else {
            Debug.Log("You cannot walk on " + tileOnNewPosition + "!");
        }
    }
}
