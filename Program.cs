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
                new Milky(3, "Milk"),
                new Fruit(4, "Orange")
            };

            var courier = new Courier(1, "Courier1");
            var driver = new Driver(1, "Driver1");
            var customer = new Customer(1, "Customer1");

            customer.OrdersCurrent = new[] { customer.CustomerCart.CreateOrder<Order<Delivery>>(addressesPickPoint, adressesShop, productsStore) };
            customer.OrdersCurrent[0].Print();

            if (customer.OrdersCurrent[0].Delivery is HomeDelivery)
            {
                courier.OrdersCurrent = new Order<Delivery>[customer.OrdersCurrent.Length];
                courier.OrdersCurrent[0] = customer.OrdersCurrent[0];

                courier.MoveOrder(courier.OrdersCurrent[0]);
                Console.WriteLine("Заказ доставлен!");
            }
            else
            {
                driver.OrdersCurrent = new Order<Delivery>[customer.OrdersCurrent.Length];
                driver.OrdersCurrent[0] = customer.OrdersCurrent[0];

                driver.MoveOrder(driver.OrdersCurrent[0]);

                Console.WriteLine();
                Console.WriteLine("Заказ доставлен!");
            }

            Console.ReadKey();
        }
    }
    /// <summary>
    /// Статический класс вывода информации
    /// </summary>
    public static class Printer
    {
        public static void Print<TProduct>(this TProduct[] products) where TProduct : Product// Вывести на экран список продуктов
        {
            foreach (var product in products)
            {
                Console.WriteLine($"{product.ID} {product.Name}");
            }
        }

        public static void PrintOrderedProducts<TProduct>(this TProduct[] orderdProducts) where TProduct : Product// Вывести на экран список заказанных продуктов
        {
            var products = orderdProducts.GroupBy(p => p.ID);

            foreach (var product in products)
            {
                Console.WriteLine($"{product.Select(p => p.Name).First()} {product.Count()}");
            }
        }

        public static void Print(this string[] addresses)// Вывести на экран адреса доставки
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

        public static void Print(this Order<Delivery> order)// Вывод на экран итоговой информации по заказу
        {
            Console.WriteLine();
            Console.WriteLine($"Ваш заказ №{order.Number}\n" +
                $"Доставка по адресу: {order.Delivery.Address}\n" +
                $"Комментарий: {order.Description}\n" +
                $"Ваши покупки:");

            order.OrderedProducts.PrintOrderedProducts();
        }
    }

    /// <summary>
    /// Статический класс получения и конвертации данных из косоли
    /// </summary>
    public static class Checker
    {
        public static int InsertInt()// Метод проверки ввода на число
        {
            if (int.TryParse(Console.ReadLine(), out int result))
            {
                return result;
            }

            Console.WriteLine("Введите значение цифрами:");
            return InsertInt();
        }

        public static string InsertString() // Метод проверки на пустую строку
        {
            var insert = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(insert))
            {
                return insert;
            }

            Console.WriteLine("Поле не должно быть пустым. Попробуйте ещё раз:");
            return InsertString();
        }

        public static string CheckLine() // Метод проверки ответа для продолжения/завершения заказа
        {
            var answer = Console.ReadLine().ToLower();
            var validAnswers = new string[] { "да", "нет" };

            if (answer == validAnswers[0] || answer == validAnswers[1])
            {
                return answer;
            }
            else
            {
                Console.WriteLine("Уточните, (\"да\" или \"нет\"):");
                return CheckLine();
            }
        }

        public static int CheckIndexRange(string[] addresses)// Метод проверки индекса
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
    /// Класс перегрузки операторов true/false
    /// </summary>
    public class Answer
    {
        private string line; 
        public string Line { get => Line = line; set => line = value; }

        public Answer()
        {
            line = Checker.CheckLine();
        }

        public static bool operator true(Answer answer)
        {
            return answer.Line == "нет";
        }

        public static bool operator false(Answer answer)
        {
            var Line = Checker.InsertString();

            return answer.Line == "да";
        }
    }

    /// <summary>
    /// Класс корзины
    /// </summary>
    public class Cart
    {
        private int customerId;
        public int CustomerId { get => CustomerId = customerId; set => customerId = value; }

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

        public Cart(int customerId)
        {
            this.customerId = customerId;
        }

        public Order<Delivery> CreateOrder<TOrder>(string[] addressesPickPoint, string[] adressesShop, Product[] products) where TOrder : Order<Delivery> // Метод создания заказа
        {
            productsStore = products;

            var random = new Random().Next(100);

            var order = new Order<Delivery>(ProductsStore)
            {
                Number = random,
            };

            order.Delivery = order.GetTypeDelivery(addressesPickPoint, adressesShop);
            order.Description = order.InsertDescription();
            order.OrderedProducts = AddProduct<Product>(products);

            return order;
        }

        private Product[] AddProduct<TProduct>(Product[] products) where TProduct : Product // Метод добавления товара в корзину
        {
            productsStore = products;

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

                var answer = new Answer();

                if (answer)
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
        public int id;
        public string userName;

        public int ID { get => ID = id; set => id = value; }
        public string UserName { get => UserName = userName; set => userName = value; }

        public abstract Order<Delivery>[] OrdersCurrent { get; set; }
        public abstract Order<Delivery>[] OrdersFinished { get; set; }

        public User(int id, string userName)
        {

        }
        
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

        public Customer(int id, string name) : base(id, name)
        {
            this.id = id;
            this.userName = name;

            customerCart = new Cart(ID);
        }

        public override Order<Delivery> MoveOrder(Order<Delivery> order)
        {
            return OrdersFinished[0] = AddOrderFinal();
        }

        public override Order<Delivery> AddOrderFinal()
        {
            return OrdersCurrent[0];
        }
    }

    public class Courier : User
    {
        public override Order<Delivery>[] OrdersCurrent { get; set; }
        public override Order<Delivery>[] OrdersFinished { get; set; }

        public Courier(int id, string userName) : base(id, userName)
        {
        }

        public override Order<Delivery> MoveOrder(Order<Delivery> order)
        {
            OrdersFinished = new Order<Delivery>[OrdersCurrent.Length];

            return OrdersFinished[0] = AddOrderFinal();
        }

        public override Order<Delivery> AddOrderFinal()
        {
            return OrdersCurrent[0];
        }
    }

    public class Driver : User
    {
        public override Order<Delivery>[] OrdersCurrent { get; set; }
        public override Order<Delivery>[] OrdersFinished { get; set; }

        public Driver(int id, string driverName) : base(id, driverName)
        {

        }

        public override Order<Delivery> MoveOrder(Order<Delivery> order)
        {
            OrdersFinished = new Order<Delivery>[OrdersCurrent.Length];

            return OrdersFinished[0] = AddOrderFinal();
        }

        public override Order<Delivery> AddOrderFinal()
        {
            return OrdersCurrent[0];
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


