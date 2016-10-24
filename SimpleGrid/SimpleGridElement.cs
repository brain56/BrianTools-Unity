using UnityEngine;
using System.Collections;

namespace BrianTools.SimpleGrid
{
    public class SimpleGridElement : MonoBehaviour
    {
        private SimpleGridManager _grid;
        public SimpleGridManager Grid
        {
            get { return _grid; }
            set { _grid = value; }
        }

        [SerializeField]
        private object _gridElementValue;
        public object GridElementValue
        {
            get { return _gridElementValue; }
            set { _gridElementValue = value; }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}