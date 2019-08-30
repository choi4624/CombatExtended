﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using UnityEngine;

namespace CombatExtended
{
    public static class ArmorUtilityCE
    {
        #region Constants

        private const float PenetrationRandVariation = 0.05f;    // Armor penetration will be randomized by +- this amount
        private const float SoftArmorMinDamageFactor = 0.2f;    // Soft body armor will always take at least original damage * this number from sharp attacks

        #endregion

        #region Properties

        private static readonly SimpleCurve dmgMultCurve = new SimpleCurve { new CurvePoint(0.5f, 0), new CurvePoint(1, 0.5f), new CurvePoint(2, 1) };    // Used to calculate the damage reduction from the penetration / armor ratio
        private static readonly StuffCategoryDef[] softStuffs = { StuffCategoryDefOf.Fabric, DefDatabase<StuffCategoryDef>.GetNamed("Leathery") };

        #endregion

        #region Methods

        /// <summary>
        /// Calculates damage through armor, depending on damage type, target and natural resistance. Also calculates deflection and adjusts damage type and impacted body part accordingly.
        /// </summary>
        /// <param name="originalDinfo">The pre-armor damage info</param>
        /// <param name="pawn">The damaged pawn</param>
        /// <param name="hitPart">The pawn's body part that has been hit</param>
        /// <param name="armorReduced">Whether sharp damage was deflected by armor</param>
        /// <param name="shieldAbsorbed">Returns true if attack did not penetrate pawn's melee shield</param>
        /// <param name="armorDeflected">Whether the attack was completely absorbed by the armor</param>
        /// <returns>If shot is deflected returns a new dinfo cloned from the original with damage amount, Def and ForceHitPart adjusted for deflection, otherwise a clone with only the damage adjusted</returns>
        public static DamageInfo GetAfterArmorDamage(DamageInfo originalDinfo, Pawn pawn, BodyPartRecord hitPart, out bool armorDeflected, out bool armorReduced, out bool shieldAbsorbed)
        {
            shieldAbsorbed = false;
            armorDeflected = false;
            armorReduced = false;

            if (originalDinfo.Def.armorCategory == null) return originalDinfo;

            var dinfo = new DamageInfo(originalDinfo);
            var dmgAmount = dinfo.Amount;
            var involveArmor = dinfo.Def.harmAllLayersUntilOutside;
            bool isAmbientDamage = dinfo.IsAmbientDamage();

            // In case of ambient damage (fire, electricity) we apply a percentage reduction formula based on the sum of all applicable armor
            if (isAmbientDamage)
            {
                dinfo.SetAmount(Mathf.CeilToInt(GetAmbientPostArmorDamage(dmgAmount, originalDinfo.Def.armorCategory.armorRatingStat, pawn, hitPart)));
                armorDeflected = dinfo.Amount <= 0;
                return dinfo;
            }

            var penAmount = originalDinfo.ArmorPenetrationInt; //GetPenetrationValue(originalDinfo);

            // Apply worn armor
            if (involveArmor && pawn.apparel != null && !pawn.apparel.WornApparel.NullOrEmpty())
            {
                var apparel = pawn.apparel.WornApparel;

                // Check for shields first
                var shield = apparel.FirstOrDefault(x => x is Apparel_Shield);
                if (shield != null)
                {
                    // Determine whether the hit is blocked by the shield
                    var blockedByShield = false;
                    if (!(dinfo.Weapon?.IsMeleeWeapon ?? false))
                    {
                        var shieldDef = shield.def.GetModExtension<ShieldDefExtension>();
                        if (shieldDef == null)
                        {
                            Log.ErrorOnce("CE :: shield " + shield.def.ToString() + " is Apparel_Shield but has no ShieldDefExtension", shield.def.GetHashCode() + 12748102);
                        }
                        else
                        {
                            var hasCoverage = shieldDef.PartIsCoveredByShield(hitPart, pawn);
                            if (hasCoverage)
                            {
                                // Right arm is vulnerable during warmup/attack/cooldown
                                blockedByShield = !((pawn.stances?.curStance as Stance_Busy)?.verb != null && hitPart.IsInGroup(CE_BodyPartGroupDefOf.RightArm));
                            }
                        }
                    }
                    // Try to penetrate the shield
                    if (blockedByShield && !TryPenetrateArmor(dinfo.Def, shield.GetStatValue(dinfo.Def.armorCategory.armorRatingStat), ref penAmount, ref dmgAmount, shield))
                    {
                        shieldAbsorbed = true;
                        armorDeflected = true;
                        dinfo.SetAmount(0);

                        // Apply secondary damage to shield
                        var props = dinfo.Weapon?.projectile as ProjectilePropertiesCE;
                        if (props != null && !props.secondaryDamage.NullOrEmpty())
                        {
                            foreach (var sec in props.secondaryDamage)
                            {
                                if (shield.Destroyed) break;
                                var secDinfo = sec.GetDinfo();
                                var pen = originalDinfo.ArmorPenetrationInt; //GetPenetrationValue(originalDinfo);
                                var dmg = (float)secDinfo.Amount;
                                TryPenetrateArmor(secDinfo.Def, shield.GetStatValue(secDinfo.Def.armorCategory.armorRatingStat), ref pen, ref dmg, shield);
                            }
                        }

                        return dinfo;
                    }
                }

                // Apparel is arranged in draw order, we run through reverse to go from Shell -> OnSkin
                for (var i = apparel.Count - 1; i >= 0; i--)
                {
                    var app = apparel[i];

                    if (app != null
                        && app.def.apparel.CoversBodyPart(hitPart)
                        && !TryPenetrateArmor(dinfo.Def, app.GetStatValue(dinfo.Def.armorCategory.armorRatingStat), ref penAmount, ref dmgAmount, app))
                    {
                        // Hit was deflected, convert damage type
                        //armorReduced = true;
                        dinfo = GetDeflectDamageInfo(dinfo, hitPart);
                        if (app == apparel.ElementAtOrDefault(i))   //Check whether the "deflecting" apparel is still in the WornApparel - if not, the next loop checks again and errors out because the index is out of range
                            i++;    // We apply this piece of apparel twice on conversion, this means we can't use deflection on Blunt or else we get an infinite loop of eternal deflection
                    }
                    if (dmgAmount <= 0)
                    {
                        dinfo.SetAmount(0);
                        armorDeflected = true;
                        return dinfo;
                    }
                }
            }

            // Apply natural armor
            var partsToHit = new List<BodyPartRecord>() { hitPart };
            if (involveArmor)
            {
                var curPart = hitPart;
                while (curPart.parent != null && curPart.depth == BodyPartDepth.Inside)
                {
                    curPart = curPart.parent;
                    partsToHit.Add(curPart);
                }
            }
            for (var i = partsToHit.Count - 1; i >= 0; i--)
            {
                var curPart = partsToHit[i];
                var coveredByArmor = curPart.IsInGroup(CE_BodyPartGroupDefOf.CoveredByNaturalArmor);
                var partArmor = pawn.GetStatValue(CE_StatDefOf.BodyPartDensity);   // How much armor is provided by sheer meat
                var unused = dmgAmount;

                // Only apply damage reduction when penetrating armored body parts
                if (coveredByArmor ? !TryPenetrateArmor(dinfo.Def, partArmor + pawn.GetStatValue(dinfo.Def.armorCategory.armorRatingStat), ref penAmount, ref dmgAmount) : !TryPenetrateArmor(dinfo.Def, partArmor, ref penAmount, ref unused))
                {
                    dinfo.SetHitPart(curPart);
                    if (coveredByArmor && pawn.RaceProps.IsMechanoid)
                    {
                        // For Mechanoid natural armor, apply deflection and blunt armor
                        dinfo = GetDeflectDamageInfo(dinfo, curPart);
                        TryPenetrateArmor(dinfo.Def, partArmor + pawn.GetStatValue(dinfo.Def.armorCategory.armorRatingStat), ref penAmount, ref dmgAmount);
                    }
                    break;
                }
                if (dmgAmount <= 0)
                {
                    dinfo.SetAmount(0);
                    armorDeflected = true;
                    return dinfo;
                }
            }

            dinfo.SetAmount(Mathf.CeilToInt(dmgAmount));
            return dinfo;
        }

