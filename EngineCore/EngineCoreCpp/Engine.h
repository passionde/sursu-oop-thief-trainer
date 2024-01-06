#pragma once

#include <windows.h>
#include <tchar.h>
#include <psapi.h>
#include <iostream>
#include <TlHelp32.h>

#pragma pack(push, 1)
struct Setting
{
    uint32_t Parameter;
    DWORD Offset;
};
#pragma pack(pop)

#define AMMO_TYPE 1
#define MONEY_TYPE 2

class Engine
{
private:
    DWORD _processId;
    DWORD _baseAddress;
    HANDLE _hProcess;

    DWORD _ammoOffset = 0x21AAEA;
    DWORD _moneyOffset = 0x269FF3;
public:
    Engine(DWORD pid, DWORD baseAddress, Setting* settings, int size);
    ~Engine();
    BOOL HackAmmo();
    BOOL RestoreAmmo();
    BOOL HackMoney();
    BOOL RestoreMoney();
};


extern "C" __declspec(dllexport) Engine * createEngine(DWORD pid, DWORD baseAddress, Setting * settings, int size);
extern "C" __declspec(dllexport) bool destroyEngine(Engine * engine);
extern "C" __declspec(dllexport) bool hackAmmo(Engine * engine);
extern "C" __declspec(dllexport) bool restoreAmmo(Engine * engine);
extern "C" __declspec(dllexport) bool hackMoney(Engine * engine);
extern "C" __declspec(dllexport) bool restoreMoney(Engine * engine);