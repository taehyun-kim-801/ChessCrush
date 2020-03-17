#include "GameScene.h"
#include "AnchorPointHelper.h"
#include "ScreenPositionHelper.h"

Scene* GameScene::createScene() {
	return GameScene::create();
}

bool GameScene::init() {
	if (!Scene::init()) return false;
	chessBoardSprite = Sprite::create("ChessBoard_1.png");
	chessBoardSprite->setAnchorPoint(AnchorPointHelper::MiddleCenter);
	chessBoardSprite->setPosition(ScreenPositionHelper::MiddleCenter);
	chessBoardSprite->setContentSize(ChessBoardContentSize);

	this->addChild(chessBoardSprite);
	return true;
}