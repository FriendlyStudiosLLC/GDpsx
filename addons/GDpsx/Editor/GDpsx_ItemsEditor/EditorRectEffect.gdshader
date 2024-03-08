shader_type canvas_item;
render_mode skip_vertex_transform;

uniform sampler2D iChannel0;
uniform sampler2D iChannel1;
uniform float iTime;
uniform float scale_modifier = 1.0;

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
    vec3 trailColor = vec3(0.0);
    for (float k = 0.0; k < 8.0; k++) {
        float phase = k / 8.0 * 2.0 * pi;
        float DIRECTION = time * 0.2 + phase;
        float DISTANCE = 0.3;
        vec3 POSITION = vec3(dirDist(DIRECTION, DISTANCE), 0.0);
        POSITION = rY(POSITION, time * 0.5);
        POSITION = rZ(POSITION, time * 0.3);
        POSITION = rX(POSITION, time * 0.4);
        float circleStrength = 1.0 - smoothstep(0.0, 0.07, length(uv - POSITION.xy));
        trailColor += circleStrength * vec3(0.0, 0.0, 1.0) * (0.7 + 0.3 * cos(time - k * 0.5));
    }
    circles = clamp(trailColor.x + trailColor.y + trailColor.z, 0.0, 1.0);
    return vec3(circles);
}

vec3 trail_animation(vec2 uv, float time) {
    float circles = 0.0;
    vec3 trailColor = vec3(0.0);
    for (float k = 0.0; k < 16.0; k++) {
        float phase = k / 16.0 * 2.0 * pi;
        float DIRECTION = time * 0.2 + phase;
        float DISTANCE = 0.3;
        vec3 POSITION = vec3(dirDist(DIRECTION, DISTANCE), 0.0);
        POSITION = rY(POSITION, time * 0.5);
        POSITION = rZ(POSITION, time * 0.3);
        POSITION = rX(POSITION, time * 0.4);
        float circleStrength = 1.0 - smoothstep(0.0, 0.07, length(uv - POSITION.xy));
        trailColor += circleStrength * vec3(0.0, 0.0, 1.0) * (0.7 + 0.3 * cos(time- k * 0.5));
    }
    circles = clamp(trailColor.x + trailColor.y + trailColor.z, 0.0, 1.0);
    return vec3(circles);
}

vec2 circle(float a) {
    return vec2(cos(a), sin(a));
}

void vertex() {

    VERTEX = (MODEL_MATRIX * vec4(VERTEX, 0.0, 1.0)).xy;
}

void fragment() 
{
vec2 suv = (UV - vec2(0.5, 0.5)) / scale_modifier;
    suv.x *= SCREEN_PIXEL_SIZE.x / SCREEN_PIXEL_SIZE.y;
	
    suv.y *= SCREEN_PIXEL_SIZE.x / SCREEN_PIXEL_SIZE.y;
    float time = iTime * TIME; 
	
vec3 drawing = animation(suv, time);
drawing = pow(drawing, vec3(2.5, 1.8, 1.0));
vec3 traildrawing1 = animation(suv, time-0.25);
traildrawing1 = pow(traildrawing1, vec3(2.5, 1.8, 1.0));
vec3 traildrawing2 = animation(suv, time-0.5);
traildrawing2 = pow(traildrawing2, vec3(2.5, 1.8, 1.0));
vec3 traildrawing3 = animation(suv, time-0.75);
traildrawing3 = pow(traildrawing3, vec3(2.5, 1.8, 1.0));
vec3 traildrawing4 = animation(suv, time-.125);
traildrawing4 = pow(traildrawing4, vec3(2.5, 1.8, 1.0));
vec3 newDraw = drawing+(traildrawing1*.5)+(traildrawing2*.5)+(traildrawing3*.5)+(traildrawing4*.5);

// Texture manipulation from Buffer B
vec4 d = vec4(0.0);
for(float i = 0.0; i < 8.0; i++) {
    vec2 p = circle((i / 8.0) * pi * 2.0);
    p.x *= (1.0/suv).x / (1.0/suv).y;
    d = max(d, texture(iChannel1, suv + (p * 0.00015)));
}
vec4 texColor = pow(texture(iChannel0, suv), vec4(10.0)) + (clamp(d, 0.0, 1.0) * 0.95);

// Image logic
vec4 finalColor = texture(iChannel0, suv) + (texture(iChannel1, suv) * 1.0) + vec4(clamp(1.0 - ((length(suv) - 0.2) + 0.2) * 2.0, 0.0, 1.0) * vec3(0.1, 0.12, 0.3), 0.0);

// Combining effects (simplified version for demonstration)
COLOR = vec4(newDraw, 1.0) * texColor * finalColor;
}