#version 330 core

layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform vec2 trans;
uniform vec2 scale;
uniform float rotation;

void main() 
{
	texCoord = aTexCoord;

	float xScale = aPos.x * scale.x;
	float yScale = aPos.y * scale.y;

	float xRot = (xScale * cos(rotation)) - (yScale * sin(rotation));
	float yRot = (yScale * cos(rotation)) + (xScale * sin(rotation));

	gl_Position = vec4(xRot + trans.x, yRot + trans.y, 0.0, 1.0);
}