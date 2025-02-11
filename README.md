чтобы запустить нужно выполнить в папке проекта:
1. Запустить контейнер с базой данных `docker compose up`
2. Применить миграции к БД `dotnet ef --project DAL database update  --context OrdersTaskContext --startup-project WebApi`
3. установить бибилотеки для фронта `npm install`
4. Запуск бека из визуалки профиль https
5. Запуск фронта `npm run dev`
