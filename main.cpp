#include "SDL.h"
#include <vector>

#define WINDOW_WIDTH 1600
#define WINDOW_HEIGHT 900
#define MAP_SIZE 24
#define TEX_WIDTH 64
#define TEX_HEIGHT 64

struct Player {
	double speed = 0.05f;
	double x, y,viewAngle;
	double FOV = M_PI / 3;
};

SDL_Window* window = SDL_CreateWindow("Raycasting", SDL_WINDOWPOS_CENTERED, SDL_WINDOWPOS_CENTERED, WINDOW_WIDTH, WINDOW_HEIGHT, SDL_WINDOW_SHOWN);
SDL_Renderer* renderer = SDL_CreateRenderer(window, -1, SDL_RENDERER_ACCELERATED);

bool windowShouldClose = false;
bool showMap = false;
int map[MAP_SIZE][MAP_SIZE] =
{
  {4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,4,7,7,7,7,7,7,7,7},
  {4,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,7},
  {4,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7},
  {4,0,2,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7},
  {4,0,3,0,0,0,0,0,0,0,0,0,0,0,0,0,7,0,0,0,0,0,0,7},
  {4,0,4,0,0,0,0,5,5,5,5,5,5,5,5,5,7,7,0,7,7,7,7,7},
  {4,0,5,0,0,0,0,5,0,5,0,5,0,5,0,5,7,0,0,0,7,7,7,1},
  {4,0,6,0,0,0,0,5,0,0,0,0,0,0,0,5,7,0,0,0,0,0,0,8},
  {4,0,7,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,7,7,7,1},
  {4,0,8,0,0,0,0,5,0,0,0,0,0,0,0,5,7,0,0,0,0,0,0,8},
  {4,0,0,0,0,0,0,5,0,0,0,0,0,0,0,5,7,0,0,0,7,7,7,1},
  {4,0,0,0,0,0,0,5,5,5,5,0,5,5,5,5,7,7,7,7,7,7,7,1},
  {6,6,6,6,6,6,6,6,6,6,6,0,6,6,6,6,6,6,6,6,6,6,6,6},
  {8,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,4},
  {6,6,6,6,6,6,0,6,6,6,6,0,6,6,6,6,6,6,6,6,6,6,6,6},
  {4,4,4,4,4,4,0,4,4,4,6,0,6,2,2,2,2,2,2,2,3,3,3,3},
  {4,0,0,0,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,0,0,0,2},
  {4,0,0,0,0,0,0,0,0,0,0,0,6,2,0,0,5,0,0,2,0,0,0,2},
  {4,0,0,0,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,2,0,2,2},
  {4,0,6,0,6,0,0,0,0,4,6,0,0,0,0,0,5,0,0,0,0,0,0,2},
  {4,0,0,5,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,2,0,2,2},
  {4,0,6,0,6,0,0,0,0,4,6,0,6,2,0,0,5,0,0,2,0,0,0,2},
  {4,0,0,0,0,0,0,0,0,4,6,0,6,2,0,0,0,0,0,2,0,0,0,2},
  {4,4,4,4,4,4,4,4,4,4,1,1,1,2,2,2,2,2,2,3,3,3,3,3}
};
Player player;
int lastMouseX, lastMouseY;
Uint32 screenBuffer[WINDOW_WIDTH * WINDOW_HEIGHT];
Uint32 blank[WINDOW_WIDTH * WINDOW_HEIGHT];
SDL_Texture* frame = SDL_CreateTexture(renderer, SDL_PIXELFORMAT_RGB888, SDL_TEXTUREACCESS_STREAMING, WINDOW_WIDTH, WINDOW_HEIGHT);
Uint32 textures[8][TEX_WIDTH*TEX_HEIGHT];

