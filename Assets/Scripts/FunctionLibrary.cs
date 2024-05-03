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
    public delegate float Function (float x, float z, float t);

    // an array of delegates
    static Function[] functions = {
        Wave,
        MultiWave,
        Ripple,
        Test
    };

    // an enum to select which function to use
    public enum FunctionName {
        Wave,
        MultiWave,
        Ripple,
        Test
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

    // *1 sine wave 
    public static float Wave(float x, float z, float t) {
        // create a diagonal wave by using the sum of x & z
        return Sin(PI * (x +z+ t));
    }

    // *2 A more complex sine function
    // to add more complexity we add another sine function that has the double frequency
    public static float MultiWave(float x, float z, float t) {
        //* let's make each wave use a separate dimension 
        //* we use x for the first wave
        float y = Sin(PI * (x + 0.5f * t));

        // division requires a bit more work than multiplication => rule of thumb to prefer multiplication over division
        ////y += Sin(2f * PI * (x + t)) / 2f;

        //* we use z dimension for the second wave
        y += 0.5f * Sin(2f * PI * (z + t));

        //* add a 3rd wave that travels the XZ diagonal
        // we'll use the same wave as the wave function
        // except the time is going to be slowed down to 1/4
        y += Sin(PI * (x + z + 0.25f * t));

        // the max & min values of this new wave is 1.5 and -1.5
        // => divide by 1.5 to garantee that we stay in -1,1 range
        ////return y / 1.5f;

        // constant expressions (like 2/3) are evaluated at compile time
        // so we'll use only multiplication at runtime
        ////return y * (2f/3f);

        // after adding the 3rd wave the result is divided by 2.5 to keep it in -1,1 range
        return y * (1f/ 2.5f);
    }

    // *3 a ripple like effect
    public static float Ripple(float x, float z, float t) {
        // by making the sine wave move away from the origin instead of always traveling in the same direction
        // we do it by basing it on the distance from the origin == the absolute value of x
        ////float d = Abs(x);
        
        //* to make the ripple spread in all directions on the XZ plane
        // => calculate disnatance in both directions
        // => Pythagorean theorem
        float d = Sqrt(x * x + z * z);

        ////float y = Sin(4f * PI * d); // y=sin(4πd)
        
        // to animate it (make it flow outward) => substract the time from the value we pass to Sin
        float y = Sin(PI * (4f * d - t));
        // the result is hard to see because y varies too much
        // => reduce it by decreasing the amplitude of the wave
        return y / (1f + 10f *d);
    }

    public static float Test (float x, float z, float t) {
		float d = Abs(x);
		float y = Sin(4f * PI * d);
		return y;
	}
    
}
