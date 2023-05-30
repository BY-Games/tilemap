using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class ChaseThePlayerAgent : Agent {
    // Offset vector is used when training multiple grids, help for determine the correct positions for other grid agents
    [SerializeField] private Transform offSetVecPosition;

    // tiles to check for a valid position of the agent and player.
    [SerializeField] Tilemap tilemap = null;
    [SerializeField] AllowedTiles allowedTiles = null;

    // display colors for debugging.
    [SerializeField] private Material winMaterial;
    [SerializeField] private Material loseMaterial;

    [SerializeField] private SpriteRenderer trainingState;

    // [SerializeField] private RandomPosition randomPosition;
    private TileBase TileOnPosition(Vector3 worldPosition) {
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition + offSetVecPosition.position);
        // Debug.Log(cellPosition);
        return tilemap.GetTile(cellPosition);
    }


    // The transform component of the object that the agent should chase.
    [FormerlySerializedAs("target")] [SerializeField]
    private Transform targetTransform;

    private bool checkPos(Vector3 position) {
        TileBase tileOnNewPosition;
        tileOnNewPosition = TileOnPosition(position);
        return allowedTiles.CanBeSpawnHere(tileOnNewPosition);
    }

    // Generate random position based on tilemap locations (range is fixed in this map, therefore fixed numbers)
    public Vector3 getRandomPosition() {
        int counter = 0;
        Vector3 rndPos;

        do {
            // Debug.Log("current iteration - " + counter);
            counter++;

            const double offsetXAxis = 8.5;
            var locX = UnityEngine.Random.Range(0, 18) - offsetXAxis;
            const double offsetYAxis = 4.5;
            var locY = UnityEngine.Random.Range(0, 10) - offsetYAxis;
            rndPos = new Vector3((float)locX, (float)locY, 6);
            // Debug.Log("Vector chosen : " + rndPos);

            // check if the generated position located on allowed tile.
            if (checkPos(rndPos)) {
                // Debug.Log("Break on counter " + counter);

                break;
            }
        } while (counter < 50); // limit the number of checks to avoid delay of each episode.

        return rndPos;
    }

    public override void OnEpisodeBegin() {
        // At each episode generate random valid position on map, for agent and target.

        transform.localPosition = new Vector3(-3.5f, -2.5f, 6f);
        targetTransform.localPosition = new Vector3(-4.5f, 4.5f, 6f);
        transform.localPosition = getRandomPosition();
        targetTransform.localPosition = getRandomPosition();

        ;
    }

    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(targetTransform.localPosition);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        ActionSegment<float> continousActions = actionsOut.ContinuousActions;
        continousActions[0] = Input.GetAxisRaw("Horizontal");
        continousActions[1] = Input.GetAxisRaw("Vertical");
    }

    [SerializeField] private float stepDelay = 0.5f; // Adjust the delay time as needed

    private float timeSinceLastStep = 0f;
    private bool isInferenceOnly = false;

    private void Start() {
        // Retrieve the BehaviorParameters component attached to the agent
        var behaviorParameters = GetComponent<BehaviorParameters>();
        if (behaviorParameters != null) {
            // Check the value of the "Inference Only" option
            isInferenceOnly = behaviorParameters.BehaviorType == BehaviorType.InferenceOnly;
        }
    }

    public override void OnActionReceived(ActionBuffers actions) {
        if (!isInferenceOnly) {
            PerformMovement(actions);
        }
        else // Perform movement with short delay in "Inference Only" mode for visual and playing comfort.
        {
            timeSinceLastStep += Time.deltaTime;

            if (timeSinceLastStep >= stepDelay) {
                // Perform movement after the delay time has passed
                PerformMovement(actions);

                // Reset the time counter
                timeSinceLastStep = 0f;
            }
        }
    }

    private void PerformMovement(ActionBuffers actions) {
        // Because the tile is set to be 0.5 length tiles, the movement will be by tiles.
        // positive/negative action received will be +-y/x movement, 0 will stay at place. 
        var horizontal = 0f;
        var vertical = 0f;
        float moveX = actions.ContinuousActions[0];
        float moveY = actions.ContinuousActions[1];
        if (moveX != 0f) {
            horizontal = moveX > 0f ? 0.5f : -0.5f;
        }

        transform.localPosition += new Vector3(horizontal, 0, 0);

        if (moveY != 0f) {
            vertical = moveY > 0f ? 0.5f : -0.5f;
        }

        transform.localPosition += new Vector3(0, vertical, 0);

        // set negative reward when touching invalid tile.
        if (!checkPos(transform.localPosition)) {
            trainingState.material = loseMaterial;
            SetReward(-1f);
            EndEpisode();
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // set positive reward when the agent catches the player on the map.
        if (other.CompareTag("Player")) {
            SetReward(1f);
            trainingState.material = winMaterial;
            Debug.Log("Reward!");
            EndEpisode();
        }

        if (other.CompareTag("Bounds")) {
            SetReward(-1f);
            trainingState.material = loseMaterial;
            Debug.Log("Penalty");
            EndEpisode();
        }
    }
}