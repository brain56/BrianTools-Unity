using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace BrianTools
{
    namespace SimpleGrid
    {
        public class SimpleGridManager : MonoBehaviour
        {
            [SerializeField]
            private GameObject _gridElementPrefab;

            [SerializeField]
            private List<List<SimpleGridElement>> _gridElements;

            [SerializeField]
            private Vector3 _gridElementPadding;

            [SerializeField]
            private Vector3 _startingSize;

            public SimpleGridElement GetGridElement(IntVector2 elementIndex)
            {
                return _gridElements[elementIndex.x][elementIndex.y];
            }

            public void InitializeGrid()
            {
                _gridElements = new List<List<SimpleGridElement>>(Mathf.RoundToInt(_startingSize.x));

                for (int x = 0; x < _startingSize.x; x++)
                {
                    _gridElements.Insert(x, new List<SimpleGridElement>(Mathf.RoundToInt(_startingSize.z)));
                    for (int z = 0; z < _startingSize.z; z++)
                    {
                        GameObject instance = CreateGridElement();
                        instance.transform.parent = this.transform;
                        instance.transform.localPosition = new Vector3(x, 0, z);
                        instance.name = "Cell (" + x + ",0," + z + ")";

                        SimpleGridElement gridComp = instance.GetComponent<SimpleGridElement>();
                        _gridElements[x].Insert(z, gridComp);
                        gridComp.Grid = this;

                    }
                }
            }

            private GameObject CreateGridElement()
            {
                GameObject instance = GameObject.Instantiate(_gridElementPrefab) as GameObject;
                return instance;
            }
        }
    }
}