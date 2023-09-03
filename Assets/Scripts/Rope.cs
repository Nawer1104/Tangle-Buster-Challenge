using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour
{
    public bool isEndRope;

    private bool isClicked = false;

    public Rigidbody2D hook;

    public GameObject linkPrefab;

    public int links = 7;

    public Rope connectedRopeEnd;

    public GameObject vfxOnClick;

    void Start()
    {
        if (!isEndRope)
        {
            GenerateRope();
        }

    }

    void GenerateRope()
    {
        Rigidbody2D previousRB = hook;
        for (int i = 0; i < links; i++)
        {
            GameObject link = Instantiate(linkPrefab, transform);
            HingeJoint2D joint = link.GetComponent<HingeJoint2D>();
            joint.connectedBody = previousRB;

            if (i < links - 1)
            {
                previousRB = link.GetComponent<Rigidbody2D>();
            }
            else
            {
                HingeJoint2D jointEnd = connectedRopeEnd.GetComponent<HingeJoint2D>();
                jointEnd.connectedBody = link.GetComponent<Rigidbody2D>();
                /*jointEnd.anchor = Vector2.zero;
                jointEnd.connectedAnchor = new Vector2(0f, -3f);*/
            }
        }
    }

    private void OnMouseDown()
    {
        if (isClicked) return;
        isClicked = true;
        GameObject vfx = Instantiate(vfxOnClick, transform.position, Quaternion.identity);
        Destroy(vfx, 1f);

        hook.GetComponent<HingeJoint2D>().enabled = false;
        gameObject.GetComponent<HingeJoint2D>().enabled = false;

        StartCoroutine(Delay(2));

    }

    private IEnumerator Delay(int seceond)
    {
        yield return new WaitForSeconds(seceond);

        GameManager.Instance.levels[GameManager.Instance.GetCurrentIndex()].ropes.Remove(this);
    }
}