using System.Collections.Generic;
using Assets.FantasyInventory.Scripts.Data;
using Assets.FantasyInventory.Scripts.Enums;

namespace Assets.FantasyInventory.Scripts.GameData
{
    /// <summary>
    /// Item params are stored here. If you want to store them in any kind of database, please refer to dictionary serialization:
    /// https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.html
    /// Alternatively, use Newtonsoft JSON (recommended).
    /// </summary>
    public class Items
    {
        public static readonly Dictionary<ItemId, ItemParams> Params = new Dictionary<ItemId, ItemParams>
        {
            {
                ItemId.SupayMask,
                new ItemParams
                {
                    Type = ItemType.Mask,
                    Properties = new List<Property> { new Property(PropertyId.MagicDamage, 100) },
                    Price = 1000
                }
            },
            {
                ItemId.Flute,
                new ItemParams
                {
                    Type = ItemType.Loot,
                    Price = 10
                }
            },
            {
                ItemId.GoldPieces,
                new ItemParams
                {
                    Type = ItemType.Currency,
                    //Tags = new List<ItemTag> { ItemTag.NotForSale }
                }
            },
            {
                ItemId.KunkaKuchuna,
                new ItemParams
                {
                    Type = ItemType.Weapon,
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage, 100) },
                    Price = 200
                }
            },
            {
                ItemId.Sword,
                new ItemParams
                {
                    Type = ItemType.Weapon,
                    Tags = new List<ItemTag> { ItemTag.Sword },
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage, 10) },
                    Price = 1000
                }
            },
            {
                ItemId.Bow,
                new ItemParams
                {
                    Type = ItemType.Weapon,
                    Tags = new List<ItemTag> { ItemTag.Bow, ItemTag.TwoHanded },
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage, 15) },
                    Price = 2000
                }
            },
            {
                ItemId.KunturMask,
                new ItemParams
                {
                    Type = ItemType.Mask,
                    Properties = new List<Property> { new Property(PropertyId.MagicDamage, 10) },
                    Price = 1000
                }
            },
            {
                ItemId.AyahuascaRoot,
                new ItemParams
                {
                    Type = ItemType.Potion,
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage, 5) },
                    Price = 500
                }
            },
            {
                ItemId.LuminousMushroom,
                new ItemParams
                {
                    Type = ItemType.Potion,
                    Properties = new List<Property> { new Property(PropertyId.RestoreMana, 50) },
                    Price = 200
                }
            },
            {
                ItemId.PachamamaAmulet,
                new ItemParams
                {
                    Type = ItemType.Amulet,
                    Properties = new List<Property> { new Property(PropertyId.RestoreHealth, 50) },
                    Price = 1000
                }
            },
            {
                ItemId.SilverRing,
                new ItemParams
                {
                    Type = ItemType.Ring,
                    Properties = new List<Property> { new Property(PropertyId.MagicDefense, 5) },
                    Price = 500
                }
            },
            {
                ItemId.Spear,
                new ItemParams
                {
                    Type = ItemType.Weapon,
                    Tags = new List<ItemTag> { ItemTag.Spear, ItemTag.TwoHanded },
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage, 15) },
                    Price = 2500
                }
            },
            {
                ItemId.StoneAmulet,
                new ItemParams
                {
                    Type = ItemType.Necklace,
                    Properties = new List<Property> { new Property(PropertyId.MagicDefense, 10) },
                    Price = 1000
                }
            },
            {
                ItemId.TwoHandedSword,
                new ItemParams
                {
                    Type = ItemType.Weapon,
                    Tags = new List<ItemTag> { ItemTag.Sword, ItemTag.TwoHanded },
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage, 20) },
                    Price = 5000
                }
            },
            {
                ItemId.WarriorTearAmulet,
                new ItemParams
                {
                    Type = ItemType.Amulet,
                    //Tags = new List<ItemTag> { ItemTag.Axe },
                    Properties = new List<Property> { new Property(PropertyId.PhysicDamage, 25) },
                    Price = 100
                }
            }
        };
    }
}