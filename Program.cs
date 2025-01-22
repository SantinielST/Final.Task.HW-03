namespace Final.Task.HW_03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var addressesPickPoint = new[] { "Арбатская", "Павелецкая", "Марьино" };
            var adressesShop = new[] { "Королёв", "Видное", "Одинцово" };

            var productsStore = new Product[]
            {
                new Fruit(1, "Apple"),
                new Vegetable(2, "Tomato"),
                new Milky(3, "Milk")
            };

            var courier = new Courier(1, "Courier1");
            var driver = new Driver(1, "Driver1");
            var customers = new Customer[3];

            for (int i = 0; i < customers.Length; i++)
            {
                customers[i] = new Customer(i + 1, ($"Customer{i + 1}"));
                customers[i].CustomerCart = new Cart(customers[i].ID, productsStore, addressesPickPoint, adressesShop);
            }

            customers[0].OrdersCurrent = new[] { customers[0].CustomerCart.CreateOrder() };
            customers[0].OrdersCurrent[0].Print();

            Console.ReadKey();
        }
    }
    /// <summary>
    /// Статический класс вывода информации
    /// </summary>
    public static class Printer
    {
        public static void Print<TProducts>(this TProducts[] products) where TProducts : Product// Вывести на экран список продуктов
        {
            foreach (var product in products)
            {
                Console.WriteLine($"{product.ID} {product.Name}");
            }
        }

        //public static void Print(this List<Product> products)// Вывести на экран список заказанных продуктов
        //{
        //    var printOrderProd = new List<Product>();

        //    foreach (var product in products)
        //    {
        //        for (int i = 0; i > products.Count; i++)
        //        {

        //        }

        //        Console.WriteLine($"{product.ID} {product.Name}");
        //    }
        //}

        public static void Print(this string[] addresses)// Вевести на экран адреса доставки
        {
            for (int i = 0; i < addresses.Length; i++)
            {
                Console.WriteLine($"{i + 1} {addresses[i]}");
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
                    Console.WriteLine("Доставка в пункт выдачи. Выберите адрес доставки:");
                    break;

                case 3:
                    Console.WriteLine("Доставка в магазин. Выберите адрес доставки:");
                    break;
                default:
                    Console.WriteLine("Такого типа не существует, попробуйте снова.");
                    break;
            }
        }

        public static void Print(this Order<Delivery> order)
        {
            Console.WriteLine($"Ваш заказ №{order.Number}\n" +
                $"Доставка по адресу: {order.Delivery.Address}\n" +
                $"Комментарий: {order.Description}\n" +
                $"Ваши покупки:");

            order.OrderedProducts.Print();
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

            Console.WriteLine("Поле не должно быть пустым. Попробуйте ещё раз:");
            return InsertString();
        }

        public static bool ContinueOrder()
        {
            var answer = Console.ReadLine().ToLower();
            var validAnswers = new string[] { "да", "нет" };

            if (answer == validAnswers[0] || answer == validAnswers[1])
            {
                if (answer == validAnswers[0])
                    return true;
                else
                    return false;
            }
            else
            {
                Console.WriteLine("Уточните, (\"да\" или \"нет\"):");
                return ContinueOrder();
            }
        }

        public static int CheckIndexRange(string[] addresses)
        {
            if (int.TryParse(Console.ReadLine(), out int result))
            {
                if (result <= addresses.LongLength && result > 0)
                    return result;
            }

            Console.WriteLine("Такого адреса не существует, попробуйте снова:");
            return CheckIndexRange(addresses);
        }
    }

    /// <summary>
    /// Класс корзины
    /// </summary>
    public class Cart
    {
        private int customerId;
        public int CustomerId { get => CustomerId = customerId; set => customerId = value; }

        private string[] addressesPickPoint;
        private string[] adressesShop;

        private Product[] productsStore;
        public Product[] ProductsStore
        {
            get => ProductsStore = productsStore;
            private set
            {
                if (value is null || value.Length == 0)
                {
                    Console.WriteLine("В магазине закончился товар.");
                    // Закончить выполнение программы.
                }
                else
                {
                    productsStore = value;
                }
            }
        }
        public Product this[int index]
        {
            get
            {
                if (index >= 0 && index < productsStore.Length)
                {
                    return productsStore[index];
                }
                else
                {
                    return null;
                }
            }
            private set
            {
                if (index >= 0 && index < productsStore.Length)
                {
                    productsStore[index] = value;
                }
            }
        }

        public Cart(int customerId, Product[] products, string[] addressesPP, string[] addressesPPShops)
        {
            this.customerId = customerId;
            productsStore = products;
            addressesPickPoint = addressesPP;
            adressesShop = addressesPPShops;
        }

        public Order<Delivery> CreateOrder()
        {
            var random = new Random().Next(100);

            var order = new Order<Delivery>(ProductsStore)
            {
                Number = random,
            };

            order.Delivery = order.GetTypeDelivery(addressesPickPoint, adressesShop);
            order.Description = order.InsertDescription();
            order.OrderedProducts = AddProduct<Product>();

            return order;
        }

        private Product[] AddProduct<TProduct>() where TProduct : Product // Метод добавления товаров в корзину
        {
            var orderedProducts = new List<Product>();
            var openOrder = true;

            while (openOrder)
            {
                Console.WriteLine("Выберите товар по номеру цифрами:");
                ProductsStore.Print();

                var index = Checker.InsertInt();

                if (index <= ProductsStore.LongLength && index > 0)
                {
                    var product = ProductsStore[index - 1];
                }
                else
                {
                    Console.WriteLine("Такого товара не существует, попробуйте снова.");
                    ProductsStore.Print();
                    continue;
                }

                Console.WriteLine("Укажите количество цифрами:");

                var count = Checker.InsertInt();
                var orderedProduct = new Product[count];

                for (int i = 0; i < count; i++)
                {
                    orderedProduct[i] = ProductsStore[index - 1];
                    orderedProducts.Add(orderedProduct[i]);
                }

                Console.WriteLine("Продолжить подбор товаров, (да или нет)?");

                if (!Checker.ContinueOrder())
                {
                    openOrder = false;
                }
            }
            return orderedProducts.ToArray();
        }
    }

    /// <summary>
    /// Классы пользователей
    /// </summary>

    public abstract class User
    {
        private int id;
        private string userName;

        public int ID { get => ID = id; set => id = value; }
        public string UserName { get => UserName = userName; set => userName = value; }

        public abstract Order<Delivery>[] OrdersCurrent { get; set; }
        public abstract Order<Delivery>[] OrdersFinished { get; set; }

        public abstract Order<Delivery> AddOrderCurrent();
        public abstract Order<Delivery> AddOrderFinal();

        public User(int id, string userName)
        {

        }

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

        public Customer(int id, string name) : base(id, name)
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

        public Courier(int id, string userName) : base(id, userName)
        {
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

    public class Driver : User
    {
        public override Order<Delivery>[] OrdersCurrent { get; set; }
        public override Order<Delivery>[] OrdersFinished { get; set; }

        public Driver(int id, string driverName) : base(id, driverName)
        {

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
    }

    /// <summary>
    /// Класс заказа
    /// </summary>
    /// <typeparam name="TDelivery"></typeparam>
    public class Order<TDelivery> where TDelivery : Delivery
    {
        public TDelivery Delivery;

        public int Number;

        public string Description;

        private Product[] orderedProducts;
        public Product[] OrderedProducts { get => OrderedProducts = orderedProducts; set => orderedProducts = value; }
        public Product this[int index]
        {
            get
            {
                if (index >= 0 && index < orderedProducts.Length)
                {
                    return orderedProducts[index];
                }
                else
                {
                    return null;
                }
            }
            private set
            {
                if (index >= 0 && index < orderedProducts.Length)
                {
                    orderedProducts[index] = value;
                }
            }
        }

        public Order(Product[] products)
        {
            this.orderedProducts = products;
        }

        public Delivery GetTypeDelivery(string[] addressesPP, string[] addressesPPShops) // Выбор типа доставки
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
                    addressesPP.Print();
                    return new PickPointDelivery()
                    {
                        Address = addressesPP[Checker.CheckIndexRange(addressesPP) - 1],
                    };

                case 3:
                    Printer.Print(type);
                    addressesPPShops.Print();
                    return new ShopDelivery()
                    {
                        Address = addressesPPShops[Checker.CheckIndexRange(addressesPP) - 1],
                    };
            }
            Printer.Print(type);
            return GetTypeDelivery(addressesPP, addressesPPShops);
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
        public Fruit(int id, string name) : base(id, name)
        {
            ID = id;
            Name = name;
        }
    }

    public class Vegetable : Product
    {
        public Vegetable(int id, string name) : base(id, name)
        {
            ID = id;
            Name = name;
        }
    }

    public class Milky : Product
    {
        public Milky(int id, string name) : base(id, name)
        {
            ID = id;
            Name = name;
        }
    }

}


