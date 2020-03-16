#pragma once
#include "cocos2d.h"
USING_NS_CC;

class AnchorPointHelper {
public:
	static Point& TopLeft;
	static Point& TopCenter;
	static Point& TopRight;
	static Point& MiddleLeft;
	static Point& MiddleCenter;
	static Point& MiddleRight;
	static Point& MiddleRight;
	static Point& BottomLeft;
	static Point& BottomCenter;
	static Point& BottomRight;

	static void Initialize();
};