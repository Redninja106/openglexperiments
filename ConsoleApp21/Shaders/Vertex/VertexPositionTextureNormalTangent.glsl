#version 410 core
layout (location = 0) in vec3 vertexPosition;
layout (location = 1) in vec2 vertexTexCoords;
layout (location = 2) in vec3 vertexNormal;
layout (location = 3) in vec3 vertexTangent;

out vec3 Position;
out vec2 TexCoords;
out vec3 TangentLightPos;
out vec3 TangentViewPos;
out vec3 TangentFragPos;

uniform mat4 model;
uniform mat4 view;
uniform mat4 proj;

uniform vec3 lightPos;
uniform vec3

void main()
{
	gl_Position = proj * view * model * vec4(vertexPosition, 1);
	Position = vec3(model * vec4(vertexPosition, 1));
	TexCoords = vertexTexCoords;
	
	mat3 normalMatrix = mat3(transpose(inverse(model)));
	
	vec3 normal = normalize(normalMatrix * vertexNormal);
	vec3 tangent = normalize(normalMatrix * vertexTangent);
	vec3 bitangent = cross(normal, tangent);
	
	mat3 tbn = transpose(mat3(tangent, bitangent, normal));
	TangentLightPos = tbn * lightPos;
}