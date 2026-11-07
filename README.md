# WT-6 Minimal API — Университет

REST API для управления студентами. ASP.NET Core Minimal API, .NET 8.

## API

| Метод | URL | Назначение |
|---|---|---|
| GET | `/api/students` | Получить список студентов (с фильтрацией `?search=`) |
| GET | `/api/students/{id}` | Получить студента по ID |
| POST | `/api/students` | Создать студента |
| PUT | `/api/students/{id}` | Обновить студента |
| DELETE | `/api/students/{id}` | Удалить студента |
| GET | `/api/info` | Информация о приложении |

## Пример POST-запроса

**Запрос:**
```json
POST /api/students
Content-Type: application/json

{
  "fullName": "Новиков Артём Сергеевич",
  "specialty": "Программная инженерия",
  "course": 2,
  "averageGrade": 8.5,
  "isEnrolled": true
}