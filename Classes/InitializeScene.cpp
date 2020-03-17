#include "InitializeScene.h"
#include "AnchorPointHelper.h"
#include "ScreenPositionHelper.h"
#include "GameScene.h"

Scene* InitializeScene::createScene() {
	return InitializeScene::create();
}

bool InitializeScene::init() {
	if (!Scene::init()) return false;

	auto initCallback = CallFunc::create(CC_CALLBACK_0(InitializeScene::GameInitialize, this));
	auto nextSceneCallback = CallFunc::create(CC_CALLBACK_0(InitializeScene::LoadNextScene, this));
	auto seq = Sequence::create(initCallback, nextSceneCallback, nullptr);
	runAction(seq);

	return true;
}

void InitializeScene::GameInitialize() {
	ScreenPositionHelper::Initialize();
	AnchorPointHelper::Initialize();
}

void InitializeScene::LoadNextScene() {
	Director::getInstance()->replaceScene(GameScene::createScene());
}