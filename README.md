# Roguelike

Documentation (Russian):

https://docs.google.com/document/d/1gGd7HVuZDebvwOwDosIV9V6NMwdhsqI7JzbrhdflZwk/edit?usp=sharing


Project architecture:

https://drive.google.com/file/d/1pjahMQ__q2c_2XQI5jrehokMEN3w2RMF/view


Prerequisites:
```
.NET Core 3.1
```

Build for Linux:
```
xbuild Roguelike.sln
```

Build for Windows:

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
[<path>] - path to the folder with the map
```


Примеры карт и их формат можно найти в RoguelikeTest/test_maps/