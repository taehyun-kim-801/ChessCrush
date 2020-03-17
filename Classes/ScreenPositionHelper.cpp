#include "ScreenPositionHelper.h"

Point* ScreenPositionHelper::TopLeft = nullptr;
Point* ScreenPositionHelper::TopCenter = nullptr;
Point* ScreenPositionHelper::TopRight = nullptr;
Point* ScreenPositionHelper::MiddleLeft = nullptr;
Point* ScreenPositionHelper::MiddleCenter = nullptr;
Point* ScreenPositionHelper::MiddleRight = nullptr;
Point* ScreenPositionHelper::BottomLeft = nullptr;
Point* ScreenPositionHelper::BottomCenter = nullptr;
Point* ScreenPositionHelper::BottomRight = nullptr;

void ScreenPositionHelper::Initialize() {
	const auto screenSize = Director::getInstance()->getWinSize();
	
	TopLeft = new Point(0, screenSize.height);
	TopCenter = new Point(screenSize.width / 2, screenSize.height);
	TopRight = new Point(screenSize.width, screenSize.height);
	MiddleLeft = new Point(0, screenSize.height / 2);
	MiddleCenter = new Point(screenSize.width / 2, screenSize.height / 2);
	MiddleRight = new Point(screenSize.width, screenSize.height / 2);
	BottomLeft = new Point(0, 0);
	BottomCenter = new Point(screenSize.width / 2, 0);
	BottomRight = new Point(screenSize.width, 0);
}