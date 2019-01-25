using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WakEncyclopedie.BO
{
    public class SkillsStat
    {
        public int Id { get; private set; }
        public string Type { get; private set;}
        public int ValuePerPoints { get; private set; }

        public int MaxAssignedPoints { get; private set; }
        public int AssignedPoints { get; private set; }

        public SkillsStat(int id, string type, int value, int maxAssignedPoints){
            Id = id;
            Type = type;
            ValuePerPoints = value;
            MaxAssignedPoints = maxAssignedPoints;
        }


        public SkillsStat(int id, string type, int value, int maxAssignedPoints, int assignedPoints) : this(id, type, value, maxAssignedPoints) {
            AssignedPoints = assignedPoints;
        }

        public int AssignPoints(int pointsToAssign) {
            int assignedPointsBefore = AssignedPoints;
            if (AssignedPoints + pointsToAssign <= MaxAssignedPoints) {
                AssignedPoints += pointsToAssign;
            } else {
                AssignedPoints = MaxAssignedPoints;
            }
            int assignedPointsAfter = AssignedPoints - assignedPointsBefore;
            int pointsRemaining = pointsToAssign - assignedPointsAfter;
            return pointsRemaining;
        }

        public int RemovePoints(int pointsToRemove) {
            int assignedPointsBefore = AssignedPoints;
            if (AssignedPoints - pointsToRemove >= 0) {
                AssignedPoints -= pointsToRemove;
            } else {
                AssignedPoints = 0;
            }
            int assignedPointsAfter = assignedPointsBefore - AssignedPoints;
            int pointsRemaining = pointsToRemove - assignedPointsAfter;
            return pointsRemaining;
        }

        public int GetTotalValue() {
            return ValuePerPoints * AssignedPoints;
        }
    }
}
