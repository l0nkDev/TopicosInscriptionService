using CareerApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TopicosP1Backend.Scripts
{
    public enum Function
    {
        GetCareers, PostCareer, GetCareer, PutCareer, DeleteCareer,//
        GetGestions, PostGestion, GetGestion, DeleteGestion,//
        GetGroups, PostGroup, GetGroup, PutGroup, DeleteGroup,//
        GetTimeSlots, PostTimeSlot, PutTimeSlot, DeleteTimeSlot,//
        GetInscriptions, PostInscription, GetInscription, DeleteInscription,//
        GetModules, PostModule, GetModule, DeleteModule,//
        GetModRooms, PostModRoom, DeleteModRoom,//
        GetPeriods, PostPeriod, GetPeriod, PutPeriod, DeletePeriod,//
        GetStudents, PostStudent, GetStudent, PutStudent, DeleteStudent,//
        GetStudentGroup, PutStudentGroup,//
        GetStudentHistory, GetStudentAvaliables,//
        GetStudyPlans, PostStudyPlan, GetStudyPlan, PutStudyPlan, DeleteStudyPlan,//
        GetSpSubjects, PostSpSubject, PutSpSubject, DeleteSpSubject,//
        GetSpSubDependencies, PostSpSubDependency, DeleteSpSubDependency,//
        GetSubjects, PostSubject, GetSubject, PutSubject, DeleteSubject,//
        GetTeachers, PostTeacher, GetTeacher, PutTeacher, DeleteTeacher//
    }

    public class QueuedFunction
    {
        required public int Queue { get; set; }
        required public string Hash { get; set; }
        required public Function Function { get; set; }
        required public List<string> ItemIds { get; set; }
        required public string Body { get; set; }
        required public string Callback { get; set; }
        public DBItem ToDBItem() => new(this);
        public class DBItem
        {
            public int Queue { get; set; }
            public long Id { get; set; }
            public string Hash { get; set; }
            public int Function { get; set; }
            public string ItemIds { get; set; }
            public string Body { get; set; }
            public string Callback { get; set; }
            public QueuedFunction ToQueueItem() => new()
            {
                Queue = Queue,
                Hash = Hash,
                Function = (Function)Function,
                ItemIds = JsonSerializer.Deserialize<List<string>>(ItemIds),
                Body = Body,
                Callback = Callback
            };

            public DBItem(QueuedFunction qf)
            {
                Queue = qf.Queue;
                Hash = qf.Hash;
                Function = (int)qf.Function;
                ItemIds = JsonSerializer.Serialize(qf.ItemIds);
                Body = qf.Body;
                Callback = qf.Callback;
            }

            [JsonConstructor]
            public DBItem(string Hash, int Function, string ItemIds, string Body)
            {
                this.Hash = Hash;
                this.Function = Function;
                this.ItemIds = ItemIds;
                this.Body = Body;
            }
        }

        public async Task<object?> Execute(Context context)
        {
            switch (Function)
            {
                case Function.GetCareers: return await Career.GetCareers(context);
                case Function.PostCareer: return await Career.PostCareer(context, JsonSerializer.Deserialize<Career.CareerPost>(Body));
                case Function.GetCareer: return await Career.GetCareer(context, long.Parse(ItemIds[0]));
                case Function.PutCareer: return await Career.PutCareer(context, long.Parse(ItemIds[0]), JsonSerializer.Deserialize<Career.CareerPost>(Body));
                case Function.DeleteCareer: return await Career.DeleteCareer(context, long.Parse(ItemIds[0]));

                case Function.GetGestions: return await Gestion.GetGestions(context);
                case Function.PostGestion: return await Gestion.PostGestion(context, JsonSerializer.Deserialize<Gestion>(Body));
                case Function.GetGestion: return await Gestion.GetGestion(context, long.Parse(ItemIds[0]));
                case Function.DeleteGestion: return await Gestion.DeleteGestion(context, long.Parse(ItemIds[0]));

                case Function.GetGroups: return await Group.GetGroups(context);
                case Function.PostGroup: return await Group.PostGroup(context, JsonSerializer.Deserialize<Group.GroupPost>(Body));
                case Function.GetGroup: return await Group.GetGroup(context, long.Parse(ItemIds[0]));
                case Function.PutGroup: return await Group.PutGroup(context, long.Parse(ItemIds[0]), JsonSerializer.Deserialize<Group.GroupPost>(Body));
                case Function.DeleteGroup: return await Group.DeleteGroup(context, long.Parse(ItemIds[0]));

                case Function.GetTimeSlots: return await Group.GetTimeSlots(context, long.Parse(ItemIds[0]));
                case Function.PostTimeSlot: return await Group.PostTimeSlot(context, long.Parse(ItemIds[0]), JsonSerializer.Deserialize<TimeSlot.TimeSlotPost>(Body));
                case Function.PutTimeSlot: return await Group.PutTimeSlot(context, long.Parse(ItemIds[0]), long.Parse(ItemIds[1]), JsonSerializer.Deserialize<TimeSlot.TimeSlotPost>(Body));
                case Function.DeleteTimeSlot: return await Group.DeleteTimeSlot(context, long.Parse(ItemIds[0]), long.Parse(ItemIds[1]));

                case Function.GetInscriptions: return await Inscription.GetInscriptions(context);
                case Function.PostInscription: return await Inscription.PostInscription(context, JsonSerializer.Deserialize<Inscription.InscriptionPost>(Body));
                case Function.GetInscription: return await Inscription.GetInscription(context, long.Parse(ItemIds[0]));
                case Function.DeleteInscription: return await Inscription.DeleteInscription(context, long.Parse(ItemIds[0]));

                case Function.GetModules: return await Module.GetModules(context);
                case Function.PostModule: return await Module.PostModule(context, JsonSerializer.Deserialize<Module.ModulePost>(Body));
                case Function.GetModule: return await Module.GetModule(context, long.Parse(ItemIds[0]));
                case Function.DeleteModule: return await Module.DeleteModule(context, long.Parse(ItemIds[0]));

                case Function.GetModRooms: return await Module.GetModRooms(context, long.Parse(ItemIds[0]));
                case Function.PostModRoom: return await Module.PostModRoom(context, long.Parse(ItemIds[0]), JsonSerializer.Deserialize<Module.ModulePost>(Body));
                case Function.DeleteModRoom: return await Module.DeleteModRoom(context, long.Parse(ItemIds[0]), long.Parse(ItemIds[1]));

                case Function.GetPeriods: return await Period.GetPeriods(context);
                case Function.PostPeriod: return await Period.PostPeriod(context, JsonSerializer.Deserialize<Period.PostDTO>(Body));
                case Function.GetPeriod: return await Period.GetPeriod(context, long.Parse(ItemIds[0]));
                case Function.DeletePeriod: return await Period.DeletePeriod(context, long.Parse(ItemIds[0]));

                case Function.GetStudents: return await Student.GetStudents(context);
                case Function.PostStudent: return await Student.PostStudent(context, JsonSerializer.Deserialize<Student.StudentPost>(Body));
                case Function.GetStudent: return await Student.GetStudent(context, long.Parse(ItemIds[0]));
                case Function.PutStudent: return await Student.PutStudent(context, long.Parse(ItemIds[0]), JsonSerializer.Deserialize<Student.StudentPost>(Body));
                case Function.DeleteStudent: return await Student.DeleteStudent(context, long.Parse(ItemIds[0]));

                case Function.GetStudentGroup: return await Student.GetStudentGroup(context, long.Parse(ItemIds[0]), long.Parse(ItemIds[1]));
                case Function.PutStudentGroup: return await Student.PutStudentGroup(context, long.Parse(ItemIds[0]), long.Parse(ItemIds[1]), JsonSerializer.Deserialize<StudentGroups.SgDTO>(Body));

                case Function.GetStudentHistory: return await Student.History(context, long.Parse(ItemIds[0]));
                case Function.GetStudentAvaliables: return await Student.Available(context, long.Parse(ItemIds[0]));

                case Function.GetStudyPlans: return await StudyPlan.GetStudyPlans(context);
                case Function.PostStudyPlan: return await StudyPlan.PostStudyPlan(context, JsonSerializer.Deserialize<StudyPlan.StudyPlanPost>(Body));
                case Function.GetStudyPlan: return await StudyPlan.GetStudyPlan(context, ItemIds[0]);
                case Function.PutStudyPlan: return await StudyPlan.PutStudyPlan(context, ItemIds[0], JsonSerializer.Deserialize<StudyPlan.StudyPlanPost>(Body));
                case Function.DeleteStudyPlan: return await StudyPlan.DeleteStudyPlan(context, ItemIds[0]);

                case Function.GetSpSubjects: return await StudyPlan.GetSpSubject(context, ItemIds[0]);
                case Function.PostSpSubject: return await StudyPlan.PostSpSubject(context, ItemIds[0], JsonSerializer.Deserialize<StudyPlan.StudyPlanSubjectPost>(Body));
                case Function.PutSpSubject: return await StudyPlan.PutSpSubject(context, ItemIds[0], ItemIds[1], JsonSerializer.Deserialize<StudyPlan.StudyPlanSubjectPost>(Body));
                case Function.DeleteSpSubject: return await StudyPlan.DeleteSpSubject(context, ItemIds[0], ItemIds[1]);

                case Function.GetSpSubDependencies: return await StudyPlan.GetSpSubDependencies(context, ItemIds[0], ItemIds[1]);
                case Function.PostSpSubDependency: return await StudyPlan.PostSpSubDependency(context, ItemIds[0], ItemIds[1], JsonSerializer.Deserialize<StudyPlan.SPSDependency>(Body));
                case Function.DeleteSpSubDependency: return await StudyPlan.DeleteSpSubDependency(context, ItemIds[0], ItemIds[1], ItemIds[2]);

                case Function.GetSubjects: return await Subject.GetAll(context);
                case Function.PostSubject: return await Subject.Post(context, JsonSerializer.Deserialize<Subject.PostSubject>(Body));
                case Function.GetSubject: return await Subject.Get(context, ItemIds[0]);
                case Function.PutSubject: return await Subject.Put(context, ItemIds[0], JsonSerializer.Deserialize<Subject.PostSubject>(Body));
                case Function.DeleteSubject: return await Subject.Delete(context, ItemIds[0]);

                case Function.GetTeachers: return await Teacher.GetTeachers(context);
                case Function.PostTeacher: return await Teacher.PostTeacher(context, JsonSerializer.Deserialize<Teacher.TeacherPost>(Body));
                case Function.GetTeacher: return await Teacher.GetTeacher(context, long.Parse(ItemIds[0]));
                case Function.PutTeacher: return await Teacher.PutTeacher(context, long.Parse(ItemIds[0]), JsonSerializer.Deserialize<Teacher.TeacherPost>(Body));
                case Function.DeleteTeacher: return await Teacher.DeleteTeacher(context, long.Parse(ItemIds[0]));
            }
            return new BadRequestResult();
        }
    }
}
