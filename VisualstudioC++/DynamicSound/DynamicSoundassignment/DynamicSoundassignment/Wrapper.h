#pragma once
#include "LibSettings.h"
#include "Sound Manager.h"
#include "SoundStorage.h"




#ifdef __cplusplus
extern "C"
{
#endif

	LIB_API bool Init();
	LIB_API void CleanUp();
	LIB_API void initializeSound();
	LIB_API void loadSound(int key);
	LIB_API void playSound();
#ifdef  __cplusplus
};

#endif
