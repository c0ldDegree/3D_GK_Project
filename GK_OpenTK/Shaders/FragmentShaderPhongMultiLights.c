#version 450 core


#define MAX_LIGHTS 10
uniform int numLights;
uniform struct Light {
	vec3 LightPosition_worldspace;
} allLights[MAX_LIGHTS];

layout(location = 21) uniform  mat4 view;
layout(location = 20) uniform  mat4 model;
// Interpolated values from the vertex shaders
in vec2 UV;
in vec3 Position_worldspace;
in vec3 Normal_cameraspace;
in vec3 EyeDirection_cameraspace;

// Ouput data
out vec3 color;

uniform sampler2D tex;


void main()
{
	//		vec2 flip_textCoord = vec2(TextCoord.x,1-TextCoord.y);
	//		outColor = texture(tex,flip_textCoord);

	// Light emission properties
	// You probably want to put them as uniforms
	vec3 LightColor = vec3(1, 1, 1);
	float LightPower = 100.0f;

	// Material properties
	vec2 flip_textCoord = vec2(UV.x, 1 - UV.y);
	vec3 MaterialDiffuseColor = texture(tex, flip_textCoord).rgb;
	vec3 MaterialAmbientColor = 0.01f*vec3(0.1, 0.1, 0.1) * MaterialDiffuseColor;
	vec3 MaterialSpecularColor = vec3(0.3, 0.3, 0.3);

	for (int i = 0; i < numLights; i++) {
		vec3 LightPosition_cameraspace = (view * vec4(allLights[i].LightPosition_worldspace, 1)).xyz;
		vec3 LightDirection_cameraspace = LightPosition_cameraspace + EyeDirection_cameraspace;
		// Distance to the light
		float distance = length(allLights[i].LightPosition_worldspace - Position_worldspace);

		// Normal of the computed fragment, in camera space
		vec3 n = normalize(Normal_cameraspace);
		// Direction of the light (from the fragment to the light)
		vec3 l = normalize(LightDirection_cameraspace);
		// Cosine of the angle between the normal and the light direction, 
		// clamped above 0
		//  - light is at the vertical of the triangle -> 1
		//  - light is perpendicular to the triangle -> 0
		//  - light is behind the triangle -> 0
		float cosTheta = clamp(dot(n, l), 0, 1);

		// Eye vector (towards the camera)
		vec3 E = normalize(EyeDirection_cameraspace);
		// Direction in which the triangle reflects the light
		vec3 R = reflect(-l, n);
		// Cosine of the angle between the Eye vector and the Reflect vector,
		// clamped to 0
		//  - Looking into the reflection -> 1
		//  - Looking elsewhere -> < 1
		float cosAlpha = clamp(dot(E, R), 0, 1);

		color = color +
			// Ambient : simulates indirect lighting
			MaterialAmbientColor +
			// Diffuse : "color" of the object
			MaterialDiffuseColor * LightColor * LightPower * cosTheta / (distance*distance) +
			// Specular : reflective highlight, like a mirror
			MaterialSpecularColor * LightColor * LightPower * pow(cosAlpha, 5) / (distance*distance);
	}
}