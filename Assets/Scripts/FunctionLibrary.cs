using UnityEngine;
using static UnityEngine.Mathf; // we add static to use all constant and static methods

/*
    this class will  contain multiple mathematical functions
    this class isn't going to be a component type
    and we're not going to create an object instance of it => it'll be a static class
    (so it doesn't need a monobehaviour) 
    instead we'll use it to provide a collection of publicly accessible methods
*/
public static class FunctionLibrary 
{
    /* 
        we'll use delegates to design what method to use
        *delegate* is a reference type (similar to function pointer in C/C++) 
        => tells which method to be called when an event is triggered
    */
    ////public delegate float Function (float x, float z, float t);
    //* we want our functions to outpt 3d positions instead of 1d values
    // => we use 3d vectors instead of 1d floats
    public delegate Vector3 Function (float u, float v, float t);

    // an array of delegates
    static Function[] functions = {
        Wave,
        MultiWave,
        Ripple,
        Sphere,
        Torus
    };

    // an enum to select which function to use
    public enum FunctionName {
        Wave,
        MultiWave,
        Ripple,
        Sphere,
        Torus
    }

    // the method that will be called to choose the function
    public static Function GetFunction (FunctionName name) {
        // cast the enum value to an int
        return functions[(int)name];
    }

    /*  by default methods are instance methods (need to be invoked by an object instance) 
        => make them static
        the method returns a floating_point number (float)
        the method has 2 parameters :
            x is the x coordinate of the point
            t is the time
    */

    //* 1. ************** sine wave *********************************

    // public static float Wave(float x, float z, float t) {
    //     // create a diagonal wave by using the sum of x & z
    //     return Sin(PI * (x +z+ t));
    // }
    // modify the functions to return a 3d vector
    public static Vector3 Wave(float u, float v, float t) {
        ////return Sin(PI * (x +z+ t));
        Vector3 p;
        p.x = u;
        // create a diagonal wave by using the sum of x & z
        p.y = Sin(PI * (u + v + t));
        p.z = v;
        return p;
    }

    //* 2. ************** A more complex sine function *********************************
    // to add more complexity we add another sine function that has the double frequency
    public static Vector3 MultiWave(float u, float v, float t) {
        Vector3 p; 
        p.x = u;
        //* let's make each wave use a separate dimension 
        //* we use x for the first wave
        p.y = Sin(PI * (u + 0.5f * t));

        // division requires a bit more work than multiplication => rule of thumb to prefer multiplication over division
        ////y += Sin(2f * PI * (x + t)) / 2f;

        //* we use z dimension for the second wave
        p.y  += 0.5f * Sin(2f * PI * (v + t));

        //* add a 3rd wave that travels the XZ diagonal
        // we'll use the same wave as the wave function
        // except the time is going to be slowed down to 1/4
        p.y  += Sin(PI * (u + v + 0.25f * t));

        // the max & min values of this new wave is 1.5 and -1.5
        // => divide by 1.5 to garantee that we stay in -1,1 range
        ////return y / 1.5f;

        // constant expressions (like 2/3) are evaluated at compile time
        // so we'll use only multiplication at runtime
        ////return y * (2f/3f);

        // after adding the 3rd wave the result is divided by 2.5 to keep it in -1,1 range
        ////return y * (1f/ 2.5f);
        p.y *= (1f/ 2.5f);
        p.z = v;
        return p; 
    }

    //* 3. ************** a ripple like effect *********************************
    public static Vector3 Ripple(float u, float v, float t) {
        // by making the sine wave move away from the origin instead of always traveling in the same direction
        // we do it by basing it on the distance from the origin == the absolute value of x
        ////float d = Abs(x);
        
        //* to make the ripple spread in all directions on the XZ plane
        // => calculate disnatance in both directions
        // => Pythagorean theorem
        float d = Sqrt(u * u + v * v);

        Vector3 p;
        p.x = u;

        ////float y = Sin(4f * PI * d); // y=sin(4πd)
        
        // to animate it (make it flow outward) => substract the time from the value we pass to Sin
        ////float y = Sin(PI * (4f * d - t));
        
        p.y = Sin(PI * (4f * d - t));

        // the result is hard to see because y varies too much
        // => reduce it by decreasing the amplitude of the wave
        ////return y / (1f + 10f *d);
        p.y /= (1f + 10f *d);
        p.z = v;
        return p;
    }

    // public static float Test (float x, float z, float t) {
	// 	float d = Abs(x);
	// 	float y = Sin(4f * PI * d);
	// 	return y;
	// }

    // this is a UV sphere
    public static Vector3 Sphere (float u, float v, float t) {
        //*perturbing the sphere by changing r and adding s
        //*radius
        //float r = Cos(0.5f * PI * v);
        //float r = 0.5f + 0.5f * Sin(PI * t);
        //float r = 0.9f + 0.1f * Sin(8f * PI * u);
        //float r = 0.9f + 0.1f * Sin(8f * PI * v);
        //* using both u and v to get twisting bands and adding time make them rotate
        float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        float s = r * Cos(0.5f * PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin(PI * 0.5f * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    //* create the torus from the sphere (a ring torus)
    //* how far we pull the sphere apart influences the shape of the torus 
    //* => defining the major radius of the sphere
    //* the other radius (minor radius) determines the thickness of the ring
    public static Vector3 Torus (float u, float v, float t){
        ////float r = 0.9f + 0.1f * Sin(PI * (6f * u + 4f * v + t));
        ////set radius to 1
        ////float r = 1f;
        
        /* 
        * we animate the torus by turning it into a rotating star pattern
        */
        // major radius
        float r1 = 0.7f + 0.1f * Sin(PI * (6f * u + 0.5f * t)); ////0.75f;
        // minor radius
        float r2 = 0.15f + 0.05f * Sin(PI * (8f * u + 4f * v + 2f * t)); ////0.25f;

        //* morph the sphere into a torus by pulling its half-circles away from each other
        //* and turning them into full circles
        //* use v to describe an entire circle instead of half

        ////float s = r * Cos(0.5f * PI * v);
        ////float s = 0.5f + r * Cos(PI * v);
        float s = r1 + r2 * Cos(PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        ////p.y = r * Sin(0.5f * PI * v); // change r with it's interesting
        p.y = r2 * Sin( PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    // create the torus from the sphere
    // because we pulled the sphere apart by half unit => self intersecting torus
    public static Vector3 SpindleTorus (float u, float v, float t){
        //set radius to 1
        float r = 1f;
        float s = 0.5f + r * Cos(PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin( PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }

    // create the torus from the sphere
    // pulling the sphere apart by one unit 
    //=> torus that doesnt intersect
    // but doesnt have a hole
    public static Vector3 HornTorus (float u, float v, float t){
        //set radius to 1
        float r = 1f;
        float s = 1f + r * Cos(PI * v);
        Vector3 p;
        p.x = s * Sin(PI * u);
        p.y = r * Sin( PI * v);
        p.z = s * Cos(PI * u);
        return p;
    }
    
}
