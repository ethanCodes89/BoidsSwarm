using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class SwarmBehavior : MonoBehaviour
{
    [SerializeField] private float separationDistance = .3f;
    [SerializeField] private float separationForce = 5.0f;
    [SerializeField] private float cohesionForce = 5.0f;
    [SerializeField] private float radiusFromCenter = 10.0f;
    private List<Transform> boidTransforms;
    private Vector3 swarmCenter = Vector3.zero;
    
    private Rigidbody boidRB;
    private void Start()
    {
        boidRB = GetComponent<Rigidbody>();
        boidTransforms = new List<Transform>();
        SwarmBehavior[] swarmBehaviors = FindObjectsOfType<SwarmBehavior>();
        foreach (SwarmBehavior unit in swarmBehaviors)
        {
            //Make sure we don't add this boid to the list of transforms
            if (unit != this)
            {
                boidTransforms.Add(unit.gameObject.GetComponent<Transform>());   
            }
        }
    }
    
    private void FixedUpdate()
    {
        Vector3 separationVector = CalculateSeparation();
        boidRB.AddForce(separationVector * separationForce);

        Vector3 cohesionVector = CalculateCohesion();
        boidRB.AddForce(cohesionVector * cohesionForce);
    }

    private Vector3 CalculateSeparation()
    {
        Vector3 separationVector = Vector3.zero;
        //used to ensure if boids are not close enough we still get some randomized movement out of them
        separationVector += Random.insideUnitSphere * radiusFromCenter; 
        
        foreach (Transform boidTransform in boidTransforms)
        {
            Vector3 vectorToBoid = boidTransform.position - transform.position;
            float distance = vectorToBoid.magnitude;  

            if (distance < separationDistance)
            {
                //Use Mathf.Pow to square the value vs getting a more expensive square root
                float separationForce = Mathf.Pow(separationDistance / distance, 2.0f);
                separationVector -= vectorToBoid.normalized * separationForce;
            }
        }
        
        return separationVector.normalized;
    }

    private Vector3 CalculateCohesion() //Cohesion here is slightly different than normal flocks - based on swarm center vs cohesion to other boids
    {
        Vector3 cohesionVector = Vector3.zero;
        Vector3 toSwarmCenter = swarmCenter - transform.position;
        
        float distanceToCenter = toSwarmCenter.magnitude;
        
        if (distanceToCenter > radiusFromCenter)
        {
            cohesionVector = toSwarmCenter.normalized * cohesionForce;
        }

        return cohesionVector;
    }
}
