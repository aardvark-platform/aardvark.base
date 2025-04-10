@echo off
IF EXIST build RMDIR /S /Q build
cmake -DCMAKE_BUILD_TYPE=Release -S . -B build
cmake --build build --config Release
cmake --install build --config Release