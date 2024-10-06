using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class NossyAi : MonoBehaviour
{
    // General waypoints for idle wandering
    public GameObject[] waypoints;
    public float minDist = 0.5f; // Minimum distance to waypoint
    public float speed = 2f; // Movement speed

    private int currentWaypointIndex = 0; // Index of the current waypoint

    // Separate waypoints for front and bed
    public GameObject frontWaypoint;
    public GameObject bedWaypoint;

    private State currentState; // Current state of the AI
    private Coroutine idleCoroutine; // Reference to the idle coroutine

    // Unity Events to call upon reaching front and bed waypoints
    public UnityEvent onReachFrontWaypoint;
    public UnityEvent onReachBedWaypoint;

    // Enum for the AI states
    private enum State
    {
        Idle,
        GoToFront,
        GoToBed,
        Death
    }

    private void Start()
    {
        currentState = State.Idle; // Initialize the state to Idle
        StartIdleState(); // Start Idle at the beginning
    }

    private void Update()
    {
        Debug.Log("Current State: " + currentState); // Display current state in console

        switch (currentState)
        {
            case State.Idle:
                // Idle state logic is handled in coroutine
                break;
            case State.GoToFront:
                MoveToWaypoint(frontWaypoint, onReachFrontWaypoint);
                break;
            case State.GoToBed:
                MoveToWaypoint(bedWaypoint, onReachBedWaypoint);
                break;
            case State.Death:
                // No movement in death state
                Debug.Log("State: Death - No movement allowed.");
                break;
        }
    }

    // Generic movement function to move towards a specific waypoint
    private void MoveToWaypoint(GameObject targetWaypoint, UnityEvent onArrivalEvent)
    {
        if (targetWaypoint == null)
        {
            Debug.LogError("Target waypoint is null! Ensure the waypoint is set in the inspector.");
            return;
        }

        float dist = Vector3.Distance(transform.position, targetWaypoint.transform.position);

        if (dist > minDist)
        {
            Move(targetWaypoint.transform.position);
        }
        else
        {
            Debug.Log("Reached waypoint: " + targetWaypoint.name);
            onArrivalEvent.Invoke(); // Trigger event upon arrival
            StopCurrentState(); // Stop movement and wait for a new state
        }
    }

    // Function to handle movement logic
    private void Move(Vector3 targetPosition)
    {
        Debug.Log("Moving towards: " + targetPosition);
        transform.LookAt(targetPosition);
        transform.position += transform.forward * speed * Time.deltaTime;
        Debug.Log("New position: " + transform.position);
    }

    // Start the Idle state, which makes the AI wander between waypoints
    public void StartIdleState()
    {
        StopCurrentState(); // Stop any current state
        currentState = State.Idle; // Set state to Idle
        Debug.Log("State: Idle - Wandering between waypoints.");
        idleCoroutine = StartCoroutine(IdleCoroutine());
    }

    // Coroutine for idle wandering between waypoints
    private IEnumerator IdleCoroutine()
    {
        while (currentState == State.Idle)
        {
            // Move to the next waypoint
            yield return MoveToNextWaypoint();

            // Wait for a random time between 5 and 8 seconds at each waypoint
            float idleWaitTime = Random.Range(5f, 8f);
            Debug.Log("Waiting at waypoint for: " + idleWaitTime + " seconds");
            yield return new WaitForSeconds(idleWaitTime);
        }
    }

    // Function to move to the next waypoint in the array
    private IEnumerator MoveToNextWaypoint()
    {
        GameObject currentTarget = waypoints[currentWaypointIndex];
        float dist = Vector3.Distance(transform.position, currentTarget.transform.position);

        // Move towards the target until we're within the minimum distance
        while (dist > minDist && currentState == State.Idle)
        {
            Move(currentTarget.transform.position);
            dist = Vector3.Distance(transform.position, currentTarget.transform.position);
            yield return null; // Wait for the next frame
        }

        Debug.Log("Reached waypoint: " + currentTarget.name);

        // Advance to the next waypoint in the array
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    // Public method to switch to "Go to Front" state
    public void StartGoToFrontState()
    {
        StopCurrentState(); // Stop any current state
        currentState = State.GoToFront; // Set state to GoToFront
        Debug.Log("State: GoToFront - Heading towards the front waypoint.");
    }

    // Public method to switch to "Go to Bed" state
    public void StartGoToBedState()
    {
        StopCurrentState(); // Stop any current state
        currentState = State.GoToBed; // Set state to GoToBed
        Debug.Log("State: GoToBed - Heading towards the bed waypoint.");
    }

    // Public method to switch to "Death" state
    public void StartDeathState()
    {
        StopCurrentState(); // Stop any current state
        currentState = State.Death; // Set state to Death
        Debug.Log("State: Death - No movement allowed.");
    }

    // Helper function to stop any currently running state (e.g. idle coroutine)
    private void StopCurrentState()
    {
        if (idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine); // Stop the idle coroutine
            idleCoroutine = null;
        }
        Debug.Log("State stopped.");
    }
}
