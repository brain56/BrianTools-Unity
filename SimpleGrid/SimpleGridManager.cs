using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BrianTools.SimpleGrid
{
    public class SimpleGridManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _gridElementPrefab;

        [SerializeField]
        private Dictionary<IntVector2, SimpleGridElement> _gridElements;

        [SerializeField]
        private Vector3 _gridElementPadding;
        [SerializeField]
        private Vector3 _gridElementSize;

        [SerializeField]
        private Vector3 _startingSize;

        public SimpleGridElement GetGridElement(IntVector2 elementIndex)
        {
            return _gridElements[elementIndex];
        }

        [ContextMenu("InitializeGrid()")]
        public void InitializeGrid()
        {
            _gridElements = new Dictionary<IntVector2, SimpleGridElement>();//new List<List<SimpleGridElement>>(Mathf.RoundToInt(_startingSize.x));

            for (int x = -(int)_startingSize.x / 2; x < _startingSize.x / 2; x++)
            {
                //_gridElements.Add(x, new List<SimpleGridElement>(Mathf.RoundToInt(_startingSize.z)));
                for (int z = -(int)_startingSize.z / 2; z < _startingSize.z / 2; z++)
                {
                    GameObject instance = CreateGridElement();
                    instance.transform.parent = this.transform;
                    instance.transform.localPosition = new Vector3(x * _gridElementSize.x, 0 * _gridElementSize.y, z * _gridElementSize.z);
                    instance.name = "Cell (" + x + ",0," + z + ")";

                    SimpleGridElement gridComp = instance.GetComponent<SimpleGridElement>();
                    //_gridElements[x].Insert(z, gridComp);
                    gridComp.Grid = this;

                    IntVector2 indexer = new IntVector2(x, z);
                    _gridElements.Add(indexer, gridComp);

                }
            }
        }

        private GameObject CreateGridElement()
        {
            GameObject instance = GameObject.Instantiate(_gridElementPrefab) as GameObject;
            return instance;
        }

        public SimpleGridElement GetGridElementAtInputPosition()
        {
            SimpleGridElement element = null;

            RaycastHit hit;
            Camera camera = Camera.main;
            Ray ray = camera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1000));
            Debug.DrawRay(ray.origin, ray.direction * 1000);

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log("Hit: " + hit.collider.gameObject.name);
                element = hit.collider.gameObject.GetComponent<SimpleGridElement>();
            }
            return element;
        }
    }
}