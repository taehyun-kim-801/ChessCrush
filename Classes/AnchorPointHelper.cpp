#include "AnchorPointHelper.h"

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