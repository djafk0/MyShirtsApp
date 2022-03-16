namespace MyShirtsApp.Data
{
    public class DataConstants
    {
        public class Shirt
        {
            public const int NameMaxLength = 30;
            public const int NameMinLength = 3;
            public const decimal MaxPrice = 999.99M;
            public const decimal MinPrice = 0.01M;
            public const int FabricMaxLength = 30;
            public const int FabricMinLength = 3;
            public const int ImageUrlMaxLength = 2048;
        }

        public class Size
        {
            public const int NameMaxLength = 3;
        }

        public class ShirtSize
        {
            public const int MinCount = 0;
            public const int MaxCount = 10;
        }
    }
}
