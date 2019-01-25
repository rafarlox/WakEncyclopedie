using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WakEncyclopedie.DAO;

namespace WakEncyclopedie.BO
{
    public class Skill
    {
        #region skillStats_constants
        private const string HP_IN_PERCENT_STRING = "% Points de vie";
        private const string RES_ALL_STRING = "Résistance Élémentaire";
        private const string BARRIER_STRING = "Barrière";
        private const string HP_RECEIVED_IN_PERCENT_STRING = "% Soins reçus";
        private const string HP_IN_ARMOR_IN_PERCENT_STRING = "% Points de Vie en Amure";
        private const string MASTERY_ALL_STRING = "Maîtrise Élémentaire";
        private const string ST_MASTERY_STRING = "Maîtrise Monocible";
        private const string AREA_MASTERY_STRING = "Maîtrise Zone";
        private const string MELEE_MASTERY_STRING = "Maîtrise Mêlée";
        private const string DISTANCE_MASTERY_STRING = "Maîtrise Distance";
        private const string LOCK_STRING = "Tacle";
        private const string DODGE_STRING = "Esquive";
        private const string INITIATIVE_STRING = "Initiative";
        private const string LOCK_AND_DODGE_STRING = "Tacle et Esquive";
        private const string WILL_STRING = "Volonté";
        private const string CC_STRING = "% Coup Critique";
        private const string BLOCK_STRING = "% Parade";
        private const string CRITICAL_MASTERY_STRING = "Maîtrise Critique";
        private const string REAR_MASTERY_STRING = "Maîtrise Dos";
        private const string BERSERK_MASTERY_STRING = "Maîtrise Berserk";
        private const string HEALING_MASTERY_STRING = "Maîtrise Soin";
        private const string REAR_RESISTANCE_STRING = "Résistance Dos";
        private const string CRIT_RESISTANCE_STRING = "Résistance Critique";
        private const string AP_STRING = "Point d'Action";
        private const string MP_AND_DAMMAGES_STRING = "Point de Mouvement et dégâts";
        private const string RANGE_AND_DAMMAGE_STRING = "Portée et dégâts";
        private const string WP_STRING = "Point de Wakfu";
        private const string CONTROL_AND_DAMMAGE_STRING = "Contrôle et dégâts";

        private const int HP_IN_PERCENT_VALUE = 4;
        private const int RES_ALL_VALUE = 10;
        private const int BARRIER_VALUE = 1;
        private const int HP_RECEIVED_IN_PERCENT_VALUE = 6;
        private const int HP_IN_ARMOR_IN_PERCENT_VALUE = 4;
        private const int MASTERY_ALL_VALUE = 5;
        private const int MASTERY_ELEM_VALUE = 8;
        private const int HEALTH_VALUE = 20;
        private const int LOCK_OR_DODGE_VALUE = 6;
        private const int LOCK_AND_DODGE_VALUE = 4;
        private const int INITIATIVE_VALUE = 4;
        private const int WILL_VALUE = 1;
        private const int CC_OR_BLOCK_VALUE = 1;
        private const int CRIT_MASTERY_VALUE = 4;
        private const int REAR_OR_HEALING_MASTERY_VALUE = 6;
        private const int BERSERK_MASTERY_VALUE = 8;
        private const int REAR_OR_CRIT_RESISTANCE_VALUE = 4;
        private const int AP_MP_RANGE_VALUE = 1;
        private const int DAMMAGE_FOR_MP_VALUE = 20;
        private const int DAMMAGE_FOR_RANGE_OR_CONTROL_VALUE = 40;
        private const int WP_OR_CONTROL_VALUE = 2;
        private const int DAMMAGE_INFLIGED_IN_PERCENT_VALUE = 10;
        private const int MAJOR_RES_ALL_VALUE = 50;

