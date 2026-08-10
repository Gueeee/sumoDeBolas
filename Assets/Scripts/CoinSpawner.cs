using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour {
    public GameObject CoinPrefab;
    public float cooldown = 3f;

    void Start() {
        StartCoroutine(SpawnCoin());
    }

    IEnumerator SpawnCoin() {
        yield return new WaitForSeconds(3f);

        float CoinX = UnityEngine.Random.Range(-6, 6);
        float CoinZ = UnityEngine.Random.Range(-6, 6);
        Vector3 CoinPos = new Vector3(CoinX, 1f, CoinZ);
        
        Instantiate(CoinPrefab, CoinPos, Quaternion.identity);

        // loop infinito
        StartCoroutine(SpawnCoin());        
    }
}
