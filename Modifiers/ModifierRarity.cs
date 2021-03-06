﻿using System;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace Loot.Modifiers
{
	/// <summary>
	/// Defines the rarity of a modifier
	/// </summary>
	public abstract class ModifierRarity : ICloneable
	{
		public Mod Mod { get; internal set; }
		public uint Type { get; internal set; }

		public virtual string Name => GetType().Name;
		public virtual Color? OverrideNameColor => null;
		public virtual string ItemPrefix => null;
		public virtual string ItemSuffix => null;

		public abstract float RequiredRarityLevel { get; }
		public abstract Color Color { get; }

		public virtual bool MatchesRequirements(Modifier modifier)
			=> modifier.MatchesRarity(this);

		public override string ToString()
			=> EMMUtils.JSLog(typeof(ModifierRarity), this);

		public virtual void Clone(ref ModifierRarity clone)
		{

		}

		public object Clone()
		{
			ModifierRarity clone = (ModifierRarity)MemberwiseClone();
			clone.Mod = Mod;
			clone.Type = Type;
			Clone(ref clone);
			return clone;
		}

		/// <summary>
		/// Allows modder to do custom loading here
		/// Use the given TC to pull data you saved using <see cref="Save(TagCompound)"/>
		/// </summary>
		/// <param name="tag"></param>
		public virtual void Load(TagCompound tag)
		{

		}

		/// <summary>
		/// Allows modder to do custom saving here
		/// Use the given TC to put data you want to save, which can be loaded using <see cref="Load(TagCompound)"/>
		/// </summary>
		/// <param name="tag"></param>
		public virtual void Save(TagCompound tag)
		{

		}

		protected internal static ModifierRarity _Load(TagCompound tag)
		{
			string modname = tag.GetString("ModName");
			Assembly assembly;
			if (EMMLoader.Mods.TryGetValue(modname, out assembly))
			{
				ModifierRarity r = (ModifierRarity)Activator.CreateInstance(assembly.GetType(tag.GetString("Type")));
				r.Type = tag.Get<uint>("RarityType");
				r.Mod = ModLoader.GetMod(modname);
				r.Load(tag);
				return r;
			}
			throw new Exception($"ModifierRarity load error for {modname}");
		}

		protected internal static TagCompound Save(ModifierRarity rarity)
		{
			var tag = new TagCompound
			{
				{"Type", rarity.GetType().FullName },
				{"RarityType", rarity.Type },
				{"ModName", rarity.Mod.Name },
			};
			rarity.Save(tag);
			return tag;
		}
	}
}
