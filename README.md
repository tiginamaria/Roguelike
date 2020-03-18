# Roguelike

Архитектурная документация:
https://docs.google.com/document/d/1gGd7HVuZDebvwOwDosIV9V6NMwdhsqI7JzbrhdflZwk/edit?usp=sharing

Запуск под Linux:

xbuild Roguelike.sln [-random]

Запуск под Windows:

xbuild Roguelike.sln [-random]

msbuild Roguelike.sln /t:Rebuild /p:Configuration=Release /p:Platform="any cpu"