        /// <summary>
        /// Calculates armor for penetrating damage types (Blunt, Sharp). Applies damage reduction based on armor penetration to armor ratio and calculates damage accordingly, with the difference being applied to the armor Thing. Also calculates whether a Sharp attack is deflected.
        /// </summary>
        /// <param name="def">The DamageDef of the attack</param>
        /// <param name="armorAmount">The amount of armor to apply</param>
        /// <param name="penAmount">How much penetration the attack still has</param>
        /// <param name="dmgAmount">The pre-armor amount of damage</param>
        /// <param name="armor">The armor apparel</param>
        /// <returns>False if the attack is deflected, true otherwise</returns>
        private static bool TryPenetrateArmor(DamageDef def, float armorAmount, ref float penAmount, ref float dmgAmount, Thing armor = null)
        {
            // Calculate deflection
            var isSharpDmg = def.armorCategory == DamageArmorCategoryDefOf.Sharp;
            var rand = UnityEngine.Random.Range(penAmount - PenetrationRandVariation, penAmount + PenetrationRandVariation);
            var deflected = isSharpDmg && armorAmount > rand;

            // Apply damage reduction
            float dmgMult = 1;
            var defCE = def.GetModExtension<DamageDefExtensionCE>() ?? new DamageDefExtensionCE();
            var noDmg = deflected && defCE.noDamageOnDeflect;
            dmgMult = noDmg ? 0 : dmgMultCurve.Evaluate(penAmount / armorAmount);

            var newDmgAmount = dmgAmount * dmgMult;
            var newPenAmount = deflected && !noDmg ? penAmount : penAmount * dmgMult;

            // Apply damage to armor
            if (armor != null)
            {
                var isSoftArmor = armor.Stuff != null && armor.Stuff.stuffProps.categories.Any(s => softStuffs.Contains(s));
                if (isSoftArmor)
                {
                    // Soft armor takes absorbed damage from sharp and no damage from blunt
                    if (isSharpDmg)
                    {
                        var armorDamage = Mathf.Max(dmgAmount * SoftArmorMinDamageFactor, dmgAmount - newDmgAmount);
                        armor.TakeDamage(new DamageInfo(def, Mathf.CeilToInt(armorDamage)));
                    }
                }
                else
                {
                    // Hard armor takes damage as reduced by damage resistance and can be almost impervious to low-penetration attacks
                    var armorDamage = Mathf.Max(1, newDmgAmount);
                    armor.TakeDamage(new DamageInfo(def, Mathf.CeilToInt(armorDamage)));
                }
            }

            dmgAmount = Mathf.Max(0, newDmgAmount);
            penAmount = Mathf.Max(0, newPenAmount);
            return !deflected;
        }

