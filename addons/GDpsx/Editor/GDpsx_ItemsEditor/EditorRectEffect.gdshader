shader_type canvas_item;

uniform sampler2D iChannel0;
uniform sampler2D iChannel1;
uniform float iTime;

const float pi = 3.14159265358979323;

vec3 rX(vec3 p, float a) {
    float c = cos(a);
    float s = sin(a);
    return vec3(p.x, c * p.y - s * p.z, s * p.y + c * p.z);
}

vec3 rY(vec3 p, float a) {
    float c = cos(a);
    float s = sin(a);
    return vec3(c * p.x + s * p.z, p.y, -s * p.x + c * p.z);
}

vec3 rZ(vec3 p, float a) {
    float c = cos(a);
    float s = sin(a);
    return vec3(c * p.x - s * p.y, s * p.x + c * p.y, p.z);
}

vec2 dirDist(float dir, float dist) {
    return vec2(cos(dir) * dist, sin(dir) * dist);
}

vec3 animation(vec2 uv, float time) {
    float circles = 0.0;
    for (float k = 0.0; k < 8.0; k++) {
        float DIRECTION = time * k * 0.1;
        float DISTANCE = 0.2;
        vec3 POSITION = vec3(dirDist(DIRECTION, DISTANCE), 0.0);
        POSITION = rY(POSITION, time * 1.1);
        POSITION = rZ(POSITION, time * 2.15);
        POSITION = rX(POSITION, time * 0.52);
        circles = max(circles, clamp(1.0 - (length(uv - POSITION.xy) * 40.0), 0.0, 1.0));
    }
    return vec3(circles);
}

vec2 circle(float a) {
    return vec2(cos(a), sin(a));
}

void fragment() 
{
vec2 uv = FRAGCOORD.xy / (1.0/SCREEN_PIXEL_SIZE);
vec2 suv = uv - vec2(0.5, 0.5);
suv.x *= (1.0/UV).x / (1.0/UV).y;
float time = iTime * TIME;
// Animation from Buffer A
vec3 drawing = animation(suv, time);
drawing = pow(drawing, vec3(2.5, 1.8, 1.0));

// Texture manipulation from Buffer B
vec4 d = vec4(0.0);
for(float i = 0.0; i < 8.0; i++) {
    vec2 p = circle((i / 8.0) * pi * 2.0);
    p.x *= (1.0/UV).x / (1.0/UV).y;
    d = max(d, texture(iChannel1, uv + (p * 0.00015)));
}
vec4 texColor = pow(texture(iChannel0, uv), vec4(10.0)) + (clamp(d, 0.0, 1.0) * 0.95);

// Image logic
vec4 finalColor = texture(iChannel0, uv) + (texture(iChannel1, uv) * 1.0) + vec4(clamp(1.0 - ((length(suv) - 0.2) + 0.2) * 2.0, 0.0, 1.0) * vec3(0.1, 0.12, 0.3), 0.0);

// Combining effects (simplified version for demonstration)
COLOR = vec4(drawing, 1.0) * texColor * finalColor;
}