using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject slimePrefab;// define the object that is going to be used
    [SerializeField]
    private GameObject bigSlimePrefab;
    [SerializeField]
    private float slimeInterval = 1.5f;// seconds between each slime
    [SerializeField]
    private int slimesToKill = 10;// how many slimes appear (and must be killed) before the big slime

    // NOTE: the room is 20 x 12, so the walls are at X = ±10 and Y = ±6.
    // These limits are smaller on purpose, so no slime appears on or outside the walls.
    [SerializeField]
    private float limitX = 8f;
    [SerializeField]
    private float limitY = 4.5f;

    // list of all the slimes that were spawned, used to know when they are all dead
    private List<EnemyHealth> slimes = new List<EnemyHealth>();

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        // 1. spawn the slimes one by one
        for (int i = 0; i < slimesToKill; i++)
        {
            Vector3 position = new Vector3(Random.Range(-limitX, limitX), Random.Range(-limitY, limitY), 0f);// random position inside the room
            GameObject newSlime = Instantiate(slimePrefab, position, Quaternion.identity);

            slimes.Add(newSlime.GetComponent<EnemyHealth>());// save it to check later if it died

            yield return new WaitForSeconds(slimeInterval);// wait before the next one
        }

        // 2. wait here until every slime is dead
        while (AnySlimeAlive())
        {
            yield return null;// check again next frame
        }

        // 3. all slimes dead: the big slime appears in the center
        Instantiate(bigSlimePrefab, Vector3.zero, Quaternion.identity);
    }

    private bool AnySlimeAlive()
    {
        foreach (EnemyHealth slime in slimes)
        {
            // a slime counts as alive only if it still exists and is not dead
            if (slime != null && !slime.IsDead)
            {
                return true;
            }
        }
        return false;
    }
}