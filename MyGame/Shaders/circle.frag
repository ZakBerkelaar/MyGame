#version 330

out vec4 outputColor;

in vec2 texCoord;

//TODO: Could be faster
uniform vec4 color;
uniform float radius;
uniform vec2 position;

float map(float value, float min1, float max1, float min2, float max2);

void main()
{
	//outputColor = vec4(1.0, 0.0, 1.0, 1.0);
//	if(distance(vec2(0.5, 0.5), texCoord) > 0.25) 
//		discard;
	//outputColor = vec4(1.0, 1.0, 1.0, 1.0);
	//float dist = 1 - clamp(distance(vec2(0.5, 0.5), texCoord) * 3, 0.0, 1.0);
	float dist = distance(position, texCoord);
	if(dist > radius) discard;
	float dist2 = 1 - map(dist, 0.0, radius, 0, 1);
	outputColor = mix(color, vec4(dist2, dist2, dist2, 1.0), dist2);
	//outputColor = texture2D(dither0, texCoord * 4);
}

float map(float value, float min1, float max1, float min2, float max2) {
  return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
}