#include "ScreenPositionHelper.h"

Point ScreenPositionHelper::TopLeft = nullptr;
Point ScreenPositionHelper::TopCenter = nullptr;
Point ScreenPositionHelper::TopRight = nullptr;
Point ScreenPositionHelper::MiddleLeft = nullptr;
Point ScreenPositionHelper::MiddleCenter = nullptr;
Point ScreenPositionHelper::MiddleRight = nullptr;
Point ScreenPositionHelper::BottomLeft = nullptr;
Point ScreenPositionHelper::BottomCenter = nullptr;
Point ScreenPositionHelper::BottomRight = nullptr;

void ScreenPositionHelper::Initialize() {
	const auto screenSize = Director::getInstance()->getWinSize();
	
	TopLeft = Point(0, screenSize.height);
	TopCenter = Point(screenSize.width / 2, screenSize.height);
	TopRight = Point(screenSize.width, screenSize.height);
	MiddleLeft = Point(0, screenSize.height / 2);
	MiddleCenter = Point(screenSize.width / 2, screenSize.height / 2);
	MiddleRight = Point(screenSize.width, screenSize.height / 2);
	BottomLeft = Point(0, 0);
	BottomCenter = Point(screenSize.width / 2, 0);
	BottomRight = Point(screenSize.width, 0);
}