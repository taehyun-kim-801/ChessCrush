#include "AnchorPointHelper.h"

Point* AnchorPointHelper::TopLeft = nullptr;
Point* AnchorPointHelper::TopCenter = nullptr;
Point* AnchorPointHelper::TopRight = nullptr;
Point* AnchorPointHelper::MiddleLeft = nullptr;
Point* AnchorPointHelper::MiddleCenter = nullptr;
Point* AnchorPointHelper::MiddleRight = nullptr;
Point* AnchorPointHelper::BottomLeft = nullptr;
Point* AnchorPointHelper::BottomCenter = nullptr;
Point* AnchorPointHelper::BottomRight = nullptr;

void AnchorPointHelper::Initialize() {
	TopLeft = new Point(0, 1);
	TopCenter = new Point(0.5f, 1);
	TopRight = new Point(1, 1);
	MiddleLeft = new Point(0, 0.5f);
	MiddleCenter = new Point(0.5f, 0.5f);
	MiddleRight = new Point(1, 0.5f);
	BottomLeft = new Point(0, 0);
	BottomCenter = new Point(0.5f, 0);
	BottomRight = new Point(1, 0);
}