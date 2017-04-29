Shader "ZonkaZombies/SplitscreenMask" 
{ 
	SubShader 
	{
		// Apply this shader before drawing geometry
		Tags { "Queue" = "Geometry-500" }
 
		ColorMask 0
		ZWrite On
		Pass 
		{
			Stencil{
				Ref 1
				Comp always
				Pass replace
			}
		}
	}
}