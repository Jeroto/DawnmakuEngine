﻿#version 330 core
out vec4 outputColor;

in vec2 texCoord;
in vec4 colorMod;


uniform sampler2D texture0;

void main()
{
    outputColor = texture(texture0, texCoord);
	outputColor.a = outputColor.r;
	outputColor *= colorMod;
}