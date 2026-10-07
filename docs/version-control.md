# Правила работы с Git

## Ветки
- `main` — стабильная версия проекта
- `develop` — интеграционная ветка
- `feature/*` — новые функции
- `fix/*` — исправления дефектов
- `docs/*` — изменения документации

## Правила коммитов
Формат: `<type>: <description>`
Типы: feat, fix, docs, test, refactor, chore

## Слияние
- feature/* сливаются в develop через Pull Request
- Перед слиянием: dotnet build и dotnet test