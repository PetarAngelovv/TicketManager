namespace GCommon
{
    public static class GlobalValidation
    {
        public static class Category
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }

        public static class Event
        {
            public const int NameMinLength = 3;
            public const int NameMaxLength = 100;

            public const int LocationMinLength = 3;
            public const int LocationMaxLength = 100;

            public const int DescriptionMinLength = 10;
            public const int DescriptionMaxLength = 1000;

            public const double MinPrice = 0.01;
            public const double MaxPrice = 1000.00;

            public const int MaxTickets = 10000;
            public const int MinTickets = 1;

            public const string CreatedOnFormat = "yyyy-MM-dd";
            public const int CreatedOnLength = 10;
        }
    }

        public static class Ticket
        {
            public const decimal MinPrice = 1.00m;
            public const decimal MaxPrice = 1000.00m;
        }

        public static class Order
        {
            // Ако решa да валидираме нещо конкретно в поръчките (напр. UserId), тук ще го добавим
        }

        public static class General
        {
            public const string RequiredErrorMessage = "Полето е задължително.";
        }
    }