        private const int SKILL_STATS_INFINITE_POINTS = 50;
        private const int RES_ALL_MAX_POINTS = 10;
        private const int BARRIER_MAX_POINTS = 10;
        private const int HP_RECEIVED_IN_PERCENT_MAX_POINTS = 5;
        private const int HP_IN_ARMOR_IN_PERCENT_MAX_POINTS = 10;
        private const int MASTERY_ELEM_MAX_POINTS = 20;
        private const int INITIATIVE_MAX_POINTS = 20;
        private const int WILL_MAX_POINTS = 20;
        private const int CC_OR_BLOCK_MAX_POINTS = 20;
        private const int REAR_OR_CRIT_RESISTANCE_MAX_POINTS = 20;
        private const int MAJOR_MAX_POINTS = 1;
        #endregion skillStats_constants

        private const int FIRST_LEVEL_FOR_SKILLS = 2;
        private const double LEVEL_INTERVAL_FOR_SKILLS = 4.0;
        private const double LEVEL_INTERVAL_FOR_MAJOR_SKILLS = 50.0;
        private const int CORRECTION_FOR_SKILLS = 1; // Value to decrement for the formula
        private const int LEVEL_AFTER_FOR_STREGTH = 1;
        private const int LEVEL_AFTER_FOR_AGILITY = 2;
        private const int LEVEL_AFTER_FOR_LUCK = 3;

        public Build ActualBuild { get; set; }

        public List<SkillsStat> Intelligence { get; set; } = new List<SkillsStat>();
        public List<SkillsStat> Strength { get; set; } = new List<SkillsStat>();
        public List<SkillsStat> Agility { get; set; } = new List<SkillsStat>();
        public List<SkillsStat> Luck { get; set; } = new List<SkillsStat>();
        public List<SkillsStat> Major { get; set; } = new List<SkillsStat>();

        public int MaxIntelligencePointsRemaining { get; private set; }
        public int MaxStrengthPointsRemaining { get; private set; }
        public int MaxAgilityPointsRemaining { get; private set; }
        public int MaxLuckPointsRemaining { get; private set; }
        public int MaxMajorPointsRemaining { get; private set; }


        public int IntelligencePointsRemaining { get; private set; }
        public int StrengthPointsRemaining { get; private set; }
        public int AgilityPointsRemaining { get; private set; }
        public int LuckPointsRemaining { get; private set; }
        public int MajorPointsRemaining { get; private set; }

        public Skill() {

        }

        public Skill(Build build) {
            ActualBuild = build;
            ResetSkills();
        }

        public List<List<SkillsStat>> GetAllListOfSkillsStat() {
            return new List<List<SkillsStat>>() {
                Intelligence,
                Strength,
                Agility,
                Luck,
                Major,
            };
        }

