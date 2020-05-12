# Roguelike

Архитектурная документация:
https://docs.google.com/document/d/1gGd7HVuZDebvwOwDosIV9V6NMwdhsqI7JzbrhdflZwk/edit?usp=sharing

Пререквизиты:
```
.NET Core 3.1
```

Сборка под Linux:
```
xbuild Roguelike.sln
```

Сборка под Windows:

```
msbuild Roguelike.sln
```

## Запуск сервера: 
`Roguelike/bin/Debug/Roguelike.exe --server`

## Запуск клиента: 
`Roguelike/bin/Debug/Roguelike.exe --client`
Далее потребуется указать имя пользователя и id сессии.

## Запуск локальной игры: 
`Roguelike/bin/Debug/Roguelike.exe`

## Запуск локальной игры с восстановлением прошлой игры:
`Roguelike/bin/Debug/Roguelike.exe --load`

## Запуск локальной игры с загрузкой карты из файла:
```
Roguelike/bin/Debug/Roguelike.exe [<path>]
[<path>] - путь до файла с картой
```


Примеры карт и их формат можно найти в RoguelikeTest/test_maps/
