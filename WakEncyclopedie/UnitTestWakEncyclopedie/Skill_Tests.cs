using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WakEncyclopedie.BO;

namespace UnitTestWakEncyclopedie {
    [TestClass]
    public class Skill_Tests {
        [TestMethod]
        [DataRow(1, 0)] // 0 for all
        [DataRow(2, 1)]
        [DataRow(3, 1)]
        [DataRow(4, 1)]
        [DataRow(5, 1)]
        [DataRow(6, 2)]
        [DataRow(80, 20)]
        [DataRow(81, 20)] // 20 for all
        [DataRow(82, 21)]
        [DataRow(83, 21)]
        [DataRow(84, 21)]
        [DataRow(85, 21)]
        [DataRow(86, 22)]
        [DataRow(200, 50)] // 50 for all
        public void CalculatePointsForIntelligence(int level, int expected) {
            Skill skill = new Skill();
            Assert.AreEqual(expected, skill.CalculatePointsForIntelligence(level));
        }

        [TestMethod]
        [DataRow(1, 0)] // 0 for all
        [DataRow(2, 0)]
        [DataRow(3, 1)]
        [DataRow(4, 1)]
        [DataRow(5, 1)]
        [DataRow(6, 1)]
        [DataRow(80, 20)]
        [DataRow(81, 20)] // 20 for all
        [DataRow(82, 20)]
        [DataRow(83, 21)]
        [DataRow(84, 21)]
        [DataRow(85, 21)]
        [DataRow(86, 21)]
        [DataRow(200, 50)] // 50 for all
        public void CalculatePointsForStrength(int level, int expected) {
            Skill skill = new Skill();
            Assert.AreEqual(expected, skill.CalculatePointsForStrength(level));
        }

        [TestMethod]
        [DataRow(1, 0)] // 0 for all
        [DataRow(2, 0)]
        [DataRow(3, 0)]
        [DataRow(4, 1)]
        [DataRow(5, 1)]
        [DataRow(6, 1)]
        [DataRow(80, 20)]
        [DataRow(81, 20)] // 20 for all
        [DataRow(82, 20)]
        [DataRow(83, 20)]
        [DataRow(84, 21)]
        [DataRow(85, 21)]
        [DataRow(86, 21)]
        [DataRow(200, 50)] // 50 for all
        public void CalculatePointsForAgility(int level, int expected) {
            Skill skill = new Skill();
            Assert.AreEqual(expected, skill.CalculatePointsForAgility(level));
        }

        [TestMethod]
        [DataRow(1, 0)] // 0 for all
        [DataRow(2, 0)]
        [DataRow(3, 0)]
        [DataRow(4, 0)]
        [DataRow(5, 1)]
        [DataRow(6, 1)]
        [DataRow(80, 19)]
        [DataRow(81, 20)] // 20 for all
        [DataRow(82, 20)]
        [DataRow(83, 20)]
        [DataRow(84, 20)]
        [DataRow(85, 21)]
        [DataRow(86, 21)]
        [DataRow(200, 50)] // 50 for all
        public void CalculatePointsForLuck(int level, int expected) {
            Skill skill = new Skill();
            Assert.AreEqual(expected, skill.CalculatePointsForLuck(level));
        }

        [TestMethod]
        [DataRow(1, 0)] // 0 for all
        [DataRow(25, 1)]
        [DataRow(74, 1)]
        [DataRow(75, 2)]
        [DataRow(76, 2)]
        [DataRow(80, 2)]
        [DataRow(125, 3)]
        [DataRow(175, 4)]
        [DataRow(200, 4)]
        public void CalculatePointsForMajor(int level, int expected) {
            Skill skill = new Skill();
            Assert.AreEqual(expected, skill.CalculatePointsForMajor(level));
        }
    }
}
