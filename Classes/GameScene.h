#pragma once
#include "cocos2d.h"
USING_NS_CC;

class GameScene :public Scene {
public:
	static Scene* createScene();
	
	virtual bool init();

	CREATE_FUNC(GameScene);

private:
	const Size ChessBoardContentSize = Size(500, 500);
	Sprite* chessBoardSprite;
};