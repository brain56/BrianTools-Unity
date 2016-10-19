using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Simulates a parallax effect with a 2D camera using the element's z-axis position
/// as a parameter for displacement.
/// </summary>
public class BlParallax2d : MonoBehaviour {

    [SerializeField]
    private Camera _referenceCamera;

    [SerializeField]
    private List<GameObject> _parallaxElements;

}
