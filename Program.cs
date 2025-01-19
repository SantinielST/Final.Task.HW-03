namespace Final.Task.HW_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var products = new List<Product>
            {
                new Fruit(1, "Apple"),
                new Vegetable(2, "Tomato"),
                new Milky(3, "Milk")
            };
            Console.ReadKey();
        }
    }

    class Cart
    {
        public int customerID;
        public Product[] products;

        public Order<Delivery> CreateOrder<TOrder>() where TOrder : Order<Delivery>
        {
            return new Order<Delivery>();
        }

    }

    /// <summary>
    /// Классы пользователей
    /// </summary>

    abstract class User
    {
        public int id;
        public string userName;
        public int phoneNumber;
        public string eMail;

    }

    class Customer : User
    {
        public Cart customerCart;
        public Order<Delivery>[] myOrders;
    }

    class Courier : User
    {
        public Order<HomeDelivery>[] ordersHomeDelivery;
    }

    class Driver : User
    {
        public Order<Delivery>[] orders;
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

    /// <summary>
    /// Добавлены классы продуктов
    /// </summary>

    abstract class Product
    {
        public int id;
        public string name;

        public int ID { get => ID = id; protected set => id = value; }
        public string Name { get => Name = name; set => Name = value; }

        protected Product(int id, string name)
        {
        }
    }

    class Fruit : Product
    {
        public Fruit(int id, string name) : base(0, "Product")
        {
            this.id = id;
            this.name = name;
        }
    }

    class Vegetable : Product
    {
        public Vegetable(int id, string name) : base(0, "Product")
        {
            this.id = id;
            this.name = name;
        }
    }

    class Milky : Product
    {
        public Milky(int id, string name) : base(0, "Product")
        {
            this.id = id;
            this.name = name;
        }
    }
}


