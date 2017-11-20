# youtube-dl-wrapper-exe
Very basic youtube-dl wrapper binary written in C#.NET
I got annoyed that you can not "cross compile" with pyinstaller from Linux to Windows.
Meaning I couldn't build youtube-dl.exe nightly.

So I made this small exe file that just relays stuff from the combined youtube-dl python script,
which is created by running `make` in the youtube-dl git repo.

This file can be thrown into any directory that's PATH'd in Windows, make sure youtube-dl(.py) is in the same directory.
This also requires Python3.5/3.6 to be installed.

It currently just checks for these paths:
```bash
HKLM\SOFTWARE\Python\PythonCore\{3.6,3.5}\InstallPath
HKCU\SOFTWARE\Python\PythonCore\{3.6,3.5}\InstallPath
C:\Python{36,35}\python.exe
```
```cmd
>youtube-dl.exe --version
2017.11.15
```