        public void ResetSkills() {
            Intelligence = new List<SkillsStat>() {
                new SkillsStat(GlobalConstants.HEALTH_POINTS_IN_PERCENT_ID, HP_IN_PERCENT_STRING, HP_IN_PERCENT_VALUE, SKILL_STATS_INFINITE_POINTS),
                new SkillsStat(GlobalConstants.RESISTANCE_ALL_ID, RES_ALL_STRING, RES_ALL_VALUE, RES_ALL_MAX_POINTS),
                new SkillsStat(GlobalConstants.BARRIER_ID, BARRIER_STRING, BARRIER_VALUE, BARRIER_MAX_POINTS),
                new SkillsStat(GlobalConstants.HEALTH_RECEIVED_IN_PERCENT_ID, HP_RECEIVED_IN_PERCENT_STRING, HP_RECEIVED_IN_PERCENT_VALUE, HP_RECEIVED_IN_PERCENT_MAX_POINTS),
                new SkillsStat(GlobalConstants.HEALTH_POINTS_IN_ARMOR_IN_PERCENT_ID, HP_IN_ARMOR_IN_PERCENT_STRING, HP_IN_ARMOR_IN_PERCENT_VALUE, HP_IN_ARMOR_IN_PERCENT_MAX_POINTS),
            };
            Strength = new List<SkillsStat>() {
                new SkillsStat(GlobalConstants.MASTERY_ALL_ID, MASTERY_ALL_STRING, MASTERY_ALL_VALUE, SKILL_STATS_INFINITE_POINTS),
                new SkillsStat(GlobalConstants.SINGLE_TARGET_MASTERY_ID, ST_MASTERY_STRING, MASTERY_ELEM_VALUE, MASTERY_ELEM_MAX_POINTS),
                new SkillsStat(GlobalConstants.AREA_MASTERY_ID, AREA_MASTERY_STRING, MASTERY_ELEM_VALUE, MASTERY_ELEM_MAX_POINTS),
                new SkillsStat(GlobalConstants.MELEE_MASTERY_ID, MELEE_MASTERY_STRING, MASTERY_ELEM_VALUE, MASTERY_ELEM_MAX_POINTS),
                new SkillsStat(GlobalConstants.DISTANCE_MASTERY_ID, DISTANCE_MASTERY_STRING, MASTERY_ELEM_VALUE, MASTERY_ELEM_MAX_POINTS),
                new SkillsStat(GlobalConstants.HEALTH_POINTS_ID, GlobalConstants.HEALTH_STRING, HEALTH_VALUE, SKILL_STATS_INFINITE_POINTS),
            };
            Agility = new List<SkillsStat>() {
                new SkillsStat(GlobalConstants.LOCK_ID, LOCK_STRING, LOCK_OR_DODGE_VALUE, SKILL_STATS_INFINITE_POINTS),
                new SkillsStat(GlobalConstants.DODGE_ID, DODGE_STRING, LOCK_OR_DODGE_VALUE, SKILL_STATS_INFINITE_POINTS),
                new SkillsStat(GlobalConstants.INITIATIVE_ID, INITIATIVE_STRING, INITIATIVE_VALUE, INITIATIVE_MAX_POINTS),
                new SkillsStat(GlobalConstants.LOCK_AND_DODGE_ID, LOCK_AND_DODGE_STRING, LOCK_AND_DODGE_VALUE, SKILL_STATS_INFINITE_POINTS),
                new SkillsStat(GlobalConstants.WILL_ID, WILL_STRING, WILL_VALUE, WILL_MAX_POINTS),
            };
            Luck = new List<SkillsStat>() {
                new SkillsStat(GlobalConstants.CRITICAL_HITS_ID, CC_STRING, CC_OR_BLOCK_VALUE, CC_OR_BLOCK_MAX_POINTS),
                new SkillsStat(GlobalConstants.BLOCK_ID, BLOCK_STRING, CC_OR_BLOCK_VALUE, CC_OR_BLOCK_MAX_POINTS),
                new SkillsStat(GlobalConstants.CRITICAL_MASTERY_ID, CRITICAL_MASTERY_STRING, CRIT_MASTERY_VALUE, SKILL_STATS_INFINITE_POINTS),
                new SkillsStat(GlobalConstants.REAR_MASTERY_ID, REAR_MASTERY_STRING, REAR_OR_HEALING_MASTERY_VALUE, SKILL_STATS_INFINITE_POINTS),
                new SkillsStat(GlobalConstants.BERSERK_MASTERY_ID, BERSERK_MASTERY_STRING, BERSERK_MASTERY_VALUE, SKILL_STATS_INFINITE_POINTS),
                new SkillsStat(GlobalConstants.HEALING_MASTERY_ID, HEALING_MASTERY_STRING, REAR_OR_HEALING_MASTERY_VALUE, SKILL_STATS_INFINITE_POINTS),
                new SkillsStat(GlobalConstants.CRITICAL_RESISTANCE_ID, CRIT_RESISTANCE_STRING, REAR_OR_CRIT_RESISTANCE_VALUE, REAR_OR_CRIT_RESISTANCE_MAX_POINTS),
                new SkillsStat(GlobalConstants.REAR_RESISTANCE_ID, REAR_RESISTANCE_STRING, REAR_OR_CRIT_RESISTANCE_VALUE, REAR_OR_CRIT_RESISTANCE_MAX_POINTS),
            };
            Major = new List<SkillsStat>() {
                new SkillsStat(GlobalConstants.ACTION_POINT_ID, AP_STRING, AP_MP_RANGE_VALUE, MAJOR_MAX_POINTS),
                new SkillsStat(GlobalConstants.MP_AND_DAMMAGE_ID, MP_AND_DAMMAGES_STRING, DAMMAGE_FOR_MP_VALUE, MAJOR_MAX_POINTS),
                new SkillsStat(GlobalConstants.RANGE_AND_DAMMAGE_ID, RANGE_AND_DAMMAGE_STRING, DAMMAGE_FOR_RANGE_OR_CONTROL_VALUE, MAJOR_MAX_POINTS),
                new SkillsStat(GlobalConstants.WAKFU_POINT_ID, WP_STRING, WP_OR_CONTROL_VALUE, MAJOR_MAX_POINTS),
                new SkillsStat(GlobalConstants.CONTROL_AND_DAMMAGE_ID, CONTROL_AND_DAMMAGE_STRING, DAMMAGE_FOR_RANGE_OR_CONTROL_VALUE, MAJOR_MAX_POINTS),
                new SkillsStat(GlobalConstants.DAMMAGE_INFLIGED_ID, GlobalConstants.DAMMAGE_INFLIGED_STRING, DAMMAGE_INFLIGED_IN_PERCENT_VALUE, MAJOR_MAX_POINTS),
                new SkillsStat(GlobalConstants.RES_ALL_FOR_MAJOR_ID, RES_ALL_STRING, MAJOR_RES_ALL_VALUE, MAJOR_MAX_POINTS),
            };
            CalculatePointsToDistributed();
            ActualBuild.BStats.CalculateBuildStats();
        }

