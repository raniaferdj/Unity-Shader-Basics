using UnityEngine;

public class Graph : MonoBehaviour //MonoBehaviour extends Behaviour, which extends Component, which extends Object.
{
    [SerializeField] 
    Transform pointPrefab;

    // add a variable resolution (with enforced range of 10-100)
    [SerializeField, Range(10, 100)]
    int resolution = 10;

    // selecting functions in the editor
    ////[SerializeField, Range(0, 2)]int
    [SerializeField]
    FunctionLibrary.FunctionName function;

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
        ////var position = Vector3.zero;
        // reduce the scale of the object to better see them in the new domain
        var scale = Vector3.one * step;

        ////  create an array of points of length equal to resolution of the graph
        ////points = new Transform[resolution];
        
        // *add another dimension => change the array to a grid of points
        points = new Transform[resolution * resolution];

        // points.length is equal to resolution
        /* 
            *we track x too
            *each row has to be offset along the z dimension 
            *=> add z to the loop
            *=> z will only increment when we move to the next row
        */

        ////for (int i = 0, x = 0, z = 0 ; i<points.Length; i++, x++){
        for (int i = 0 ; i<points.Length; i++){
            // *each time we finish a row we reset x and increment z
            //// if (x == resolution){
            ////     x = 0;
            ////     z += 1;
            //// }
            // instantiate clones the Unity object passed to it => add an instance of the prefab to the scene
            // fill the array with references to our points
            Transform point = points[i] = Instantiate(pointPrefab);
            //point.localPosition = Vector3.right * i;

            ////position.x = (i + 0.5f)*step - 1f; // to fill the -1, 1 range
            //* we use x instead of i to calculate the x coordinate
            ////position.x = (x + 0.5f)*step - 1f;
            //* set the z coordinate
            ////position.z = (z + 0.5f)*step - 1f;
            // to adjust the graph on each frame we need to set the Y in the update method
            //f(x) = x^3
            ////position.y = position.x * position.x * position.x;
            //// position.y = Mathf.Sin(position.x * 4f) / 2f + 0.5f;
            ////point.localPosition = position;
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

    //* now that X and Z are no longer constant => replace update loop with the same as awake loop
    void Update () {
        // get a function delegate based on the function selected in the editor
        FunctionLibrary.Function f = FunctionLibrary.GetFunction(function);

        float time = Time.time;
        float step = 2f / resolution; //added
        float v = 0.5f * step - 1f;
        for (int i = 0, x=0, z=0; i<points.Length; i++, x++){ //modified
            //added
            // we only need to recalculate v when z changes
            if (x == resolution){
                x = 0;
                z += 1;
                v = (z + 0.5f) * step - 1f;
            }
            float u = (x + 0.5f) * step - 1f;
            ////float v = (z + 0.5f) * step - 1f;

            points[i].localPosition = f(u, v, time);

            // get a reference of the current array element and store it in a variable
            ////Transform point = points[i];
            // set the Y coordinate based on X
            ////Vector3 position = point.localPosition;

            // point.localPosition.y is not a public value
            ////position.y = position.x * position.x * position.x;
            // we will use the sine function because it changes based on time
            // we scale x by pi to see the whole repeating pattern
            // to animate this function we add the current game time to X before calculating the sine function 
            ////position.y = Mathf.Sin(Mathf.PI * (position.x + time));
            // we will use the wave function from FunctionLibrary => it's the same

            // we invoke the delegate variable instead of an explicit method
            ////position.y = f(position.x, position.z, time);
            ////point.localPosition = position;
            

        }
    }
}
