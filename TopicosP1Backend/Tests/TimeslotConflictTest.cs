using CareerApi.Models;
using Microsoft.EntityFrameworkCore;
using TopicosP1Backend.Exceptions;
using TopicosP1Backend.Scripts;

namespace TopicosInscriptionService.Tests
{
    [TestClass]
    public class TimeslotConflictTest
    {


        private static Teacher teacher = new() { FirstName = "Test", LastName = "Test" };
        private static Gestion gestion = new() { Year = 2025 };
        private static Period period = new() { Gestion = gestion, Number = 1 };
        private static Module module = new();
        private static Room room = new() { Module = module, Number = 1 };
        private static Subject subject = new() { Code = "TST101", Title = "TEST 1" };
        private static List<Group> groups = [
                new() { Code="SA", Mode="Presencial", Period=period, Subject = subject, Teacher=teacher },
                new() { Code="SA", Mode="Presencial", Period=period, Subject = subject, Teacher=teacher },
            ];
        private static DbContextOptions<Context> options = new DbContextOptionsBuilder<Context>()
            .UseInMemoryDatabase("testing").Options;

        [TestMethod]
        public void TestSameDaySameTime()
        {
            groups[0].TimeSlots = [
                new() { Day="Monday", StartTime=new(07,00), EndTime=new(08,30), Group=groups[0], Room=room },
                new() { Day="Wednesday", StartTime=new(07,00), EndTime=new(08,30), Group=groups[0], Room=room },
                new() { Day="Friday", StartTime=new(07,00), EndTime=new(08,30), Group=groups[0], Room=room },
                ];

            groups[1].TimeSlots = [
                new() { Day="Monday", StartTime=new(07,00), EndTime=new(08,30), Group=groups[0], Room=room },
                new() { Day="Wednesday", StartTime=new(07,00), EndTime=new(08,30), Group=groups[0], Room=room },
                new() { Day="Friday", StartTime=new(07,00), EndTime=new(08,30), Group=groups[0], Room=room },
                ];

            Assert.Throws<TimeslotConflictException>(() => Inscription.TimeslotConflictCheck(groups));
        }

        [TestMethod]
        public void OneAfterAnotherTest()
        {
            groups[0].TimeSlots = [
                new() { Day="Monday", StartTime=new(07,00), EndTime=new(08,30), Group=groups[0], Room=room },
                new() { Day="Wednesday", StartTime=new(07,00), EndTime=new(08,30), Group=groups[0], Room=room },
                new() { Day="Friday", StartTime=new(07,00), EndTime=new(08,30), Group=groups[0], Room=room },
                ];

            groups[1].TimeSlots = [
                new() { Day="Monday", StartTime=new(08,30), EndTime=new(10,00), Group=groups[0], Room=room },
                new() { Day="Wednesday", StartTime=new(08,30), EndTime=new(10,00), Group=groups[0], Room=room },
                new() { Day="Friday", StartTime=new(08, 30), EndTime=new(10,00), Group=groups[0], Room=room },
                ];

            Inscription.TimeslotConflictCheck(groups);
        }

        [TestMethod]
        public void TestFullQuota()
        {
            groups[0].Quota = 0;

            Assert.IsFalse(Inscription.hasQuota(groups[0]));
        }

        [TestMethod]
        public void TestAvailableQuota()
        {
            groups[0].Quota = 1;

            Assert.IsTrue(Inscription.hasQuota(groups[0]));
        }

        [TestMethod]
        public async Task TestAvailableQuotaFromDatabase()
        {
            using (var context = new Context(options))
            {
                await Inscription.PostInscription(context, new() { Gestion = 2025, GroupIds = [7], Id = 0, Period = 2, Student = 1, Type = 0 });
            }
        }
    }
}