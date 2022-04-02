namespace Ignite.GlobalConstants
{
    public class GlobalConstants
    {
        public const int SubscriptionProductType = 0;

        public const int ClassProductType = 1;


        public const int SubscriptionTypeDaily = 0;

        public const int SubscriptionTypeBasic = 1;
      
        public const int SubscriptionTypePremium = 2;

        public const int SubscriptionTypeVIP = 3;
        
        public const int SubscriptionTypeDailyOrder = 0;

        public const int SubscriptionTypeBasicOrder = 1;
      
        public const int SubscriptionTypePremiumOrder = 2;

        public const int SubscriptionTypeVIPOrder = 3;


        public const decimal SubscriptionTypeDailyPrice = 5;

        public const decimal SubscriptionTypeBasicPrice = 20;

        public const decimal SubscriptionTypePremiumPrice = 30;

        public const decimal SubscriptionTypeVipPrice = 40;

        
        public static TimeSpan SubscriptionTypeDailyDurationInDays = new TimeSpan(1, 0, 0 ,0);

        public static TimeSpan SubscriptionTypeBasicDurationInDays = new TimeSpan(30, 0, 0, 0);

        public static TimeSpan SubscriptionTypePremiumDurationInDays = new TimeSpan(30, 0, 0, 0);

        public static TimeSpan SubscriptionTypeVipDurationInDays = new TimeSpan(30, 0, 0, 0);

        
        public const int TopClassesCount = 3;

        public const int TopEventsCount = 3;


        public const string AdministratorRoleName = "Administrator";


        public const double ClassMaxPrice = 1000;

        public const int ClassMaxSeats = 20;

        public const int ClassMinDuration = 5;

        public const int ClassMaxDuration = 120;
    }
}