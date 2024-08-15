# BoreholeSystem

BoreholeSystem — это десктопное приложение, предназначенное для визуализации и управления данными с самодельного инклинометра, подключенного через USB к Arduino. Оно позволяет пользователям просматривать данные в реальном времени, переключаться между различными модулями и управлять записями в базе данных приложения.
Помимо этого имеется модуль доступа к устройству "Термокоса" для получения температур скважины расстоянием 10 метров (один датчик на каждый метр).
## Особенности

- **Служба навигации**: Позволяет перемещаться между различными представлениями в приложении с помощью централизованной службы навигации.
- **Управление базой данных**: Хранение и извлечение данных инклинометра с использованием базы данных SQLite.
- **Серийная связь**: Обмен данными с Arduino через COM-порты для получения данных измерений.
- **Динамический UI**: Использование Avalonia для кроссплатформенной разработки интерфейса с удобными управлениями и обработкой ошибок.
- **Отображение данных**: Визуальное представление данных инклинометра, включая углы и температуру.

## Архитектура

Приложение разделено на несколько ключевых компонентов:

1. **ViewModels (Модели представления)**: Здесь управляется основная бизнес-логика и обработка данных. Каждая модель представления соответствует определенной части приложения (например, `InclinometerControlViewModel`, `DatabaseViewModel`).
   
2. **Сервисы**: Сервисы, такие как `INavigationService` и `IWPFService`, управляют навигацией приложения и жизненным циклом WPF-приложения соответственно.

3. **База данных**: Класс `DatabaseController` отвечает за взаимодействие с SQLite для выполнения операций CRUD в отношении данных инклинометра.

4. **Представления**: Компоненты пользовательского интерфейса, определенные с помощью XAML. Каждое представление соответствует модели представления, создавая четкое разделение между интерфейсом и основной логикой.

## Начало работы

### Предварительные требования

- .NET 8.0 или выше
- Фреймворк Avalonia UI
- База данных SQLite (включена в приложение)

### Установка

1. Клонируйте репозиторий:
   ```sh
   git clone [https://github.com/yourusername/BoreholeSystem.git](https://github.com/CNTJManoi/BoreholeSystem.git)
   cd BoreholeSystem
   ```

2. Восстановите зависимости:
   ```sh
   dotnet restore
   ```

3. Постройте приложение:
   ```sh
   dotnet build
   ```

### Запуск приложения

Чтобы запустить приложение, выполните следующую команду:

```sh
dotnet run
```

### Использование

- После запуска приложения вы можете переключаться между различными модулями, используя доступные кнопки.
- Подключите Arduino к USB-порту и выберите соответствующий COM-порт в представлении управления инклинометром.
- Нажмите "Получить данные", чтобы извлечь данные в реальном времени с вашего инклинометра.
- Измерения можно сохранить в базе данных SQLite, нажав "Сохранить в базу данных".

## Лицензия

Этот проект лицензирован под лицензией MIT — см. файл [LICENSE](LICENSE) для получения подробной информации. 

## Благодарности

- [Avalonia](https://avaloniaui.net/) за кроссплатформенную разработку интерфейсов.
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/ru-ru/communitytoolkit/mvvm/) за паттерны MVVM.
- Arduino.
- Лаборатории неразрушающего контроля сибирского государственного университета путей сообщения за предоставленное техническое задание.
