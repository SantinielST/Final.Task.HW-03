namespace Final.Task.HW_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var memoryIDCustomer = 1;
            var memoryIDCourier = 1;
            var memoryIDDriver = 1;
            

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

        public virtual Order<Delivery> MoveOrder(Order<Delivery> order)
        {
            return default;
        }
    }

    class Customer : User
    {
        public Cart customerCart;
        public Order<Delivery>[] myOrders;

        public void AddOrder(Order<Delivery> order)
        {
            var orders = myOrders;

            var result = new Order<Delivery>[orders.Length + 1];

            for (int i = 0; i < result.Length; i++)
            {
                if (i < result.Length - 1)
                    result[i] = orders[i];
                else
                    result[i] = order;
            }
            myOrders = result;
        }
    }

    public class Courier : User
    {
        public Order<HomeDelivery>[] ordersHomeDelivery;
    }

    public class Driver : User
    {
        public Order<Delivery>[] orders;
    }

    /// <summary>
    /// Классы доставки
    /// </summary>
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
        public string companyName;
    }

    public class Order<TDelivery> where TDelivery : Delivery
    {
        public TDelivery Delivery;

        public int Number;

        public string Description;

        public Product[] AddProduct<TProduct>(int customerID) where TProduct : Product
        {
            return null;
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


