using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[AddComponentMenu("")]
public class NodeCreator : MonoBehaviour 
{
    public int chooseSplineNum;
    public GameObject baseSplines;
	public List<Spline> spline;
	public List<SplineMesh> splineMesh;

    private Vector3 up;
    private Vector3 down;
    private Vector3 left;
    private Vector3 right;

    void Start()
    {
        up = new Vector3(0, 1, 0);
        down = new Vector3(0, -1, 0);
        left = new Vector3(-1, 0, 0);
        right = new Vector3(1, 0, 0);
        if (baseSplines == null)
        {
            Debug.Log("BaseSplines is NULL!");
            return;
        }
        foreach (Transform trans in baseSplines.transform)
        {
            if (trans.GetComponent<Spline>() != null && trans.GetComponent<SplineMesh>() != null)
            {
                spline.Add(trans.GetComponent<Spline>());
                splineMesh.Add(trans.GetComponent<SplineMesh>());
            }
        }
    }
	// Update is called once per frame
	void Update () 
	{
		Move( );
		
		//Insert a new node if space is pressed
		if( Input.GetKeyDown( KeyCode.Space ) )
		{
			//Create a new spline node at the spline's end and store the created gameObject in a variable.
            GameObject newSplineNode = spline[chooseSplineNum].AddSplineNode();
			
			//Set the new node's position to the current position of the character. 
			newSplineNode.transform.position = transform.position;

            newSplineNode.transform.parent = spline[chooseSplineNum].gameObject.transform;
			//Increase the segment count of the spline mesh, so that it doesn't look edgy when 
			//the spline gets very long
            splineMesh[chooseSplineNum].segmentCount += 3;
		}
		
		//Delete the first node when X is pressed
		if( Input.GetKeyDown( KeyCode.X ) )
		{
			//Get the array of nodes
            SplineNode[] splineNodes = spline[chooseSplineNum].SplineNodes;
			
			//If there are no spline nodes left, return
			if( splineNodes.Length < 1 )
				return;
			
			//Get the spline's last node
            SplineNode lastNode = splineNodes[splineNodes.Length - 1];
			
			//Remove it from the spline
            spline[chooseSplineNum].RemoveSplineNode(lastNode);

            splineMesh[chooseSplineNum].segmentCount -= 3;
		}
	}
	
	void Move( )
	{
		if( Input.GetKey( KeyCode.W ) )
			transform.position = transform.position + up * Time.deltaTime * 60f;
		if( Input.GetKey( KeyCode.S ) )
			transform.position = transform.position + down * Time.deltaTime * 60f;
		
		if( Input.GetKey( KeyCode.A ) )
			transform.position = transform.position + left * Time.deltaTime * 60f;
		if( Input.GetKey( KeyCode.D ) )
			transform.position = transform.position + right * Time.deltaTime * 60f;
	}
}