        /// <summary>
        /// Round a double value with a custom midpointrounding to zero.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private double RoundToZero(double value) {
            // This method is necessary when we have a value like 19.5 : Math.Round will round this value at 20 and not a 19...
            double decimalPart = value - Math.Truncate(value);
            if (decimalPart == 0.5) {
                value -= decimalPart;
            }
            value = Math.Round(value);
            return value;
        }

        private void CalculatePointsToDistributed() {
            if (ActualBuild.LevelBuild >= FIRST_LEVEL_FOR_SKILLS) {
                MaxIntelligencePointsRemaining = (int)Math.Round(ActualBuild.LevelBuild / LEVEL_INTERVAL_FOR_SKILLS, MidpointRounding.AwayFromZero);
                MaxStrengthPointsRemaining = (int)Math.Floor((ActualBuild.LevelBuild + LEVEL_AFTER_FOR_STREGTH) / LEVEL_INTERVAL_FOR_SKILLS);
                MaxAgilityPointsRemaining = (int)(Math.Round((ActualBuild.LevelBuild + LEVEL_AFTER_FOR_AGILITY) / LEVEL_INTERVAL_FOR_SKILLS, MidpointRounding.AwayFromZero) - CORRECTION_FOR_SKILLS);
                MaxLuckPointsRemaining = (int)(Math.Round(RoundToZero((ActualBuild.LevelBuild + LEVEL_AFTER_FOR_LUCK) / LEVEL_INTERVAL_FOR_SKILLS)) - CORRECTION_FOR_SKILLS);
                MaxMajorPointsRemaining = (int)Math.Round(ActualBuild.LevelBuild / LEVEL_INTERVAL_FOR_MAJOR_SKILLS, MidpointRounding.AwayFromZero);
            } else {
                MaxIntelligencePointsRemaining = 0;
                MaxStrengthPointsRemaining = 0;
                MaxAgilityPointsRemaining = 0;
                MaxLuckPointsRemaining = 0;
                MaxMajorPointsRemaining = 0;
            }
            IntelligencePointsRemaining = MaxIntelligencePointsRemaining;
            StrengthPointsRemaining = MaxStrengthPointsRemaining;
            AgilityPointsRemaining = MaxAgilityPointsRemaining;
            LuckPointsRemaining = MaxLuckPointsRemaining;
            MajorPointsRemaining = MaxMajorPointsRemaining;
        }