        /// <summary>
        /// Calculates damage reduction for ambient damage types (fire, electricity) versus natural and worn armor of a pawn. Adds up the total armor percentage (clamped at 0-100%) and multiplies damage by that amount.
        /// </summary>
        /// <param name="dmgAmount">The original amount of damage</param>
        /// <param name="armorRatingStat">The armor stat to use for damage reduction</param>
        /// <param name="pawn">The damaged pawn</param>
        /// <param name="part">The body part affected</param>
        /// <returns>The post-armor damage ranging from 0 to the original amount</returns>
        private static float GetAmbientPostArmorDamage(float dmgAmount, StatDef armorRatingStat, Pawn pawn, BodyPartRecord part)
        {
            var dmgMult = 1 - pawn.GetStatValue(armorRatingStat);
            if (dmgMult <= 0) return 0;
            if (pawn.apparel != null && !pawn.apparel.WornApparel.NullOrEmpty())
            {
                var apparelList = pawn.apparel.WornApparel;
                foreach (var apparel in apparelList)
                {
                    if (apparel.def.apparel.CoversBodyPart(part)) dmgMult -= apparel.GetStatValue(armorRatingStat);
                    if (dmgMult <= 0)
                    {
                        dmgMult = 0;
                        break;
                    }
                }
            }
            return dmgAmount * dmgMult;
        }

