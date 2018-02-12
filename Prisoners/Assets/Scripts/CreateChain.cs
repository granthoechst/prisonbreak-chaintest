using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateChain : MonoBehaviour {

    // number of links in chain
    private int numLinks = 16;
    // distance gap between links in chain
    private float linkGap = .55f;

    // link prefabs
    public GameObject linkPrefab;
    public GameObject linkPrefab1;
    public GameObject linkPrefab2;

    // players to connect
    public GameObject player1;
    public GameObject player2;

    // list to contain chain links
    public List<GameObject> links = new List<GameObject>();

    // link currently being added to the chain
    public GameObject thisLink;
    // last link added to the chain
    public GameObject lastLink;

    // Use this for initialization
    void Start () {
        player1 = GameObject.FindGameObjectWithTag("Player1");
        Vector2 player1Pos = player1.GetComponent<Rigidbody2D>().position;
        player2 = GameObject.FindGameObjectWithTag("Player2");
        Vector2 player2Pos = player2.GetComponent<Rigidbody2D>().position;
        Vector2 linkPos = player1Pos;

        // attach initial link to player 1 and add to list
        thisLink = (GameObject)Instantiate(linkPrefab1, linkPos, Quaternion.identity);
        thisLink.GetComponent<HingeJoint2D>().connectedBody = player1.GetComponent<Rigidbody2D>();
        links.Add(thisLink);
        // move it to the desired position on the player
        thisLink.GetComponent<HingeJoint2D>().autoConfigureConnectedAnchor = false;
        thisLink.GetComponent<HingeJoint2D>().connectedAnchor = new Vector2(0.5f, 0.5f);

        lastLink = thisLink;

        // for loop adds numLinks number of links to the chain
        for (int i = 1; i < numLinks; i++)
        {
            // calculate the position of and instantiate the next link in the chain
            Vector2 lastPos = lastLink.GetComponent<Rigidbody2D>().position;
            //linkPos = Vector2.MoveTowards(lastPos, player2Pos, linkGap);
            linkPos = new Vector2(lastPos.x + linkGap, lastPos.y);
            if (i%2 == 0)
            {
                thisLink = (GameObject)Instantiate(linkPrefab1, linkPos, Quaternion.identity);
            } else
            {
                thisLink = (GameObject)Instantiate(linkPrefab2, linkPos, Quaternion.identity);
                thisLink.transform.SetSiblingIndex(0);
            }
            // connect it via hinge join to the previous link
            thisLink.GetComponent<HingeJoint2D>().connectedBody = lastLink.GetComponent<Rigidbody2D>();
            links.Add(thisLink);
            lastLink = thisLink;
        }

        // connect last link to Player2
        HingeJoint2D player2Joint = player2.AddComponent<HingeJoint2D>();
        player2Joint.connectedBody = lastLink.GetComponent<Rigidbody2D>();
        player2Joint.autoConfigureConnectedAnchor = false;
        player2Joint.connectedAnchor = new Vector2(2, 0);
        player2Joint.anchor = new Vector2(0.5f, 0.5f);


        // label links as children to Chain gameobject
        GameObject Chain = new GameObject("Chain");
        Chain.tag = "Chain";
        for (int i = 0; i < numLinks; i++)
        {
            links[i].transform.parent=Chain.transform;
        }

        // configure chain layer so it doesn't only collides with the ground layer
    }

    // Update is called once per frame
    void Update () {
		
	}
}
