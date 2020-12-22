#version 330

out vec4 outputColor;

in vec2 texCoord;
in vec3 pos;

uniform sampler2D screenTexture;
uniform sampler2D lightTexture;

void main()
{
	outputColor = texture(screenTexture, texCoord) * texture(lightTexture, texCoord);
	//outputColor = texture(screenTexture, texCoord) + (texture(lightTexture, texCoord) * 0.001);
	//outputColor = texture(lightTexture, texCoord) + (texture(screenTexture, texCoord) * 0.001);
}