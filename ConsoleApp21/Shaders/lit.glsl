#version 410 core

struct Light
{
	vec3 position;
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct Material 
{
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
	float shininess;
};

out vec4 FragColor;

in vec3 Position;
in vec3 Normal;

uniform Light light;
uniform Material material;

uniform vec3 viewPos;

void main()
{
	// ambient
	vec3 ambient = light.ambient * material.ambient;
	
	// diffuse
	vec3 normal = normalize(Normal);
	vec3 lightDirection = normalize(light.position - Position);

	float diffuseBrightness = max(dot(normal, lightDirection), 0);
	vec3 diffuse = light.diffuse * (diffuseBrightness * material.diffuse);

	// specular
	vec3 viewDirection = normalize(viewPos - Position);
	vec3 reflectDirection = reflect(-lightDirection, normal);

	float specularBrightness = pow(max(dot(viewDirection, reflectDirection), 0), material.shininess * 128);
	vec3 specular = light.specular * (specularBrightness * material.specular);

	vec3 incomingLight = (ambient + diffuse + specular);
	FragColor = vec4(incomingLight, 1);
}