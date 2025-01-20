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

            //order.AddProduct<Product>(customers[0].ID, productsStore);

            Console.ReadKey();
        }
    }
    /// <summary>
    /// Статический класс вывода информации
    /// </summary>
    public static class Printer
    {
        public static void Print(this Product[] products)// Вывести на экран список продуктов
        {
            foreach (var product in products)
            {
                Console.WriteLine($"{product.ID} {product.Name}");
            }
        }

        public static void Print()// Сообщение о типах доставки
        {
            Console.WriteLine("Выберите тип доставки цифрой:\n" +
                 "1. Доставка по адресу\n" +
                 "2. Доставка до пункта выдачи\n" +
                 "3. Забрать из магазина партнёра");
        }

        public static void Print(int type)// Сообщение о внесении адреса
        {
            switch (type)
            {
                case 1:
                    Console.WriteLine("Доставка по адресу. Введите адрес доставки:");
                    break;
                case 2:
                    Console.WriteLine("Доставка в пункт выдачи. Введите адрес доставки:");
                    break;

                case 3:
                    Console.WriteLine("Доставка в магазин. Введите адрес доставки:");
                    break;
                default:
                    Console.WriteLine("Такого типа не существует, попробуйте снова.");
                    break;
            }
        }
    }

    /// <summary>
    /// Статический класс получения и конвертации данных из косоли
    /// </summary>
    public static class Checker
    {
        public static int InsertInt()
        {
            if (int.TryParse(Console.ReadLine(), out int result))
            {
                return result;
            }

            Console.WriteLine("Введите значение цифрами:");
            return InsertInt();
        }

        public static string InsertString()
        {
            var insert = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(insert))
            {
                return insert;
            }

            Console.WriteLine("Поле не должно быть путым. Попробуйте ещё раз:");
            return InsertString();
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
            };

            order.Delivery = order.GetTypeDelivery();
            order.Description = order.InsertDescription();
            order.ProductsStore = order.AddProduct<Product>(customerId);

            return order;
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

        public abstract Order<Delivery>[] OrdersCurrent { get; set; }
        public abstract Order<Delivery>[] OrdersFinished { get; set; }

        public abstract Order<Delivery> AddOrderCurrent();
        public abstract Order<Delivery> AddOrderFinal();

        public virtual Order<Delivery> MoveOrder(Order<Delivery> order)
        {
            return default;
        }
    }

    public class Customer : User
    {
        private Cart customerCart;
        public Cart CustomerCart { get => CustomerCart = customerCart; set => customerCart = value; }

        public override Order<Delivery>[] OrdersCurrent { get; set; }
        public override Order<Delivery>[] OrdersFinished { get; set; }

        private bool orderDone;

        public Customer(int id, string name)
        {
            ID = id;
            UserName = name;
        }

        private void AddOrder(Order<Delivery> order)
        {
            var orders = OrdersCurrent;

            var result = new Order<Delivery>[orders.Length + 1];

            for (int i = 0; i < result.Length; i++)
            {
                if (i < result.Length - 1)
                    result[i] = orders[i];
                else
                    result[i] = order;
            }
            OrdersCurrent = result;
        }

        public override Order<Delivery> AddOrderCurrent()
        {
            throw new NotImplementedException();
        }

        public override Order<Delivery> AddOrderFinal()
        {
            throw new NotImplementedException();
        }
    }

    public class Courier : User
    {
        public override Order<Delivery>[] OrdersCurrent { get; set; }
        public override Order<Delivery>[] OrdersFinished { get; set; }

        public override Order<Delivery> AddOrderCurrent()
        {
            throw new NotImplementedException();
        }

        public override Order<Delivery> AddOrderFinal()
        {
            throw new NotImplementedException();
        }
    }

    public class Driver : User
    {
        public override Order<Delivery>[] OrdersCurrent { get; set; }
        public override Order<Delivery>[] OrdersFinished { get; set; }

        public override Order<Delivery> AddOrderCurrent()
        {
            throw new NotImplementedException();
        }

        public override Order<Delivery> AddOrderFinal()
        {
            throw new NotImplementedException();
        }
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

        public string[] AddressesPickPoints { get; set; }
    }

    public class ShopDelivery : Delivery
    {
        public int driverId;
        public string companyName;

        public string[] AddressesShops { get; set; }
    }

    public class Order<TDelivery> where TDelivery : Delivery
    {
        public TDelivery Delivery;

        public int Number;

        public string Description;

        private Product[] productsStore;
        public Product[] ProductsStore { get => ProductsStore = productsStore; set => productsStore = value; }

        public Order(int customerId, Product[] products)
        {
            this.productsStore = products;
        }

        public Product[] AddProduct<TProduct>(int customerID) where TProduct : Product
        {
            ProductsStore.Print();

            var orderedProducts = new List<Product>();

            var openOrder = true;

            

            while (openOrder)
            {
                Console.WriteLine("Выберите товар по номеру цифрами:");

                var index = Checker.InsertInt();

                var product = ProductsStore[index - 1];

                Console.WriteLine("Укажите количество цифрами:");

                var count = Checker.InsertInt();

                var orderedProduct = new Product[count];

                for (int i = 0; i < count; i++)
                {
                    orderedProduct[i] = ProductsStore[index - 1];
                }

                for (int i = 0; i < orderedProduct.Length; i++)
                {
                    orderedProducts.Add(orderedProduct[i]);
                }

                Console.WriteLine("Продолжить подбор товаров, (да или нет)?");

                if (Checker.InsertString() == "нет")
                {
                    openOrder = false;
                }

                ProductsStore.Print();
            }





            return default;
        }

        public Delivery GetTypeDelivery()
        {
            Printer.Print();

            var type = Checker.InsertInt();

            switch (type)
            {
                case 1:
                    Printer.Print(type);
                    return new HomeDelivery()
                    {
                        Address = Checker.InsertString(),
                    };

                case 2:
                    Printer.Print(type);
                    return new PickPointDelivery()
                    {
                        Address = Checker.InsertString(),
                    };

                case 3:
                    Printer.Print(type);
                    return new ShopDelivery()
                    {
                        Address = Checker.InsertString(),
                    };
            }
            Printer.Print(type);
            return GetTypeDelivery();
        }

        public string InsertDescription()
        {
            Console.WriteLine("Введите комментарий:");
            return Checker.InsertString();
        }
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


