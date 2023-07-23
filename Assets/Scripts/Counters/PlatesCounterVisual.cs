using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField]
    private float plateOffsetY = 0.1f;
    [SerializeField]
    private PlatesCounter platesCounter;
    [SerializeField]
    private Transform counterTopPoint;
    [SerializeField]
    private Transform plateVisualPrefab;

    private List<GameObject> platesVisualGameObjectList;

    private void Awake()
    {
        platesVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
        platesCounter.OnPlateRemoved += PlatesCounter_OnPlatePicked;
    }

    private void PlatesCounter_OnPlatePicked(object sender, EventArgs e)
    {
        var lastPlate = platesVisualGameObjectList[platesVisualGameObjectList.Count - 1];
        
        platesVisualGameObjectList.Remove(lastPlate);

        Destroy(lastPlate);
    }

    private void PlatesCounter_OnPlateSpawned(object sender, System.EventArgs e)
    {
        var plate = Instantiate(plateVisualPrefab, counterTopPoint);

        plate.localPosition = new Vector3(0, plateOffsetY * platesVisualGameObjectList.Count, 0);

        platesVisualGameObjectList.Add(plate.gameObject);
    }


}