void Input() {
	SDL_Event event;
	while (SDL_PollEvent(&event)) {
		if (event.type == SDL_QUIT)
			windowShouldClose = true;
		else if (event.type == SDL_MOUSEWHEEL) {
			if (event.wheel.y > 0)
				player.FOV += M_PI / 180*2;
			else player.FOV -= M_PI / 180*2;
		}
	}

	const Uint8* keyboardState = SDL_GetKeyboardState(NULL);

	double dirX = cos(player.viewAngle);
	double dirY = sin(player.viewAngle);

	if (keyboardState[SDL_SCANCODE_ESCAPE]) {
		SDL_Delay(100);
		if (SDL_GetRelativeMouseMode() == SDL_TRUE)
			SDL_SetRelativeMouseMode(SDL_FALSE);
		else SDL_SetRelativeMouseMode(SDL_TRUE);
	}
	else if (keyboardState[SDL_SCANCODE_M]) {
		SDL_Delay(100);
		showMap = !showMap;
	}
	if (keyboardState[SDL_SCANCODE_W] && map[(int)(player.y + dirY * player.speed)][(int)(player.x + dirX * player.speed)]==0) {
		player.x += dirX * player.speed;
		player.y += dirY * player.speed;
	}
	if (keyboardState[SDL_SCANCODE_S] && map[(int)(player.y - dirY * player.speed)][(int)(player.x - dirX * player.speed)] == 0) {
		player.x -= dirX * player.speed;
		player.y -= dirY * player.speed;
	}
	if (keyboardState[SDL_SCANCODE_D] && map[(int)(player.y + sin(player.viewAngle + M_PI / 2) * player.speed)][(int)(player.x + cos(player.viewAngle + M_PI / 2) * player.speed)] == 0) {
		player.x += cos(player.viewAngle + M_PI / 2) * player.speed;
		player.y += sin(player.viewAngle + M_PI / 2) * player.speed;
	}
	if (keyboardState[SDL_SCANCODE_A] && map[(int)(player.y - sin(player.viewAngle + M_PI / 2) * player.speed)][(int)(player.x - cos(player.viewAngle + M_PI / 2) * player.speed)] == 0){
		player.x -= cos(player.viewAngle + M_PI / 2) * player.speed;
		player.y -= sin(player.viewAngle + M_PI / 2) * player.speed;
	}
	int mouseX, mouseY;
	SDL_GetMouseState(&mouseX, &mouseY);
	player.viewAngle += (mouseX - lastMouseX)*M_PI / 180/10;

	if (lastMouseX == WINDOW_WIDTH - 1 && SDL_GetRelativeMouseMode() == SDL_TRUE) {
		SDL_WarpMouseInWindow(window, 0, mouseY);
		lastMouseX = 1;
	}
	else if (lastMouseX == 0 && SDL_GetRelativeMouseMode() == SDL_TRUE) {
		SDL_WarpMouseInWindow(window, WINDOW_WIDTH - 1, mouseY);
		lastMouseX = WINDOW_WIDTH - 2;
	}
	else lastMouseX = mouseX;
	lastMouseY = mouseY;
}
double RayLength(int& side, double&rayAngle,bool &hit,int& blockType) {

	double dirX = cos(rayAngle);
	double dirY = sin(rayAngle);

	double xDeltaDist = abs(1 / dirX);
	double yDeltaDist = abs(1 / dirY);

	double xRayDist;
	double yRayDist;

	int mapX = (int)player.x, mapY = (int)player.y;
	int stepX, stepY;

	if (dirX > 0) {
		stepX = 1;
		xRayDist = (mapX + 1.0 - player.x) * xDeltaDist;
	}
	else {
		stepX = -1;
		xRayDist = (player.x - mapX) * xDeltaDist;
	}	
	if (dirY > 0) {
		stepY = 1;
		yRayDist = (mapY + 1.0 - player.y) * yDeltaDist;
	}
	else {
		stepY = -1;
		yRayDist = (player.y - mapY) * yDeltaDist;
	}

	double rayDistance = 0;

	while (!hit) {
		if (xRayDist < yRayDist) {
			xRayDist += xDeltaDist;
			rayDistance = xRayDist;
			mapX += stepX;
			side = 0;
		}
		else {
			yRayDist += yDeltaDist;
			rayDistance = yRayDist;
			mapY += stepY;
			side = 1;
		}
		if (mapY >= 0 && mapY < MAP_SIZE && mapX >= 0 && mapX < MAP_SIZE)
			hit = map[mapY][mapX] > 0;
	}
	blockType = map[mapY][mapX] - 1;

	if (side == 0) rayDistance -= xDeltaDist;
	else rayDistance -= yDeltaDist;

	return rayDistance;
}
void RenderMap(){

	SDL_SetRenderDrawColor(renderer, 0, 0, 0, 0);
	SDL_RenderClear(renderer);

	SDL_SetRenderDrawColor(renderer, 50, 50, 50, 255);
	const int cellSize = WINDOW_HEIGHT / MAP_SIZE;

	for (size_t x = 1; x < MAP_SIZE; x++)
		SDL_RenderDrawLine(renderer, x * cellSize, 0, x * cellSize, WINDOW_HEIGHT);
	for (size_t y = 1; y < MAP_SIZE; y++)
		SDL_RenderDrawLine(renderer, 0, y * cellSize, WINDOW_HEIGHT, y * cellSize);

	SDL_SetRenderDrawColor(renderer, 0, 0, 200, 255);
	for (size_t x = 0; x < MAP_SIZE; x++)
	{
		for (size_t y = 0; y < MAP_SIZE; y++)
		{
			if (map[y][x] > 0) {
				SDL_Rect rect{ x * cellSize,y * cellSize,cellSize,cellSize };
				SDL_RenderFillRect(renderer, &rect);
			}
		}
	}

	SDL_SetRenderDrawColor(renderer, 255, 0, 0, 255);
	SDL_RenderDrawPointF(renderer, player.x * cellSize, player.y * cellSize);


	for (double ray = player.viewAngle-player.FOV/2; ray < player.viewAngle+player.FOV/2; ray += M_PI/180)
	{
		int side;
		int blockType;
		bool hit = false;
		double rl = RayLength(side, ray, hit,blockType);

		if (side == 0) SDL_SetRenderDrawColor(renderer, 0, 255, 0, 255);
		else SDL_SetRenderDrawColor(renderer, 0, 100, 0, 255);

		double rayEndY = rl * cellSize * sin(ray) + player.y * cellSize;
		double rayEndX = rl * cellSize * cos(ray) + player.x * cellSize;

		SDL_RenderDrawLineF(renderer, player.x * cellSize, player.y * cellSize, rayEndX, rayEndY);
	}

	SDL_RenderPresent(renderer);
}
void FlatRender() {

	SDL_SetRenderDrawColor(renderer, 0, 0, 0, 0);
	SDL_RenderClear(renderer);

	int pixelX = 0;

	for (double ray = player.viewAngle - player.FOV / 2; ray < player.viewAngle + player.FOV / 2; ray += player.FOV/WINDOW_WIDTH)
	{
		int side;
		int blockType;
		bool hit = false;

		double rl = RayLength(side, ray, hit, blockType);// *cos(ray - player.viewAngle);

		if (hit) {

			double alpha = 255 - rl * 25;
			if (alpha < 100)alpha = 100;
			else if (alpha > 255)alpha = 255;

			if (side == 0)alpha *= 0.6f;

			SDL_SetRenderDrawColor(renderer, 50* (alpha/255), 50 * (alpha / 255), alpha, 255);

			double lineHeight = WINDOW_HEIGHT / rl;
			if (lineHeight > WINDOW_HEIGHT)lineHeight = WINDOW_HEIGHT;

			double lineStart = WINDOW_HEIGHT / 2 - lineHeight / 2;
			double lineEnd = WINDOW_HEIGHT / 2 + lineHeight / 2;

			SDL_RenderDrawLineF(renderer, pixelX, lineStart, pixelX, lineEnd);
		}
		pixelX++;
	}

	SDL_RenderPresent(renderer);
}
void TexturedRender() {

	memcpy(screenBuffer, blank, sizeof(screenBuffer));
	int pixelX = 0;

	for (double ray = player.viewAngle - player.FOV / 2.0; ray <= player.viewAngle + player.FOV / 2.0; ray += player.FOV / WINDOW_WIDTH)
	{
		int side;
		int blockType;
		bool hit = false;

		double rl = RayLength(side, ray, hit, blockType);

		double rayDirX = cos(ray);
		double rayDirY = sin(ray);

		double wallX;

		if (side == 0) wallX = rayDirY * rl + player.y;
		else wallX = rayDirX * rl + player.x;
		
		wallX -= (int)wallX;
		
		int texX = (double)TEX_WIDTH * wallX;
		if (side == 0 && rayDirX > 0) texX = TEX_WIDTH - texX - 1;
		if (side == 1 && rayDirY < 0) texX = TEX_WIDTH - texX - 1;

		double correctedLength = rl * cos((ray - player.viewAngle)/2.0);

		if (hit) {		
			double lineHeight = WINDOW_HEIGHT / correctedLength;
			double stepTexY = TEX_HEIGHT / lineHeight;
			double texOffset = 0;

			if (lineHeight > WINDOW_HEIGHT) {
				texOffset = (lineHeight - WINDOW_HEIGHT) / 2.0;
				lineHeight = WINDOW_HEIGHT;
			}

			double lineStart = WINDOW_HEIGHT / 2 - lineHeight / 2;
			if (lineStart < 0)lineStart = 0;
			double lineEnd = WINDOW_HEIGHT / 2 + lineHeight / 2;
			if (lineEnd >= WINDOW_HEIGHT) lineEnd = WINDOW_HEIGHT - 1;

			double texY = texOffset*stepTexY;
			
			for (int pixelY = lineStart; pixelY < lineEnd; pixelY++)
			{
				Uint32 color = textures[blockType][TEX_WIDTH * ((int)texY & (TEX_HEIGHT-1)) + texX];
				if (side == 1) color = (color >> 1) & 8355711;
				screenBuffer[pixelY * WINDOW_WIDTH + pixelX] = color;
				texY += stepTexY;
			}
		}
		pixelX++;
	}
	SDL_UpdateTexture(frame, NULL, screenBuffer, sizeof(Uint32) * WINDOW_WIDTH);
	SDL_RenderCopy(renderer, frame, NULL, NULL);
	SDL_RenderPresent(renderer);
}

