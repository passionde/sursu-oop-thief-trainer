#include "pch.h"
#include "Engine.h"

// <--- Memory Functions --->
HANDLE GetProcessHandle(DWORD processId)
{
    HANDLE hProcess = OpenProcess(PROCESS_ALL_ACCESS, FALSE, processId);
    if (hProcess == NULL)
    {
        _tprintf(_T("OpenProcess failed (%d).\n"), GetLastError());
    }
    return hProcess;
}

int ReadIntInMemory(HANDLE hProcess, DWORD address)
{
    int buffer = 0;
    SIZE_T bytesRead;

    if (ReadProcessMemory(hProcess, (LPCVOID)address, &buffer, sizeof(int), &bytesRead) == 0 || bytesRead != sizeof(int))
    {
        _tprintf(_T("ReadProcessMemory failed (%d).\n"), GetLastError());
        return 0;
    }
    return buffer;
}

BOOL WriteBytesToMemory(HANDLE hProcess, DWORD address, const BYTE* data, SIZE_T size)
{
    SIZE_T bytesWritten;
    if (!WriteProcessMemory(hProcess, (LPVOID)address, data, size, &bytesWritten) || bytesWritten != size)
    {
        _tprintf(_T("WriteProcessMemory failed (%d).\n"), GetLastError());
        return FALSE;
    }
    return TRUE;
}


// <--- Engine Class --->
Engine::Engine(DWORD pid, DWORD baseAddress, Setting* settings, int size)
{
    _processId = pid;
    _baseAddress = baseAddress;

    if (_processId == 0 || _baseAddress == 0) { return; }

    _hProcess = GetProcessHandle(_processId);

    for (int i = 0; i < size; ++i)
    {
        uint32_t parameter = settings[i].Parameter;
        DWORD offset = settings[i].Offset;

        if (parameter == AMMO_TYPE) {
            _ammoOffset = offset;
        }
        else if (parameter == MONEY_TYPE)
        {
            _moneyOffset = offset;
        }
    }
}

Engine::~Engine() {
    CloseHandle(_hProcess);
}

BOOL Engine::HackAmmo()
{
    //                                                   
    DWORD funcAddres = _baseAddress + _ammoOffset;
    BYTE dataToWrite[] = { 0x90, 0x90 };
    return WriteBytesToMemory(_hProcess, funcAddres, dataToWrite, sizeof(dataToWrite));
}

BOOL Engine::RestoreAmmo()
{
    //                                                   
    DWORD funcAddres = _baseAddress + _ammoOffset;
    BYTE dataToWrite[] = { 0x29, 0x08 };
    return WriteBytesToMemory(_hProcess, funcAddres, dataToWrite, sizeof(dataToWrite));
}

BOOL Engine::HackMoney()
{
    //                               
    DWORD funcAddres = _baseAddress + _moneyOffset;
    BYTE dataToWrite[] = { 0x90, 0x90, 0x90, 0x90, 0x90, 0x90 };
    return WriteBytesToMemory(_hProcess, funcAddres, dataToWrite, sizeof(dataToWrite));
}

BOOL Engine::RestoreMoney()
{
    //                               
    DWORD funcAddres = _baseAddress + _moneyOffset;
    BYTE dataToWrite[] = { 0x29, 0x88, 0x74, 0x04, 0x00, 0x00 };
    return WriteBytesToMemory(_hProcess, funcAddres, dataToWrite, sizeof(dataToWrite));
}

Engine* createEngine(DWORD pid, DWORD baseAddress, Setting* settings, int size)
{
    return new Engine(pid, baseAddress, settings, size);
}


// <--- DLL Wraper --->
bool destroyEngine(Engine* engine) {
    delete engine;
    return true;
}

bool hackAmmo(Engine* engine)
{
    return engine->HackAmmo();
}

bool restoreAmmo(Engine* engine)
{
    return engine->RestoreAmmo();
}

bool hackMoney(Engine* engine)
{
    return engine->HackMoney();
}

bool restoreMoney(Engine* engine)
{
    return engine->RestoreMoney();
}