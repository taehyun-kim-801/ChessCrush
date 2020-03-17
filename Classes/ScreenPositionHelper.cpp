#include "ScreenPositionHelper.h"

Point ScreenPositionHelper::TopLeft = Point::ZERO;
Point ScreenPositionHelper::TopCenter = Point::ZERO;
Point ScreenPositionHelper::TopRight = Point::ZERO;
Point ScreenPositionHelper::MiddleLeft = Point::ZERO;
Point ScreenPositionHelper::MiddleCenter = Point::ZERO;
Point ScreenPositionHelper::MiddleRight = Point::ZERO;
Point ScreenPositionHelper::BottomLeft = Point::ZERO;
Point ScreenPositionHelper::BottomCenter = Point::ZERO;
Point ScreenPositionHelper::BottomRight = Point::ZERO;

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