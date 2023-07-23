using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounter : BaseCounter, IKitchenObjectParent
{
    public event EventHandler OnPlateSpawned;
    public event EventHandler OnPlateRemoved;

    [SerializeField]
    private float spawnPlateTimerMax;
    [SerializeField]
    private int platesSpawnAmountLimit;
    [SerializeField]
    private KitchenObjectScriptableObject scriptableObject;

    private float spawnPlateTimer;
    private int platesSpawnAmount;

    private void Update()
    {
        spawnPlateTimer += Time.deltaTime;

        if (spawnPlateTimer > spawnPlateTimerMax)
        {
            spawnPlateTimer = 0;

            if (platesSpawnAmount < platesSpawnAmountLimit)
            {
                platesSpawnAmount++;
                OnPlateSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public override void Interact(Player player)
    {
        if (!player.HasKitchenObject() && platesSpawnAmount > 0)
        {
            platesSpawnAmount--;
            OnPlateRemoved?.Invoke(this, EventArgs.Empty);
           
            KitchenObject.Spawn(scriptableObject, player);  
        }
    }
}
