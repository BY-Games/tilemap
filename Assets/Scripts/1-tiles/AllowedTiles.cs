using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

/**
 * This component just keeps a list of allowed tiles.
 * Such a list is used both for pathfinding and for movement.
 */
public class AllowedTiles : MonoBehaviour
{
    [SerializeField] TileBase[] allowedTiles = null;
    public bool mountain { set; private get; }
    public bool sea { set; private get; }

    private void Start()
    {
        mountain = false;
        sea = false;
    }

    // Assign all the tiles that each util can handle, array in case there is multiple tiles for the element.

    // goatTile will have all the tiles that represent mountains.
    [FormerlySerializedAs("goatTile")] [SerializeField]
    private TileBase[] mountainTiles;

    // boatTile will have all the tiles that represent mountains
    [FormerlySerializedAs("boatTile")] [SerializeField]
    private TileBase[] seaTiles;

    public bool CanGoOnMountain(TileBase tile)
    {
        // if goat ability is active, check if relevant.
        return (mountain && mountainTiles.Contains(tile));
    }

    public bool Contain(TileBase tile)
    {
        // if boat ability is active, check if relevant.
        if (sea && seaTiles.Contains(tile))
        {
            return true;
        }

        // default check of other allowed tiles.
        return allowedTiles.Contains(tile);
    }

    public TileBase[] Get()
    {
        return allowedTiles;
    }
}