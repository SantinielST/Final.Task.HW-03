namespace Final.Task.HW_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var memoryIDCustomer = 1;
            var memoryIDCourier = 1;
            var memoryIDDriver = 1;

            var customers = new Customer[3];

            for (int i = 0; i < customers.Length; i++)
            {
                customers[i] = new Customer(i + 1, (i + 5).ToString());
                customers[i].CustomerCart = new Cart(customers[i].ID);
            }

            var productsStore = new List<Product>
            {
                new Fruit(1, "Apple"),
                new Vegetable(2, "Tomato"),
                new Milky(3, "Milk")
            }.ToArray();

            var order = customers[0].CustomerCart.CreateOrder(productsStore);

            order.AddProduct<Product>(customers[0].ID, productsStore);
            //Console.WriteLine(order.Delivery.Address);

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
                Console.WriteLine($"{product.ID} {product.Name}");
            }
        }

        public static void PrintTypeDelivery()
        {
            Console.WriteLine("Выберите тип доставки цифрой:\n" +
                 "1. Доставка по адресу\n" +
                 "2. Доставка до пункта выдачи\n" +
                 "3. Забрать из магазина партнёра");
        }
    }

    /// <summary>
    /// Класс корзины
    /// </summary>
    public class Cart
    {
        private int customerId;
        public int CustomerId { get => CustomerId = customerId; set => customerId = value; }

        private Product[] productStore;
        public Product[] ProductStore { get; set; }

        public Cart(int customerId)
        {
            this.customerId = customerId;
        }

        public Order<Delivery> CreateOrder(Product[] ProductStore)
        {
            var random = new Random().Next(100);

            var order = new Order<Delivery>(CustomerId, ProductStore)
            {
                Number = random,
                Delivery = GetTypeDelivery(),
                Description = InsertDescription()
            };
            return order;
        }

        private Delivery GetTypeDelivery()
        {
            Printer.PrintTypeDelivery();

            var type = int.Parse(Console.ReadLine());

            switch (type)
            {
                case 1:
                    Console.WriteLine("Доставка по адресу. Введите адрес доставки:");
                    return new HomeDelivery()
                    {
                        Address = Console.ReadLine()
                    };

                case 2:
                    Console.WriteLine("Доставка в пункт выдачи. Введите адрес доставки:");
                    return new PickPointDelivery()
                    {
                        Address = Console.ReadLine()
                    };

                case 3:
                    Console.WriteLine("Доставка в магазин. Введите адрес доставки:");
                    return new ShopDelivery()
                    {
                        Address = Console.ReadLine()
                    };
            }

            return GetTypeDelivery();
        }

        private string InsertDescription()
        {
            Console.WriteLine("Введите комментарий");
            return Console.ReadLine();
        }
    }

    /// <summary>
    /// Классы пользователей
    /// </summary>

    public abstract class User
    {
        private int id;
        private string userName;
        private int phoneNumber;
        private string eMail;

        public int ID { get => ID = id; set => id = value; }
        public string UserName { get => UserName = userName; set => userName = value; }
        public int PhoneNumber { get; set; }
        public string EMail { get; set; }

        public virtual Order<Delivery> MoveOrder(Order<Delivery> order)
        {
            return default;
        }
    }

    public class Customer : User
    {
        private Cart customerCart;
        public Cart CustomerCart { get => CustomerCart = customerCart; set => customerCart = value; }

        public Order<Delivery>[] MyOrders { get; set; }

        private bool orderDone;

        public Customer(int id, string name)
        {
            ID = id;
            UserName = name;
        }

        private void AddOrder(Order<Delivery> order)
        {
            var orders = MyOrders;

            var result = new Order<Delivery>[orders.Length + 1];

            for (int i = 0; i < result.Length; i++)
            {
                if (i < result.Length - 1)
                    result[i] = orders[i];
                else
                    result[i] = order;
            }
            MyOrders = result;
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

        public Order(int customerId, Product[] products)
        {
            
        }

        public (bool, Product[]) AddProduct<TProduct>(int customerID, Product[] productsStore) where TProduct : Product
        {
            productsStore.PrintProducts();
            return default;
        }



        // ... Другие поля
    }

    /// <summary>
    /// Классы продуктов
    /// </summary>

    public abstract class Product
    {
        private int id;
        private string name;

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
            ID = id;
            Name = name;
        }
    }

    public class Vegetable : Product
    {
        public Vegetable(int id, string name) : base(0, "Product")
        {
            ID = id;
            Name = name;
        }
    }

    public class Milky : Product
    {
        public Milky(int id, string name) : base(0, "Product")
        {
            ID = id;
            Name = name;
        }
    }

}


