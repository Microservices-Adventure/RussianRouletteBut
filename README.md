# Русская рулетка,
 но при каждом выстреле падает один из микросервисов.

## Содержание
- [Наша команда](#команда-microservices-adventure)
- [Tech Stack](#tech-stack)
- [Текущее состояние проекта](#текущее-состояние-проекта)
- [Запуск проекта](#запуск-проекта)
- [Описание сервисов](#описание-сервисов)

## Команда Microservices Adventure
1. Казанков Руслан - Team Lead, Fullstack
2. Спиваченко Михаил - Backend-Developer
3. Заголовец Иван - Тестировщик

## Tech Stack
- ASP NET - платформа разработки веб-приложений. ASP NET MVC для клиента. ASP NET API для бэкэнда.
- xUnit - фреймворк для тестирования.
- Postgres - база данных, которая будет использоваться в микросервисах.
- EntityFrameworkCore - ORM для базы данных.
- Kafka - брокер сообщений.
- Docker - для запуска микросервисного веб приложения.
  
## Текущее состояние проекта
- Frontend - создан базовый проект ASP NET MVC. Проект приведён к FSD архитектуре, но папки Properties и wwwroot остались в корне проекта из-за особенностей ASP NET приложения.
- XUnitTestProject - проект с тестами, который активно ведётся.
- AuthorizationAPI - 🟢 Готово!
- Revolver - 🟢 Готово!
- ActionLog - 🟢 Готово!
- LiveMonitor - 🟢 Готово!
- UserProfile - 🟢 Готово!
  
## Запуск проекта
1. Склонировать репозиторий на локальную машину.
2. Запустить проект из папки с файлом docker-compose.yml:
```
docker-compose up -d
```
3. Перейти по одному из адресов:
- https://localhost:8081
- http://localhost:8080

Во втором случае всё равно произойдёт перенаправление на https адрес.

## Описание сервисов
### **Frontend**
Frontend - ASP NET MVC веб-клиент приложения. Именно сюда попадают пользователи, когда заходят на наш сайт. Здесь весь пользовательский интерфейс и "ручки" через которые происходит обращение к бэкенду.
### **AuthorizationAPI**
AuthorizationAPI - ASP NET Web API приложение для авторизации пользователей.
### **ActionLog**
ActionLog - сервис для публичных логов. Когда пользователи регистрируются или стреляют из револьвера, то действия пользователя отправляются в публичную таблицу логов.
### **Revolver**
Revolver - Сервис для стрельбы по другим сервисам. Когда происходит выстрел по другому сервису, то тот падает, но через определённое время встаёт.
### **LifeMonitor**
LifeMonitor - Сервис который следит за здоровьем других сервисов. На главной странице отображает, какой сервис упал, какой стоит, а какой имеет неуязвимость на какое-то время.
### **Profile**
Profile - Сервис с историей выстрелов пользователя. После регистрации пользователь может перейти на свою страницу с главной страницы.



