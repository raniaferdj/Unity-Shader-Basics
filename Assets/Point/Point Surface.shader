/*
    unity has its own language for shaders: 
        it begins with the shader keyword 
        followed by a string defining a menu item for the shader
        after that comes a code block for the shader's content
*/
Shader "Graph/Point Surface" {
    /*
        shaders can have multiple subshaders each defined by the subshader keyword 
        followed by a code block
    */

    /* 
        3- to make the smoothness configuration appear in the editor we have to add a 'propeties' 
        block at the top of the shader, above subshader
    */
    Properties {
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
    }
    SubShader {
        /*
            the subshader of a surface needs a code section written in a hybrid of CG & HLSL
            this code must be enclosed by the CGPROGRAM & ENDCG keywords
        */

        CGPROGRAM

        /*
            1- we need a compiler directive (pragma) written as #pragma followed by a directive 
        */

        // instruct shader compiler to generate a surface shader with standard lighting and full support for shadows
        #pragma surface ConfigureSurface Standard fullforwardshadows
        // sets a minimum for shader's target level and quality
        #pragma target 3.0

        /*
            2- color our points based on their world position
        */

        // define the input structure for our configuration function
        struct Input {
            // declare a single struct field that will contain the world position of what gets rendered
            //(float3 type is the shader equivalent of the Vector3 struct)
            float3 worldPos;
        };

        // make smoothness configurable
        float _Smoothness;

        // define a function that has 2 parameters :
        //1st an input parameter of type Input (defined above)
        //2nd is the surface configuration data 
        // "inout" indicates that it's both passed to the function and used for the result of the function
        void ConfigureSurface (Input input, inout SurfaceOutputStandard surface) {
            /* 
                to adjust the colors of our points we modify the albedo 
                as both albedo and world surface have 3 componens 
                X position controls the point's red color component, Y controls green and Z controls blue
                '* 0.5 + 0.5' to fit the colors in the domain, as negative colors make no sense
            */

            ////surface.Albedo = input.worldPos* 0.5 + 0.5;
            
            //*we can eliminate the blue by including only the red and green channels when setting the albedo
            //* the saturate function (common operation in shaders) => clamp colors => ensure they remain in the 0-1 range
            surface.Albedo.rg = saturate(input.worldPos.xy * 0.5 + 0.5); // => blue component stays 0
            
            // make it look like the default material (average smoothness)
            //// surface.Smoothness = 0.5;

            // make smoothness configurable
            surface.Smoothness = _Smoothness;
            
        }

        ENDCG
    }

    // add a fallback (?) to the standard diffuse shader
    Fallback "Diffuse"
}