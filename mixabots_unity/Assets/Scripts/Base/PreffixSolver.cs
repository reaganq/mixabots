using UnityEngine;
using System.Collections;

public class PreffixSolver  {

 public static bool GiveItem(PreffixType preffix, int ID, int amount)
 {
     string uniqueId = preffix.ToString() + ID.ToString();
     switch(preffix)
     {
         case PreffixType.ARMOR:
             RPGArmor armor = Storage.LoadbyUniqueId<RPGArmor>(uniqueId, new RPGArmor());
             //armor.CurrentDurability = armor.Durability;
            if(armor.EquipmentSlotIndex == EquipmentSlots.Head)
                return Player.Instance.Hero.HeadInventory.AddItem(armor, amount);
            if(armor.EquipmentSlotIndex == EquipmentSlots.Body)
                return Player.Instance.Hero.BodyInventory.AddItem(armor, amount);
            if(armor.EquipmentSlotIndex == EquipmentSlots.ArmL)
                return Player.Instance.Hero.ArmLInventory.AddItem(armor, amount);
            if(armor.EquipmentSlotIndex == EquipmentSlots.ArmR)
                return Player.Instance.Hero.ArmRInventory.AddItem(armor, amount);
            if(armor.EquipmentSlotIndex == EquipmentSlots.Legs)
                return Player.Instance.Hero.LegsInventory.AddItem(armor, amount);
            break;
         case PreffixType.ITEM:
             RPGItem item = Storage.LoadbyUniqueId<RPGItem>(uniqueId, new RPGItem());
            return Player.Instance.Hero.Inventory.AddItem(item, amount);
            //break;
         /*case PreffixType.SKILL:
             if (amount == -1)
             {
                 //add new skill
                 RPGSkill skill = Storage.LoadbyUniqueId<RPGSkill>(uniqueId, new RPGSkill());
                 skill.Value = 1;
                 player.Hero.Skills.Add(skill);
             }
             else
             {
                 foreach(RPGSkill skill in player.Hero.Skills)
                 {
                     if (skill.UniqueId == uniqueId)
                     {
                         if (skill.Value + amount > skill.Maximum)
                             skill.Value = skill.Maximum;
                         else
                             skill.Value += amount;
                     }
                 }
             }
         
             break;
         case PreffixType.SKILLPOINT:
             player.Hero.SkillPoint += amount;
             break;
         case PreffixType.QUEST:
             player.Hero.Quest.StartQuest(ID);
             break;
         case PreffixType.ATTRIBUTE:
             if (amount == -1)
             {
                 //add new skill
                 RPGAttribute attr = Storage.LoadbyUniqueId<RPGAttribute>(uniqueId, new RPGAttribute());
                 attr.Value = 1;
                 player.Hero.Attributes.Add(attr);
             }
             else
             {
                 foreach(RPGAttribute atr in player.Hero.Attributes)
                 {
                     if (atr.UniqueId == uniqueId)
                     {
                         if (atr.Value + amount > atr.Maximum)
                             atr.Value = atr.Maximum;
                         else
                             atr.Value += amount;
                     }
                 }
             }
             break;
         case PreffixType.ATTRIBUTEPOINT:
             player.Hero.AttributePoint += amount;
             break;  
         case PreffixType.XP:
             player.Hero.AddXp(amount);
             break;
         case PreffixType.XPPERCENT:
             player.Hero.AddXpPercent(amount);           
             break;
         case PreffixType.REPUTATION:
             player.Hero.AddReputation(ID, amount);
             break;
             */
     }
     return true;
 }
}

public enum PreffixType
{
 QUEST = 0,
 PARAGRAPH = 1,
 LINETEXT = 2,
 ITEM = 3,
 SKILLPOINT = 4,
 SKILL = 5,
 ATTRIBUTEPOINT = 6,
 ATTRIBUTE = 7,
 NPC = 8,
 WEAPON = 9,
 ARMOR = 10,
 XP = 11,
 XPPERCENT = 12,
 ENEMY = 13,
 SPELL = 14,
 WORLDOBJECT = 15,
 REPUTATION = 16
}
