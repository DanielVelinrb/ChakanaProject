using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Assets.FantasyInventory.Scripts.Data;
using Assets.FantasyInventory.Scripts.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.FantasyInventory.Scripts.Interface.Elements
{
    /// <summary>
    /// Represents item when it was selected. Displays icon, name, price and properties.
    /// </summary>
    public class ItemInfoStore : MonoBehaviour
    {
        public Text Name;
        public Text Description;
        public Text Price;
        public Image Icon;

        public void Reset()
        {
            Name.text = Description.text = Price.text = null;
            Icon.sprite = ImageCollection.Instance.DefaultItemIcon;
        }

        public void Initialize(ItemId itemId, ItemParams itemParams, bool shop = false)
        {
            Icon.sprite = ImageCollection.Instance.GetIcon(itemId);
            Name.text = SplitName(itemId.ToString());
            Description.text = $"Here will be {itemId} description soon...";

            if (itemParams.Tags.Contains(ItemTag.NotForSale))
            {
                Price.text = null;
            }
            else if (shop)
            {
                // Se comenta esta parte ya que no se implementara la venta de objetos
                //Price.text = $"Buy price: {itemParams.Price}G{Environment.NewLine}Sell price: {itemParams.Price / Shop.SellRatio}G";

                Price.text = $"Buy price: {itemParams.Price}G";
            }

            // Se comenta esta parte ya que no se implementara la venta de objetos
            //else
            //{
            //    Price.text = $"Sell price: {itemParams.Price / Shop.SellRatio}G";
            //}

            var description = new List<string> {$"Type: {itemParams.Type}"};

            if (itemParams.Tags.Any())
            {
                description[description.Count - 1] += $" <color=grey>[{string.Join(", ", itemParams.Tags.Select(i => $"{i}").ToArray())}]</color>";
            }

            foreach (var attribute in itemParams.Properties)
            {
                description.Add($"{SplitName(attribute.Id.ToString())}: {attribute.Value}");
            }

            Description.text = string.Join(Environment.NewLine, description.ToArray());
        }
        
        public static string SplitName(string name)
        {
            return Regex.Replace(Regex.Replace(name, "[A-Z]", " $0"), "([a-z])([1-9])", "$1 $2").Trim();
        }
    }
}