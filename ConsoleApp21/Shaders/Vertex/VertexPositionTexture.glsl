﻿#version 410 core

layout (location = 0) in vec3 vertexPosition;
layout (location = 1) in vec2 vertexTexCoord;

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

out vec2 texCoord;

void main()
{
	gl_Position = proj * view * model * vec4(vertexPosition, 1);
	texCoord = vertexTexCoord;
}