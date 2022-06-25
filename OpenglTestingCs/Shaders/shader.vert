#version 330 core

//These lines specify the location and type of our attributes,
//the attributes here are prefixed with a "v" as they are our inputs to the vertex shader
//this isn't strictly necessary though, but a good habit.
layout (location = 0) in vec3 vPos;
layout (location = 1) in vec4 vColor;

//This is how we declare a uniform, they can be used in all our shaders and share the same name.
//This is prefixed with a u as it's our uniform.
//uniform vec3 uColor;

uniform mat4 uModel;
uniform mat4 uView;
uniform mat4 uProjection;

out vec4 fColor;

void main() {
	gl_Position = uProjection * uView * uModel * vec4(vPos, 1.0);

	//vec4 color = vColor;
	fColor = vColor;
}