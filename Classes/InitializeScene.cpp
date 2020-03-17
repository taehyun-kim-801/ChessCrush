#include "InitializeScene.h"
#include "AnchorPointHelper.h"
#include "ScreenPositionHelper.h"
#include "GameScene.h"

Scene* InitializeScene::createScene() {
	return InitializeScene::create();
}

bool InitializeScene::init() {
	if (!Scene::init()) return false;

	ScreenPositionHelper::Initialize();
	AnchorPointHelper::Initialize();

	auto gameScene = GameScene::createScene();
	Director::getInstance()->pushScene(gameScene);
	return true;
}