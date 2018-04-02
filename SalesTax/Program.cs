using System;
using System.Collections.Generic;

namespace SalesTax
{
    /*
        Input 1:
        1 book at 12.49
        1 music CD at 14.99
        1 chocolate bar at 0.85
        
        Input 2:
        1 imported box of chocolates at 10.00
        1 imported bottle of perfume at 47.50   4.75    2.375
        
        Input 3:
        1 imported bottle of perfume at 27.99
        1 bottle of perfume at 18.99
        1 packet of headache pills at 9.75
        1 box of imported chocolates at 11.25
     */
    
    public static class TaxHelper
    {
        public static double RoundTax(double tax) => tax == 0 ? 0 : ((int)(tax / 0.05)) * 0.05 + 0.05;
    }
    
    public class Item
    {
        public const double BasicSalesTaxRate = 10;
        public const double ImportTaxRate = 5;

        public string Name { get; set; }

        public double Price { get; set; }

        public double SalesTax
        {
            get
            {
                if (IsTaxExempt)
                {
                    return 0;
                }

                return CalculateTax(BasicSalesTaxRate);
            }
        }

        public double ImportTax
        {
            get
            {
                if(!IsImported)
                {
                    return 0;
                }

                return CalculateTax(ImportTaxRate);
            }
        }

        public double PriceIncludingTax
        {
            get
            {
                return Price + TaxHelper.RoundTax(SalesTax + ImportTax);
            }
        }

        public bool IsTaxExempt { get; set; }

        public bool IsImported { get; set; }

        private double CalculateTax(double taxRate) => Price * taxRate / 100;
    }

    public class ItemBuilder
    {
        private Item item;

        public ItemBuilder(string name, double price)
        {
            item = new Item
            {
                Name = name,
                Price = price
            };
        }

        public ItemBuilder OfType(string itemType)
        {
            switch (itemType)
            {
                case "books":
                case "food":
                case "medicine":
                    item.IsTaxExempt = true;
                    break;
            }

            return this;
        }

        public ItemBuilder Imported()
        {
            item.IsImported = true;

            return this;
        }

        public Item Build()
        {
            return item;
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            List<Item> receipt1 = new List<Item>
            {
                new ItemBuilder("book", 12.49)
                    .OfType("books")
                    .Build(),

                new ItemBuilder("music CD", 14.99)
                    .OfType("music")
                    .Build(),

                new ItemBuilder("chocolate bar", 0.85)
                    .OfType("food")
                    .Build()
            };

            List<Item> receipt2 = new List<Item>
            {
                new ItemBuilder("imported box of chocolates", 10.00)
                    .OfType("food")
                    .Imported()
                    .Build(),

                new ItemBuilder("imported bottle of perfume", 47.50)
                    .OfType("cosmetics")
                    .Imported()
                    .Build()
            };

            List<Item> receipt3 = new List<Item>
            {
                new ItemBuilder("imported bottle of perfume", 27.99)
                    .OfType("cosmetics")
                    .Imported()
                    .Build(),

                new ItemBuilder("bottle of perfume", 18.99)
                    .OfType("cosmetics")
                    .Build(),

                new ItemBuilder("packet of headache pills", 9.75)
                    .OfType("medicine")
                    .Build(),

                new ItemBuilder("box of imported chocolates", 11.25)
                    .OfType("food")
                    .Imported()
                    .Build()
            };

            DisplayReceipt(receipt1);
            DisplayReceipt(receipt2);
            DisplayReceipt(receipt3);

            Console.ReadKey();
        }

        private static void DisplayReceipt(IEnumerable<Item> items)
        {
            double salesTax = 0;
            double total = 0;

            foreach (Item item in items)
            {
                salesTax += item.SalesTax;
                total += item.PriceIncludingTax;

                Console.WriteLine($"{ item.Name }: { item.PriceIncludingTax.ToString("0.00") }");
            }

            Console.WriteLine($"Sales Tax: { TaxHelper.RoundTax(salesTax).ToString("0.00") }");
            Console.WriteLine($"Total: { total.ToString("0.00") }");
        }
    }
}
