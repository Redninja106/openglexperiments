#version 410 core

struct Light
{
	vec4 vector;
	vec3 ambient;
	vec3 diffuse;
	vec3 specular;
};

struct Material 
{
	sampler2D diffuse;
	sampler2D specular;
	sampler2D emission;
	float shininess;
};

out vec4 FragColor;

in vec3 Position;
in vec3 Normal;
in vec2 TexCoords;

uniform Light light;
uniform Material material;
uniform float time;

uniform vec3 viewPos;

void main()
{
	vec3 color = vec3(texture(material.diffuse, TexCoords));

	// ambient
	vec3 ambient = light.ambient * color;
	
	// diffuse
	vec3 lightDirection;
	if (light.vector.w < 0.0001)
	{
		lightDirection = normalize(-vec3(light.vector));
	}
	else
	{
		lightDirection = normalize(vec3(light.vector) - Position);
	}

	vec3 normal = normalize(Normal);
	float diffuseBrightness = max(dot(normal, lightDirection), 0);
	vec3 diffuse = light.diffuse * color * diffuseBrightness;
	// specular
	vec3 viewDirection = normalize(viewPos - Position);
	vec3 reflectDirection = reflect(-lightDirection, normal);

	vec3 specularColor = vec3(texture(material.specular, TexCoords));
	float specularBrightness = pow(max(dot(viewDirection, reflectDirection), 0), material.shininess * 128);
	vec3 specular = light.specular * (specularBrightness * specularColor);
	
	// emission
	vec3 emission = vec3(0);
	if (specularColor == vec3(0))
	{
		emission = vec3(texture(material.emission, TexCoords + vec2(0,time)));
	}

	vec3 incomingLight = (emission + ambient + diffuse + specular);
	FragColor = vec4(incomingLight, 1);
}