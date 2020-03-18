# Roguelike

Архитектурная документация:
https://docs.google.com/document/d/1gGd7HVuZDebvwOwDosIV9V6NMwdhsqI7JzbrhdflZwk/edit?usp=sharing

Сборка под Linux:
```
xbuild Roguelike.sln
```

Сборка под Windows:
```
msbuild Roguelike.sln /t:Rebuild /p:Configuration=Release /p:Platform="any cpu"
```
