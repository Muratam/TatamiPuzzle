Shader "Custom/Mono"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_X ("Normalized X",Range(0,1)) = 0.2
		_Y ("Normalized Y",Range(0,1)) = 0.2
		_W ("Normalized Width",Range(0,1)) = 0.6
		_H ("Normalized Height",Range(0,1)) = 0.6
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Fog { Mode Off }
		Pass
		{
         GLSLPROGRAM
                  
         uniform sampler2D _MainTex;	
         uniform vec4 _MainTex_ST; 
         uniform float _X; 
         uniform float _Y; 
         uniform float _W; 
         uniform float _H; 

         #ifdef VERTEX                  
         out vec4 textureCoordinates; 
         void main(){
            textureCoordinates = gl_MultiTexCoord0;
            gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
         }         
         #endif

         #ifdef FRAGMENT        
         in vec4 textureCoordinates; 
         void main(){
            vec4 c = texture2D(_MainTex,  _MainTex_ST.xy * textureCoordinates.xy + _MainTex_ST.zw);         
            if(textureCoordinates.x > _X && textureCoordinates.x < _X + _W && 
               textureCoordinates.y > _Y && textureCoordinates.y < _Y + _H){
                float mono = 0.298912 * c.r + 0.586611 * c.g + 0.114478 * c.b;
                gl_FragColor = vec4(mono,mono,mono,0);
            }else{
	            gl_FragColor = c;
            }
         }
         #endif

         ENDGLSL
		}
	}
}
