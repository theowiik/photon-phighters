shader_type canvas_item;

uniform int levels : hint_range(1, 16);
uniform float threshold : hint_range(0.0, 1.0);
uniform int colors : hint_range(1, 32);
uniform int pixelate : hint_range(1, 10);
uniform sampler2D dither_texture : filter_nearest;

float dither(float raw, float dither, int depth) {
	
	float div = 1.0 / float(depth);
	float val = 0.0;
	
	for (int i = 0; i < depth; i++) {
		if (raw <= div * (float(i + 1))) {
			if ((raw * float(depth)) - float(i) <= dither * 0.999) {
				val = div * float(i);
			} else {
				val = div * float(i + 1);
			}
			break;
		}
	}
	if (raw >= 1.0) {
		val = 1.0;
	}
	return val;
}

void fragment()
{
	// Pixelate the screen
	vec2 screen_size = vec2(textureSize(TEXTURE, 0)) / float(pixelate);
	vec2 screen_sample_uv = floor(UV * screen_size) / screen_size;

	// Map the dither texture onto the screen pixels with tiling and repeating
	vec2 dither_size = vec2(textureSize(dither_texture, 0));
	vec2 dither_uv = mod(UV * screen_size, dither_size) / dither_size;
	
	// Sample the screen pixel
	vec3 screen_col = texture(TEXTURE, screen_sample_uv).rgb;

	// Sample the corresponding dither pixel luminance
	float dither_luminance = texture(dither_texture, dither_uv).r;

	// Dither each channel (r, g, and b) to the number of color quantizations via dither value
	vec3 quantized_col;
    for (int i = 0; i < 3; i++) {
        float raw_channel = screen_col[i];
        float dither_amount = threshold * dither_luminance;
        float quantized_channel = dither(raw_channel, dither_amount, levels);
        quantized_col[i] = floor(quantized_channel * float(colors)) / float(colors - 1);
    }
	
	// Apply the dithered color
	COLOR.rgb = quantized_col;
}