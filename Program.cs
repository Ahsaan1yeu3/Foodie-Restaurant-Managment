using System;
using System.Collections.Generic;

// Factory Pattern
public abstract class MenuItemFactory
{
    public abstract MenuItem CreateMenuItem();
}

public class PizzaFactory : MenuItemFactory
{
    public override MenuItem CreateMenuItem()
    {
        return new Pizza();
    }
}

public class PastaFactory : MenuItemFactory
{
    public override MenuItem CreateMenuItem()
    {
        return new Pasta();
    }
}

// Composite Pattern
public abstract class MenuItem
{
    public abstract void Display();
    public abstract double GetPrice();
}

public class Pizza : MenuItem
{
    public override void Display()
    {
        Console.WriteLine("Pizza - $10.99");
    }

    public override double GetPrice()
    {
        return 10.99;
    }
}

public class Pasta : MenuItem
{
    public override void Display()
    {
        Console.WriteLine("Pasta - $8.99");
    }

    public override double GetPrice()
    {
        return 8.99;
    }
}

// Strategy Pattern
public interface IPaymentStrategy
{
    void Pay(double amount);
}

public class CashPayment : IPaymentStrategy
{
    public void Pay(double amount)
    {
        Console.WriteLine($"Paid ${amount} by cash.");
    }
}

public class CreditCardPayment : IPaymentStrategy
{
    public void Pay(double amount)
    {
        Console.WriteLine($"Paid ${amount} by credit card.");
    }
}

// Builder Pattern
public class OrderBuilder
{
    private List<MenuItem> items = new List<MenuItem>();

    public void AddItem(MenuItem item)
    {
        items.Add(item);
    }

    public double CalculateTotal()
    {
        double total = 0;
        foreach (var item in items)
        {
            total += item.GetPrice();
        }
        return total;
    }
}

// Decorator Pattern
public abstract class ToppingDecorator : MenuItem
{
    protected MenuItem menuItem;

    public ToppingDecorator(MenuItem menuItem)
    {
        this.menuItem = menuItem;
    }

    public override void Display()
    {
        menuItem.Display();
    }

    public override double GetPrice()
    {
        return menuItem.GetPrice();
    }
}

public class CheeseTopping : ToppingDecorator
{
    public CheeseTopping(MenuItem menuItem) : base(menuItem) { }

    public override void Display()
    {
        base.Display();
        Console.WriteLine(" + Cheese");
    }

    public override double GetPrice()
    {
        return menuItem.GetPrice() + 1.50; // Additional price for cheese topping
    }
}

// Observer Pattern
public interface IOrderObserver
{
    void Update(Order order);
}

public class Chef : IOrderObserver
{
    public void Update(Order order)
    {
        Console.WriteLine("Chef: New order received.");
        // Process the order
    }
}

public class Order
{
    private List<IOrderObserver> observers = new List<IOrderObserver>();

    public void Attach(IOrderObserver observer)
    {
        observers.Add(observer);
    }

    public void Notify()
    {
        foreach (var observer in observers)
        {
            observer.Update(this);
        }
    }

    // Other order functionalities
    // ...
}

// Implementation
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Welcome to the Restaurant!");

        // Factory Pattern
        MenuItemFactory pizzaFactory = new PizzaFactory();
        MenuItemFactory pastaFactory = new PastaFactory();

        // Strategy Pattern
        IPaymentStrategy paymentStrategy = null;

        // Builder Pattern
        OrderBuilder orderBuilder = new OrderBuilder();

        // Observer Pattern
        Chef chef = new Chef();
        Order order = new Order();
        order.Attach(chef);

        while (true)
        {
            Console.WriteLine("\nChoose an option:");
            Console.WriteLine("1. Display Menu");
            Console.WriteLine("2. Add Item to Order");
            Console.WriteLine("3. Make Payment");
            Console.WriteLine("4. Exit");

            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    Console.WriteLine("Menu Items:");
                    MenuItem pizza = pizzaFactory.CreateMenuItem();
                    MenuItem pasta = pastaFactory.CreateMenuItem();

                    // Enhance the pizza with toppings
                    Console.WriteLine("Do you want to add extra cheese to the pizza? (Y/N):");
                    string addCheese = Console.ReadLine();
                    if (addCheese?.ToUpper() == "Y")
                    {
                        pizza = new CheeseTopping((Pizza)pizza);
                    }

                    pizza.Display();
                    pasta.Display();
                    break;

                case 2:
                    Console.WriteLine("Enter item number to add (1 for Pizza, 2 for Pasta):");
                    int itemNumber;
                    if (!int.TryParse(Console.ReadLine(), out itemNumber))
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                        continue;
                    }

                    if (itemNumber == 1)
                    {
                        orderBuilder.AddItem(pizzaFactory.CreateMenuItem());
                        Console.WriteLine("Pizza added to order.");
                    }
                    else if (itemNumber == 2)
                    {
                        orderBuilder.AddItem(pastaFactory.CreateMenuItem());
                        Console.WriteLine("Pasta added to order.");
                    }
                    else
                    {
                        Console.WriteLine("Invalid item number.");
                    }
                    break;

                case 3:
                    if (orderBuilder.CalculateTotal() == 0)
                    {
                        Console.WriteLine("Please add items to the order first.");
                        continue;
                    }

                    Console.WriteLine("Select payment method:");
                    Console.WriteLine("1. Cash Payment");
                    Console.WriteLine("2. Credit Card Payment");

                    int paymentMethodChoice;
                    if (!int.TryParse(Console.ReadLine(), out paymentMethodChoice))
                    {
                        Console.WriteLine("Invalid input. Please enter a number.");
                        continue;
                    }

                    switch (paymentMethodChoice)
                    {
                        case 1:
                            paymentStrategy = new CashPayment();
                            break;
                        case 2:
                            paymentStrategy = new CreditCardPayment();
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Using default payment method (Cash).");
                            paymentStrategy = new CashPayment();
                            break;
                    }

                    double totalAmount = orderBuilder.CalculateTotal();
                    Console.WriteLine($"Total Amount: ${totalAmount}");
                    paymentStrategy.Pay(totalAmount);
                    break;

                case 4:
                    Console.WriteLine("Exiting program. Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }
        }
    }
}