using UnityEngine;
using System.Collections;

public class RandomAnimationTrigger : MonoBehaviour
{
    // Reference to the Animator component
    private Animator animator;

    // Time range for random intervals
    public float minWaitTime = 10f;
    public float maxWaitTime = 15f;

    void Start()
    {
        // Get the Animator component attached to the GameObject
        animator = GetComponent<Animator>();

        // Start the coroutine to trigger animations randomly
        StartCoroutine(TriggerRandomAnimation());
    }

    IEnumerator TriggerRandomAnimation()
    {
        while (true)
        {
            // Wait for a random time between the min and max time
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            // Randomly choose between triggering "blep" or "blink"
            int randomChoice = Random.Range(0, 2); // 0 or 1
            if (randomChoice == 0)
            {
                animator.SetTrigger("Blink");
            }
            else
            {
                animator.SetTrigger("Blep");
            }
        }
    }
}