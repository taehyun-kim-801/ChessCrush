#include "ScreenPositionHelper.h"

void ScreenPositionHelper::Initialize() {
	auto screenSize = Director::getInstance()->getWinSize();
	
	TopLeft = Point(0, screenSize.height);
	TopCenter = Point(screenSize.width / 2, screenSize.height);
	TopRight = Point(screenSize.width, screenSize.height);
	MiddleLeft = Point(0, screenSize.height / 2);
	MiddleCenter = Point(screenSize.width / 2, screenSize.height / 2);
	MiddleRight = Point(screenSize.width, screenSize.height / 2);
	BottomLeft = Point::ZERO;
	BottomCenter = Point(screenSize.width / 2, 0);
	BottomRight = Point(screenSize.width, 0);
}