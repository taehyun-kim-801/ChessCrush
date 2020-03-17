#include "AnchorPointHelper.h"

Point AnchorPointHelper::TopLeft = nullptr;
Point AnchorPointHelper::TopCenter = nullptr;
Point AnchorPointHelper::TopRight = nullptr;
Point AnchorPointHelper::MiddleLeft = nullptr;
Point AnchorPointHelper::MiddleCenter = nullptr;
Point AnchorPointHelper::MiddleRight = nullptr;
Point AnchorPointHelper::BottomLeft = nullptr;
Point AnchorPointHelper::BottomCenter = nullptr;
Point AnchorPointHelper::BottomRight = nullptr;

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