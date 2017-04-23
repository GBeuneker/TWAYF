using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject currentBlock;
    private GameObject[] allBlocks;
    private GameObject blockPreview;

    // Use this for initialization
    void Start()
    {
        allBlocks = Resources.LoadAll<GameObject>("Prefabs/Blocks");
        GetRandomBlock();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject GetRandomBlock()
    {
        GameObject answer = currentBlock;

        currentBlock = allBlocks[Random.Range(0, allBlocks.Length)];
        Destroy(blockPreview);
        blockPreview = Instantiate(currentBlock);

        Destroy(blockPreview.GetComponent<Collider2D>());
        Destroy(blockPreview.GetComponent<Rigidbody2D>());

        blockPreview.transform.SetParent(transform);
        blockPreview.transform.localPosition = new Vector2(-2, 0);
        blockPreview.transform.localRotation = Quaternion.identity;

        StartCoroutine(ShowNextBlock());
        return answer;
    }

    IEnumerator ShowNextBlock()
    {
        float delta = 0;
        if (!SceneLoader.Instance.isPlaying)
            delta = 1;

        while (delta <= 1)
        {
            delta += Time.deltaTime * 5;
            blockPreview.transform.localPosition = Vector3.Lerp(new Vector2(-2, 0), Vector2.zero, delta);
            blockPreview.GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, delta);
            yield return null;
        }
        yield return null;
    }
}