        /// <summary>
        /// Creates a new DamageInfo from a deflected one. Changes damage type to Blunt and hit part to the outermost parent of the originally hit part.
        /// </summary>
        /// <param name="dinfo">The dinfo that was deflected</param>
        /// <param name="hitPart">The originally hit part</param>
        /// <returns>DamageInfo copied from dinfo with Def and forceHitPart adjusted</returns>
        private static DamageInfo GetDeflectDamageInfo(DamageInfo dinfo, BodyPartRecord hitPart)
        {
            var newDinfo = new DamageInfo(DamageDefOf.Blunt, dinfo.Amount, 0, //Armor Penetration
                dinfo.Angle, dinfo.Instigator, GetOuterMostParent(hitPart), dinfo.Weapon);
            newDinfo.SetBodyRegion(dinfo.Height, dinfo.Depth);
            newDinfo.SetWeaponBodyPartGroup(dinfo.WeaponBodyPartGroup);
            newDinfo.SetWeaponHediff(dinfo.WeaponLinkedHediff);
            newDinfo.SetInstantPermanentInjury(dinfo.InstantPermanentInjury);
            newDinfo.SetAllowDamagePropagation(dinfo.AllowDamagePropagation);

            return newDinfo;
        }

        /// <summary>
        /// Retrieves the first parent of a body part with depth Outside
        /// </summary>
        /// <param name="part">The part to get the parent of</param>
        /// <returns>The first parent part with depth Outside, the original part if it already is Outside or doesn't have a parent, the root part if no parents are Outside</returns>
        private static BodyPartRecord GetOuterMostParent(BodyPartRecord part)
        {
            var curPart = part;
            while (curPart.parent != null && curPart.depth != BodyPartDepth.Outside)
            {
                curPart = curPart.parent;
            }
            return curPart;
        }

        /// <summary>
        /// Determines whether a dinfo is of an ambient (i.e. heat, electric) damage type and should apply percentage reduction, as opposed to deflection-based reduction
        /// </summary>
        /// <param name="dinfo"></param>
        /// <returns>True if dinfo armor category is Heat or Electric, false otherwise</returns>
        private static bool IsAmbientDamage(this DamageInfo dinfo)
        {
            return (dinfo.Def.GetModExtension<DamageDefExtensionCE>() ?? new DamageDefExtensionCE()).isAmbientDamage;
        }

        /// <summary>
        /// Applies damage to a parry object based on its armor values. For ambient damage, percentage reduction is applied, direct damage uses deflection formulas.
        /// </summary>
        /// <param name="dinfo">DamageInfo to apply to parryThing</param>
        /// <param name="parryThing">Thing taking the damage</param>
        public static void ApplyParryDamage(DamageInfo dinfo, Thing parryThing)
        {
            var pawn = parryThing as Pawn;
            if (pawn != null)
            {
                // Pawns run their own armor calculations
                dinfo.SetAmount(Mathf.CeilToInt(dinfo.Amount * Rand.Range(0f, 0.5f)));
                pawn.TakeDamage(dinfo);
            }
            else if (dinfo.IsAmbientDamage())
            {
                var dmgAmount = Mathf.CeilToInt(dinfo.Amount * Mathf.Clamp01(parryThing.GetStatValue(dinfo.Def.armorCategory.armorRatingStat)));
                dinfo.SetAmount(dmgAmount);
                parryThing.TakeDamage(dinfo);
            }
            else
            {
                var dmgAmount = dinfo.Amount * 0.1f;
                var penAmount = dinfo.ArmorPenetrationInt; //GetPenetrationValue(dinfo);
                TryPenetrateArmor(dinfo.Def, parryThing.GetStatValue(dinfo.Def.armorCategory.armorRatingStat), ref penAmount, ref dmgAmount, parryThing);
            }
        }

        #endregion
    }
}
