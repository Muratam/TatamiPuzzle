Shader "Custom/Tatami"{
	Properties	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader{
		Cull Off 
		Pass{
         GLSLPROGRAM
                  
         uniform sampler2D _MainTex;	
         uniform vec4 _MainTex_ST; 

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
            vec2 pos = 
            vec4 c = texture2D(_MainTex, textureCoordinates.xy);         
	        gl_FragColor = c;
         }
         #endif

         ENDGLSL
		}
	}
}
