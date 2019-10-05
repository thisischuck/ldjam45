using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public GameObject enemyParent;
    // Start is called before the first frame update
    private Vector3 leftSpawner, rightSpawner, topLeft, topRight, midLeft, midRight;

    public List<int> list;

    bool started;

    void Start()
    {
        leftSpawner = transform.GetChild(0).transform.position;
        rightSpawner = transform.GetChild(1).transform.position;
        topLeft = transform.GetChild(2).transform.position;
        topRight = transform.GetChild(3).transform.position;
        midLeft = transform.GetChild(4).transform.position;
        midRight = transform.GetChild(5).transform.position;
        Fill();
    }

    // Update is called once per frame
    void Update()
    {
        if (!started)
            StartCoroutine("SpawnerTimer");
    }

    void Fill()
    {
        list = new List<int>();
        for (int i = 0; i < 6; i++)
            list.Add(i);
    }

    IEnumerator SpawnerTimer()
    {
        started = true;
        Vector3 position;
        yield return new WaitForSecondsRealtime(2f);

        var index = Random.Range(0, list.Count);

        #region Position
        int r = list[index];
        if (r == 0) position = leftSpawner;
        else if (r == 1) position = rightSpawner;
        else if (r == 2) position = topLeft;
        else if (r == 3) position = topRight;
        else if (r == 4) position = midLeft;
        else position = midRight;
        #endregion

        list.RemoveAt(index);
        if (list.Count == 0)
            Fill();

        GameObject obj = Instantiate(enemyParent, position, this.transform.rotation);
        obj.GetComponent<EnemyController>().hp = Random.Range(1, 4);

        if (r == 4)
            obj.GetComponent<EnemyController>().FlyLikeAnAngel(1);
        else if (r == 5)
            obj.GetComponent<EnemyController>().FlyLikeAnAngel(-1);

        started = false;
    }
}
