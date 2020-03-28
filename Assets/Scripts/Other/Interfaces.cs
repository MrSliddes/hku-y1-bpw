using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to store all interfaces
/// </summary>

/// <summary>
/// Used to receive damage from outside sources
/// </summary>   
public interface IReceiveDamage
{
    /// <summary>
    /// Receive damage from outside sources
    /// </summary>
    /// <param name="amount">The amount of damage it receives</param>
    void ReceiveDamage(float amount);
}
