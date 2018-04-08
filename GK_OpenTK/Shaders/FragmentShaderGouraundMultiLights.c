#version 450 core
//precision mediump float;       // Set the default precision to medium. We don't need as high of a
// precision in the fragment shader.
uniform sampler2D tex;
in 	float dif;
in vec2 UV;
out vec4 color;

void main()
{
	vec2 flip_textCoord = vec2(UV.x, 1 - UV.y);
	color = texture(tex, flip_textCoord)*dif;   // Pass the color directly through the pipeline.
}