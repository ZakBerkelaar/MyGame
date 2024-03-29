﻿#version 330 core

layout(location = 0) in vec3 aPos;
layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform vec2 pos;

void main() 
{
	texCoord = aTexCoord;
	
	gl_Position = vec4(aPos.x + pos.x, aPos.y + pos.y, aPos.z, 1.0);
}