using UnityEngine;

/// <summary>
/// PlayerLightToggle class is responsible for behaviour related to player lights.
/// It toggles the lights on or off and depletes the energy from the Player
///
/// @author - Simon T.
/// @author - Alex I.
/// @version - 1.0.0
///
/// </summary>
public class PlayerLightToggle
{
    
	private bool lightsEnabled; // If true, the lights that are children of this object are enabled.    
    private bool lightButtonPressed; // If true, the user has pressed the button to activate his lights
    private bool propulsionLightsOn; // If true, the player is holding down the propulsion button
    private float timeToDeplete;
    private float minimalEnergyRestriction;
    private float propulsionLightRange; // The percent range of light when propulsion is on
    private GameObject lightsToToggle; 
    private Player player;
    private Light closeLight;
    
    public PlayerLightToggle(GameObject lightsToToggle, Player player, LightToggleBean lightToggleBean)
    {        
        this.lightsToToggle = lightsToToggle;
        this.player = player;        
        this.lightsEnabled = lightToggleBean.DefaultLightStatus;
        this.lightButtonPressed = lightToggleBean.DefaultLightStatus;                        
        this.minimalEnergyRestriction = lightToggleBean.MinimalEnergy;
        this.propulsionLightRange = lightToggleBean.PropulsionLightRange;
        this.ToggleLights();
        
        // Cache very close light - needs to be refactored
        Transform lightTransform = lightsToToggle.transform.Find("VeryClose Light");
        if (lightTransform != null)
        {
            this.closeLight = lightTransform.GetComponent<Light>();
            this.closeLight.intensity = 0;
        }                
    }

    /// <summary>
    /// Changes the status of Light component attached to Player. Called when the light button is pressed.
    /// </summary>
    public void ToggleLights()
    {
        this.lightButtonPressed = !this.lightButtonPressed;        
        ToggleLights(this.lightButtonPressed);
    }
    
    /// <summary>
    /// Changes the status of Light component attached to Player 
    /// </summary>
    private void ToggleLights(bool enabled)
    {
        // Toggle the lights to maximum range
        ToggleLights(enabled, 1.0f);
    }
    
    /// <summary>
    /// Changes the status of Light component attached to Player 
    /// <param name="enabled">If true, the lights are toggled on.</param>
    /// <param name="percent">The strength percentage of the lights. 0 = off 1 = original strength</param>
    /// </summary>
    private void ToggleLights(bool enabled, float percent)
    {
        foreach (Light light in this.lightsToToggle.GetComponentsInChildren<Light>())
        {
            // Skip VeryClose Light                                     
            if (light == null || light.name == "VeryClose Light") { continue; }
            
            // Set the range of the light to the given percent
            LightRangeModifier rangeModifier = light.GetComponent<LightRangeModifier>();
            if (rangeModifier)
            {
                rangeModifier.ActiveLights = enabled;
                rangeModifier.PercentRange = percent;
            }
        }
        // Update lights' status
        this.lightsEnabled = enabled;
    }

    /// <summary>
    /// Gradually depletes Player's energy when lights are turned on
    /// </summary>
    /// <param name="timeToDeplete">Time it takes to deplete a unit of energy when light toggle is on</param>
    /// <param name="energyCost">Energy cost of having Player lights turned on</param>
    public void DepleteLight(float timeToDeplete, float energyCost)
    {
        if (this.lightButtonPressed)
        {
            this.timeToDeplete += Time.deltaTime;
            if (this.timeToDeplete > timeToDeplete)
            {
                this.player.LightEnergy.Deplete(energyCost);
                this.timeToDeplete = 0;

                if (this.minimalEnergyRestriction >= this.player.LightEnergy.CurrentEnergy)
                {
                    this.ToggleLights();
                }
            }
        }
    }
    
    /// <summary>
    /// Turns on the lights after the player starts propulsing
    /// </summary>
    public void OnPropulsionStart()
    {
        if (!lightButtonPressed)
        {
            // Turn on the lights at limited range when the player starts propulsing
            ToggleLights(true,propulsionLightRange);
        }
    }
    
    /// <summary>
    /// Turns off the lights after the player stops propulsing
    /// </summary>
    public void OnPropulsionEnd()
    {
        if (!lightButtonPressed)
        {
            // Turn off the lights since the light button is off and the propulsion ended
            ToggleLights(false);
        }
        else
        {
            // Turn on the lights at full range if the light button is on
            ToggleLights(true, 1.0f);
        }
    }

    public bool LightsEnabled
    {
        // If true, the lights are enabled and the GameObject is visible
        get { return this.lightsEnabled; }
    }
    
    public bool LightButtonPressed
    {
        //If true, the light button was pressed and the player's light should be on
        get { return this.lightButtonPressed; }
    }

    public Light ProbeLight
    {
        get { return this.closeLight; }
        set { this.closeLight = value; }
    }
}
