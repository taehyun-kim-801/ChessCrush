#include "AnchorPointHelper.h"

Point AnchorPointHelper::TopLeft = Point::ZERO;
Point AnchorPointHelper::TopCenter = Point::ZERO;
Point AnchorPointHelper::TopRight = Point::ZERO;
Point AnchorPointHelper::MiddleLeft = Point::ZERO;
Point AnchorPointHelper::MiddleCenter = Point::ZERO;
Point AnchorPointHelper::MiddleRight = Point::ZERO;
Point AnchorPointHelper::BottomLeft = Point::ZERO;
Point AnchorPointHelper::BottomCenter = Point::ZERO;
Point AnchorPointHelper::BottomRight = Point::ZERO;

void AnchorPointHelper::Initialize() {
	TopLeft = Point(0, 1);
	TopCenter = Point(0.5f, 1);
	TopRight = Point(1, 1);
	MiddleLeft = Point(0, 0.5f);
	MiddleCenter = Point(0.5f, 0.5f);
	MiddleRight = Point(1, 0.5f);
	BottomLeft = Point(0, 0);
	BottomCenter = Point(0.5f, 0);
	BottomRight = Point(1, 0);
}