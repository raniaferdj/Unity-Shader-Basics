using UnityEngine;

public class Graph : MonoBehaviour //MonoBehaviour extends Behaviour, which extends Component, which extends Object.
{
    [SerializeField] 
    Transform pointPrefab;

    // add a variable resolution (with enforced range of 10-100)
    [SerializeField, Range(10, 100)]
    int resolution = 10;

    // tracking the points for animating the graph
    Transform[] points;

    void Awake (){

        /**
        the idea for the positions of the cubes is : vector(x, f(x), 0)

        when working with functions we often use a range of 0-1 for x 
        or when working with functions centered around 0 (vectors?) we use a range of -1-1 
        */
        // adjust the step
        float step = 2f / resolution;
        var position = Vector3.zero;
        // reduce the scale of the object to better see them in the new domain
        var scale = Vector3.one * step;

        //  create an array of points of length equal to resolution of the graph
        points = new Transform[resolution];
        // points.length == resolution
        for (int i = 0; i<points.Length; i++){
            // instantiate clones the Unity object passed to it => add an instance of the prefab to the scene
            // fill the array with references to our points
            Transform point = points[i] = Instantiate(pointPrefab);
            point.localPosition = Vector3.right * i;
            position.x = (i + 0.5f)*step - 1f; // to fill the -1, 1 range
            // to adjust the graph on each frame we need to set the Y in the update method
            //f(x) = x^3
            ////position.y = position.x * position.x * position.x;
            //// position.y = Mathf.Sin(position.x * 4f) / 2f + 0.5f;
            point.localPosition = position;
            point.localScale = scale;

            // set the point object to be a child of graph object
            point.SetParent(transform, false); 
            // false because we don't want to move the object 
            //(Unity will attempt to keep the object at its original world position, rotation, and scale)
        }

        // // we reuse point variable because we don't need to hold on to a reference once we're done with it
        // point = Instantiate(pointPrefab); 
        // point.localPosition = Vector3.right * 2f;
    }
    void Update () {
        float time = Time.time;
        for (int i = 0; i<points.Length; i++) {
            // get a reference of the current array element and store it in a variable
            Transform point = points[i];
            // set the Y coordinate based on X
            Vector3 position = point.localPosition;
            // point.localPosition.y is not a public value
            ////position.y = position.x * position.x * position.x;
            // we will use the sine function because it changes based on time
            // we scale x by pi to see the whole repeating pattern
            // to animate this function we add the current game time to X before calculating the sine function 
            position.y = Mathf.Sin(Mathf.PI * (position.x + time));
            point.localPosition = position;
            

        }
    }
}
