using UnityEngine;
using System.Collections;

/// <summary>
/// Applies an arrival steering force on an NPC
///
/// @author - Jonathan L.A
/// @version - 1.0.0
///
/// </summary>
[System.Serializable]
public class Arrival : NPCActionable
{   
    
    /// <summary>
	/// The Transform that this steering behavior is targetting
	/// </summary>
	private Transform targetTransform;
    
    /// <summary>
    /// When the entity gets this close to his target, he will start slowing down
    /// </summary>
    public float slowingRadius;
    
    public Arrival(int priority, string id, Transform targetTransform) : base(priority, id)
    {
        this.targetTransform = targetTransform;
    }
    
    /// <summary>
    /// Called every frame when the action needs to be performed.
    /// Applies an arrival steering force on the given steerable
    /// </summary>
	public override void Execute(Steerable steerable) 
    {
        base.Execute(steerable);
        
        if (targetTransform)
        {
            steerable.AddArrivalForce(targetTransform.position, slowingRadius, strengthMultiplier);
        }
        else
        {
            // If the target transform is null, there is nothing to seek. Thus, stop the action
            ActionCompleted();
        }
    }
    
    /// <summary>
	/// The Transform that this steering behavior is targetting
	/// </summary>
	public Transform TargetTransform
    {
        get { return targetTransform; }
        set { targetTransform = value; }
    }
    
}