int main(int argc, char** argv) {

	for (int x = 0; x < WINDOW_WIDTH; x++)
	{
		for (int y = 0; y < WINDOW_HEIGHT; y++)
		{
			blank[y * WINDOW_WIDTH + x] = 0x0000;
		}
	}

	for (int x = 0; x < TEX_WIDTH; x++)
		for (int y = 0; y < TEX_HEIGHT; y++)
		{
			int xorcolor = (x * 256 / TEX_WIDTH) ^ (y * 256 / TEX_HEIGHT);
			//int xcolor = x * 256 / texWidth;
			int ycolor = y * 256 / TEX_HEIGHT;
			int xycolor = y * 128 / TEX_HEIGHT + x * 128 / TEX_WIDTH;
			textures[0][TEX_WIDTH * y + x] = 65536 * 254 * (x != y && x != TEX_WIDTH - y); //flat red texture with black cross
			textures[1][TEX_WIDTH * y + x] = xycolor + 256 * xycolor + 65536 * xycolor; //sloped greyscale
			textures[2][TEX_WIDTH * y + x] = 256 * xycolor + 65536 * xycolor; //sloped yellow gradient
			textures[3][TEX_WIDTH * y + x] = xorcolor + 256 * xorcolor + 65536 * xorcolor; //xor greyscale
			textures[4][TEX_WIDTH * y + x] = 256 * xorcolor; //xor green
			textures[5][TEX_WIDTH * y + x] = 65536 * 192 * (x % 16 && y % 16); //red bricks
			textures[6][TEX_WIDTH * y + x] = 65536 * ycolor; //red gradient
			textures[7][TEX_WIDTH * y + x] = 128 + 256 * 128 + 65536 * 128; //flat grey texture
		}

	player.x = 1.5f;
	player.y = 1.5f;
	player.viewAngle = 0.0f;
	
	SDL_SetRelativeMouseMode(SDL_TRUE);
	SDL_WarpMouseInWindow(window, WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2);
	
	SDL_GetMouseState(&lastMouseX, &lastMouseY);
	
	const int frameDelay = 1000 / 75;
	Uint32 frameStart;
	int frameTime;
	
	while (!windowShouldClose) {
	
		frameStart = SDL_GetTicks();
	
		Input();
		if (showMap) RenderMap();
		else TexturedRender();
	
		frameTime = SDL_GetTicks() - frameStart;
	
		if (frameDelay > frameTime)
			SDL_Delay(frameDelay - frameTime);
	}

	return 0;
}