using UnityEngine;
using System.Collections;

/// <summary>
/// ????
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class Flocking : NPCActionable
{
    /** Steering behaviours which allow NPCs to flock together */
    [SerializeField]
    private Wander wander;
    [SerializeField]
    private WallAvoidance wallAvoidance;
    [SerializeField]
    private Alignment alignment;
    [SerializeField]
    private Cohesion cohesion;
    [SerializeField]
    private Separation separation;
    
    public Flocking(int priority, string id) : base(priority, id)
    {
        this.SetPriority(priority);
        this.SetID(id);
    }
    
    /// <summary>
    /// Sets the priorities of each steering behaviour.
    /// This priority is used to index the behaviours in a priority dictionary
    /// </summary>
    public void SetPriority(int priority)
    {
        this.priority = priority;        
        wander.Priority = priority;
        wallAvoidance.Priority = priority;
        alignment.Priority = priority;
        cohesion.Priority = priority;
        separation.Priority = priority;
    }
    
    /// <summary>
    /// Updates the ID for each internal steering behaviour.
    /// Allows the action to be referenced by a unique index
    /// </summary>
    public void SetID(string id)
    {
        this.id = id;
        
        wander.Id = id;
        wallAvoidance.Id = id;
        alignment.Id = id;
        cohesion.Id = id;
        separation.Id = id;
    }
    
    /// <summary>
    /// Sets the properties for the Wander steering behaviour
    /// <param name="strengthMultiplier">The amount by which the seek force is multiplied before being added to the steerable's steering force
    /// <param name="circleDistance">The distance from the entity to the "wander" circle. The greater this value,
    /// the stronger the wander force, and the more likely the entity will change directions.</param>
    /// <param name="circleRadius">The greater this radius, the stronger the wander force, and the more likely
    /// the entity will change directions.</param>
    /// <param name="angleChange">The maximum angle in degrees that the wander force can change next frame.</param>
    /// </summary>
    public void SetWanderProperties(float strengthMultiplier, float circleRadius, float circleDistance, float angleChange)
    {
        wander.strengthMultiplier = strengthMultiplier;
        wander.CircleRadius = circleRadius;
        wander.CircleDistance = circleDistance;
        wander.AngleChange = angleChange;
    }
    
    /// <summary>
    /// Avoids the nearest obstacle in front of the object. This works for obstacles of any size or shape, unlike
    /// Obstacle Avoidance, which approximates obstacles as spheres. Formal name: "Containment" or "Generalized Obstacle Avoidance"
    /// </summary>
    /// <param name="avoidanceForce">The amount of force applied in order to avoid the nearest obstacle.</param>
    /// <param name="maxViewDistance">Only obstacles within 'maxViewDistance' meters of this steerable can be avoided.</param>
    /// <param name="obstacleLayer">The layer which contains the colliders that can be avoided.</param>
    public void SetWallAvoidanceProperties(float strengthMultiplier, float avoidanceForce, float maxViewDistance, LayerMask obstacleLayer)
    {
        wallAvoidance.strengthMultiplier = strengthMultiplier;
        wallAvoidance.AvoidanceForce = avoidanceForce;
        wallAvoidance.MaxViewDistance = maxViewDistance;
        wallAvoidance.ObstacleLayer = obstacleLayer;
    }
    
    /// <summary>
    /// Sets the strength at which the alignment behaviour is executed
    /// </summary>
    public void SetAlignmentProperties(float strengthMultiplier)
    {
        alignment.strengthMultiplier = strengthMultiplier;
    }
    
    /// <summary>
    /// Sets the strength at which the separation behaviour is executed
    /// </summary>
    public void SetCohesionProperties(float strengthMultiplier)
    {
        cohesion.strengthMultiplier = strengthMultiplier;
    }
    
    /// <summary>
    /// Sets the strength at which the separation behaviour is executed
    /// </summary>
    public void SetSeparationProperties(float strengthMultiplier)
    {
        separation.strengthMultiplier = strengthMultiplier;
    }
    
    /// <summary>
    /// Called every frame when this action needs to be performed.
    /// Applies a flocking steering force on the given steerable
    /// </summary>
    public override void Execute(Steerable steerable)
    {
        // Override the steerable's min/max speed
        if (overrideSteerableSpeed)
        {
            steerable.MinSpeed = minSpeed;
            steerable.MaxSpeed = maxSpeed;
        }
        // Override the steerable's max force
        if (overrideMaxForce)
        {
            steerable.MaxForce = maxForce;
        }
        
        cohesion.Execute(steerable);
        alignment.Execute(steerable);
        separation.Execute(steerable);
        wallAvoidance.Execute(steerable);
        wander.Execute(steerable);
    }
} 