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
            }.ToArray();

            products.PrintProducts();

            Console.ReadKey();
        }
    }

    /// <summary>
    /// Статический класс вывода информации
    /// </summary>
    public static class Printer
    {
        public static void PrintProducts(this Product[] products)
        {
            foreach (var product in products)
            {
                Console.WriteLine($"{product.ID} {product.name}");
            }
        }
    }

    /// <summary>
    /// Класс корзины заказов
    /// </summary>

    public class Cart
    {
        public int customerID;
        public Product[] products;

        public Cart(int customerID)
        {
            this.customerID = customerID;
        }

        public Product[] AddProduct<TProduct>(int customerID) where TProduct : Product
        {
            return null;
        }

        public Order<Delivery> CreateOrder<TOrder>() where TOrder : Order<Delivery>
        {
            return new Order<Delivery>();
        }
    }

    /// <summary>
    /// Классы пользователей
    /// </summary>

    public abstract class User
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

    public class Courier : User
    {
        public Order<HomeDelivery>[] ordersHomeDelivery;
    }

    public class Driver : User
    {
        public Order<Delivery>[] orders;
    }


    public abstract class Delivery
    {
        public string Address;
    }

    public class HomeDelivery : Delivery
    {
        public int courierId;
    }

    public class PickPointDelivery : Delivery
    {
        public int driverId;
    }

    public class ShopDelivery : Delivery
    {
        public int driverId;
    }

    public class Order<TDelivery> where TDelivery : Delivery
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
    /// Классы продуктов
    /// </summary>

    public abstract class Product
    {
        public int id;
        public string name;

        public int ID { get => ID = id; protected set => id = value; }
        public string Name { get => Name = name; set => name = value; }

        protected Product(int id, string name)
        {
        }
    }

    public class Fruit : Product
    {
        public Fruit(int id, string name) : base(0, "Product")
        {
            this.id = id;
            this.name = name;
        }
    }

    public class Vegetable : Product
    {
        public Vegetable(int id, string name) : base(0, "Product")
        {
            this.id = id;
            this.name = name;
        }
    }

    public class Milky : Product
    {
        public Milky(int id, string name) : base(0, "Product")
        {
            this.id = id;
            this.name = name;
        }
    }
}