        /// <summary>
        /// Add or remove points to a skillstat contained in the list.
        /// </summary>
        /// <param name="listSS">The list who contains the skillstat that we want to assign points</param>
        /// <param name="idStat">The id of the stat that we will assign points</param>
        /// <param name="pointsRemaining"The remainings points available for the actual skill "branch"></param>
        /// <param name="pointsToAssign">The number of points we want to assign. Must be negative to remove points</param>
        /// <returns>Return the points remaining for the skill "branch" after the assignation</returns>
        private int AssignPointsToSkillsStat(List<SkillsStat> listSS, int idStat, int pointsRemaining, int pointsToAssign) {
            if (pointsToAssign != 0) {
                SkillsStat ss = listSS.Find(x => x.Id == idStat);
                if (ss.Id != 0) {
                    int pointsToAssignBefore = pointsToAssign;
                    int remainingsPoints;
                    if (pointsToAssign > 0)
                        remainingsPoints = ss.AssignPoints(pointsToAssign);
                    else
                        remainingsPoints = -ss.RemovePoints(-pointsToAssign); // we must inverse the value for removing it
                    return pointsToAssignBefore - remainingsPoints;
                } else {
                    Console.WriteLine("Unknown id");
                    return pointsRemaining;
                }
            } else {
                return pointsRemaining;
            }
        }

        /// <summary>
        /// Add or remove points for a skillstat in a skill "branch"
        /// </summary>
        /// <param name="listSS">The list who contains the skillstat that we want to assign points</param>
        /// <param name="maxSkillPointsRemaining">The max points allowed for the actual skill "branch"</param>
        /// <param name="skillPointsRemaining">The remainings points available for the actual skill "branch"</param>
        /// <param name="idStat">The id of the stat that we will assign points</param>
        /// <param name="pointsToAssign">The number of points we want to assign. Must be negative to remove points</param>
        /// <returns>Return the points remaining for the skill "branch" after the assignation</returns>
        private int AssignPoints(List<SkillsStat> listSS, int maxSkillPointsRemaining, int skillPointsRemaining, int idStat, int pointsToAssign) {
            // We won't add points if the skill branch don't have remanings points
            if ((pointsToAssign > 0 && skillPointsRemaining > 0) || pointsToAssign < 0) {
                // if we try to assign more points that remains, then we just assign what's available
                if (pointsToAssign > skillPointsRemaining) {
                    pointsToAssign = skillPointsRemaining;
                }
                skillPointsRemaining -= AssignPointsToSkillsStat(listSS, idStat, skillPointsRemaining, pointsToAssign);
            }
            ActualBuild.BStats.CalculateBuildStats();
            return skillPointsRemaining;
        }

        public void AssignPointsInIntelligence(int idStat, int pointsToAssign) {
            IntelligencePointsRemaining = AssignPoints(Intelligence, MaxIntelligencePointsRemaining, IntelligencePointsRemaining, idStat, pointsToAssign);
        }

        public void AssignPointsInStrength(int idStat, int pointsToAssign) {
            StrengthPointsRemaining = AssignPoints(Strength, MaxStrengthPointsRemaining, StrengthPointsRemaining, idStat, pointsToAssign);
        }

        public void AssignPointsInAgility(int idStat, int pointsToAssign) {
            AgilityPointsRemaining = AssignPoints(Agility, MaxAgilityPointsRemaining, AgilityPointsRemaining, idStat, pointsToAssign);
        }

        public void AssignPointsInLuck(int idStat, int pointsToAssign) {
            LuckPointsRemaining = AssignPoints(Luck, MaxLuckPointsRemaining, LuckPointsRemaining, idStat, pointsToAssign);
        }

        public void AssignPointsInMajor(int idStat, int pointsToAssign) {
            MajorPointsRemaining = AssignPoints(Major, MaxMajorPointsRemaining, MajorPointsRemaining, idStat, pointsToAssign);
        }

    }
}
