namespace Final.Task.HW_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
           
        }
    }

    /// <summary>
    /// Классы пользователей
    /// </summary>

    abstract class User
    {
        private int id;
        public int ID { get; set; }

        private string userName;
        public string UserName { get; set; }

        public int phoneNumber;
        public string eMail;
    }

    class Customer<TMyOrders> : User
    {
        public TMyOrders myOrders;
    }

    class Courier<TOrder> : User
    {
        public TOrder orderHomeDelivery;
    }

    class Driver<TOrderHD, TOrderSD>
    {
        public TOrderHD orderPickPointDelivery;
        public TOrderSD orderShopDelivery;
    }
    

    abstract class Delivery
    {
        public string Address;
    }

    class HomeDelivery : Delivery
    {
        public int courierId;
    }

    class PickPointDelivery : Delivery
    {
        public int driverId;
    }

    class ShopDelivery : Delivery
    {
        public int driverId;
    }

    class Order<TDelivery> where TDelivery : Delivery
    {
        public TDelivery Delivery;

        public int Number;

        public string Description;

        public void DisplayAddress()
        {
            Console.WriteLine(Delivery.Address);
        }

        // ... Другие поля
    }

    class MyOrders<TOrder>
    {
        public int userId;
        public TOrder[] orders;
    }
}


