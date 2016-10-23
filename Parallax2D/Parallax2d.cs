using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BrianTools.Parallax2D
{
    /// <summary>
    /// Simulates a parallax effect with a 2D camera using the element's z-axis position
    /// as a parameter for displacement.
    /// </summary>
    public class Parallax2d : MonoBehaviour
    {
        [SerializeField]
        private Camera _referenceCamera;

        [SerializeField]
        private List<GameObject> _parallaxElements;

    }
}
