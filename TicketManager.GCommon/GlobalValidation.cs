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
}