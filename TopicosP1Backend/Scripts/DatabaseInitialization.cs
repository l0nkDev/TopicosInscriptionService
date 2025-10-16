using CareerApi.Models;

namespace TopicosP1Backend.Scripts
{
    public class DatabaseInitialization
    {
        public static void Populate(Context context)
        {
            if (context.Careers.Any())
                return;

            var users = new List<User>
            {
                new() { Role = 'S', Login = "222008288", PasswordHash=Util.Hash("123"), Token=Util.GenToken() },
                new() { Role = 'C', Login = "admin", PasswordHash=Util.Hash("123"), Token=Util.GenToken() },
            };
            context.Users.AddRange(users);

            var careers = new List<Career>
            {
                new() { Name = "INGENIERIA INFORMATICA" },
                new() { Name = "INGENIERIA EN SISTEMAS" },
                new() { Name = "INGENIERIA EN REDES Y TELECOMUNICACIONES" },
                new() { Name = "INGENIERIA EN ROBOTICA"},
            };
            context.Careers.AddRange(careers);

            var studyplans = new List<StudyPlan>
            {
                new() { Code = "187-3", Career = careers[0] }, //INF
                new() { Code = "187-4", Career = careers[1] }, //SIS
                new() { Code = "187-5", Career = careers[2] }, //RDS
                new() { Code = "323-0", Career = careers[3] }, //RBT
                new() { Code = "187-6", Career = careers[0] }, //INF(New)
            };
            context.StudyPlans.AddRange(studyplans);

            var subjects = new List<Subject>
            {
                // Semestre 1
                new() { Code = "MAT101", Title = "CALCULO I" },                               //0   INF, SIS; RDS, RBT
                new() { Code = "INF119", Title = "ESTRUCTURAS DISCRETAS" },                   //1
                new() { Code = "INF110", Title = "INTRODUCCION A LA INFORMATICA" },           //2
                new() { Code = "FIS100", Title = "FISICA I" },                                //3
                new() { Code = "LIN100", Title = "INGLES TECNICO I" },                        //4
                new() { Code = "MET100", Title = "METODOLOGIA DE LA INVESTIGACION" },         //5
                // Semestre 2
                new() { Code = "MAT102", Title = "CALCULO II" },                              //6  
                new() { Code = "MAT103", Title = "ALGEBRA LINEAL" },                          //7
                new() { Code = "INF120", Title = "PROGRAMACION I" },                          //8
                new() { Code = "FIS102", Title = "FISICA II" },                               //9
                new() { Code = "LIN101", Title = "INGLES TECNICO II" },                       //10
                // Semestre 3
                new() { Code = "MAT207", Title = "ECUACIONES DIFERENCIALES" },                //11  
                new() { Code = "INF210", Title = "PROGRAMACION II" },                         //12
                new() { Code = "INF211", Title = "ARQUITECTURA DE COMPUTADORAS" },            //13
                new() { Code = "FIS200", Title = "FISICA III" },                              //14
                new() { Code = "ADM100", Title = "ADMINISTRACION" },                          //15
                new() { Code = "ELT241", Title = "TEORIA DE CAMPOS" },                        //16
                new() { Code = "RDS210", Title = "ANALISIS DE CIRCUITOS" },                   //17
                // Semestre 4
                new() { Code = "MAT202", Title = "PROBABILIDADES Y ESTADIST.I" },             //18  
                new() { Code = "MAT205", Title = "METODOS NUMERICOS" },                       //19
                new() { Code = "INF220", Title = "ESTRUCTURA DE DATOS I" },                   //20
                new() { Code = "INF221", Title = "PROGRAMACION ENSAMBLADOR" },                //21
                new() { Code = "ADM200", Title = "CONTABILIDAD" },                            //22
                new() { Code = "RDS220", Title = "ANALISIS DE CIRCUITOS ELECTRONICOS" },      //23
                // Semestre 5
                new() { Code = "MAT302", Title = "PROBABILIDADES Y ESTADISTICAS II" },        //24  
                new() { Code = "INF318", Title = "PROGRAMAC.LOGICA Y FUNCIONAL" },            //25
                new() { Code = "INF310", Title = "ESTRUCTURA DE DATOS II" },                  //26
                new() { Code = "INF312", Title = "BASE DE DATOS I" },                         //27
                new() { Code = "INF319", Title = "LENGUAJES FORMALES" },                      //28
                new() { Code = "ADM330", Title = "ORGANIZACION Y METODOS" },                  //29
                new() { Code = "ECO300", Title = "ECONOMIA PARA LA GESTION" },                //30
                new() { Code = "RDS310", Title = "ELECTRONICA APLICADA A REDES" },            //31
                new() { Code = "ELT352", Title = "SISTEMAS LOGICOS DIGITALES I" },            //32
                new() { Code = "ELT354", Title = "SEÑALES Y SISTEMAS" },                      //33
                // Semestre 6
                new() { Code = "MAT329", Title = "INVESTIG. OPERATIVA I" },                   //34  
                new() { Code = "INF342", Title = "SISTEMAS DE INFORMACION I" },               //35
                new() { Code = "INF323", Title = "SISTEMAS OPERATIVOS I" },                   //36
                new() { Code = "INF322", Title = "BASE DE DATOS II" },                        //37
                new() { Code = "INF329", Title = "COMPILADORES" },                            //38
                new() { Code = "ADM320", Title = "FINANZAS PARA LA EMPRESA" },                //39
                new() { Code = "ELT362", Title = "SISTEMAS LOGICOS DIGITALES II" },           //40
                new() { Code = "RDS320", Title = "INTERPRETAC. DE SISTEMAS Y SEÑALES" },      //41
                // Semestre 7  
                new() { Code = "MAT419", Title = "INVESTIGAC.OPERATIVA II" },                 //42  
                new() { Code = "INF418", Title = "INTELIGENCIA ARTIFICIAL" },                 //43
                new() { Code = "INF413", Title = "SISTEMAS OPERATIVOS II" },                  //44
                new() { Code = "INF433", Title = "REDES I" },                                 //45
                new() { Code = "INF412", Title = "SISTEMAS DE INFORMACION II" },              //46
                new() { Code = "INF432", Title = "SIS.P/EL SOPORT.A LA TOMA DECI." },         //47
                new() { Code = "INF410", Title = "APLICACIONES CON MICROPROCESADORES" },      //48
                new() { Code = "ELT374", Title = "SISTEMAS DE COMUNICACION I" },              //49
                // Semestre 8  
                new() { Code = "ECO449", Title = "PREPARAC.Y EVALUAC.DE PROYECTOS" },         //50
                new() { Code = "INF428", Title = "SISTEMAS EXPERTOS" },                       //51
                new() { Code = "INF442", Title = "SISTEMAS DE INFORM.GEOGRAFICA" },           //52
                new() { Code = "INF423", Title = "REDES II" },                                //53
                new() { Code = "INF422", Title = "INGENIERIA DE SOFTWARE I" },                //54
                new() { Code = "INF462", Title = "AUDITORIA INFORMATICA" },                   //55
                new() { Code = "RDS421", Title = "TALLER DE AN Y DISEÑO DE REDES" },          //56
                new() { Code = "RDS429", Title = "LEGISLACION EN REDES Y COMUNICACIONES" },   //57
                new() { Code = "ELT384", Title = "SISTEMAS DE COMUNICACION II" },             //58
                // Semestre 9
                new() { Code = "INF511", Title = "TALLER DE GRADO I" },                       //59
                new() { Code = "INF512", Title = "INGENIERIA DE SOFTWARE II" },               //60
                new() { Code = "INF513", Title = "TECNOLOGIA WEB" },                          //61
                new() { Code = "INF514", Title = "ARQUITECTURA DEL SOFTWARE" },               //62
                new() { Code = "RDS511", Title = "GESTION Y ADMINIS.DE REDES" },              //63
                new() { Code = "RDS512", Title = "REDES INALAMB. Y COMUN.MOVILES" },          //64
                new() { Code = "RDS519", Title = "SEGURID.EN REDES Y TRANSMISION DE DATOS" }, //65
                // Semestre 10
                new() { Code = "GDI522", Title = "GRADUACION DIRECTA" },                      //66
                new() { Code = "INF521", Title = "TALLER DE GRADO II" },                      //67
                // Modalidad de Titulacion
                new() { Code = "GRM001", Title = "MODALIDAD DE GRADUACION TECNICO MEDIO" },   //68
                new() { Code = "GRT001", Title = "MODALIDAD DE GRADUACION TECNICO SUPERIOR" },//69
                new() { Code = "GDI001", Title = "MODALIDAD DE GRADUACION" },                 //70
                new() { Code = "GRL001", Title = "MODALIDAD DE GRADUACION" },                 //71 
                //Electivas
                //Informatica
                new() { Code = "ELC101", Title = "MODELADO Y SIMULACION DE SISTEMAS" },       //72
                new() { Code = "ELC102", Title = "PROGRAMACION GRAFICA" },                    //73
                new() { Code = "ELC103", Title = "TOPIC. AVANZ. DE PROGRAMAC. (ALGORIT. GENR.)" },//74
                new() { Code = "ELC104", Title = "PROGRAMAC. DE APLICAC. DE TIEMPO REAL" },   //75
                new() { Code = "ELC105", Title = "SISTEMAS DISTRIBUIDOS" },                   //76
                new() { Code = "ELC106", Title = "INTERACCION HOMBRE-COMPUTADOR" },           //77
                new() { Code = "ELC107", Title = "CRIPTOGRAFIA Y SEGURIDAD" },                //78
                new() { Code = "ELC108", Title = "CONTROL Y AUTOMATIZACION" },                //79
                //Sistemas
                new() { Code = "ELC001", Title = "ADMINISTRACION DE RECURSOS HUMANOS" },      //80
                new() { Code = "ELC002", Title = "COSTOS Y PRESUPUESTOS" },                   //81
                new() { Code = "ELC003", Title = "PRODUCCION Y MARKETING" },                  //82
                new() { Code = "ELC004", Title = "REINGENIERIA" },                            //83
                new() { Code = "ELC005", Title = "INGENIERIA DE CALIDAD" },                   //84
                new() { Code = "ELC006", Title = "BENCHMARKING" },                            //85
                new() { Code = "ELC007", Title = "MACROECONOMIA" },                           //86
                new() { Code = "ELC008", Title = "LEGISLACION EN CIENCIAS DE LA COMPUTACION" },//87
                //Redes
                new() { Code = "ELC201", Title = "DISEÑO DE CIRCUITOS INTEGRADOS" },          //88
                new() { Code = "ELC202", Title = "INSTRUMENTACION" },                         //89
                new() { Code = "ELC203", Title = "SISTEMAS DE COMUNICACION SCADA" },          //90
                new() { Code = "ELC204", Title = "TELEVISION DIGITAL" },                      //91
                new() { Code = "ELC205", Title = "DOMOTICA" },                                //92
                new() { Code = "ELC206", Title = "LINEAS DE TRANSMISION Y ANTENAS" },         //93
                new() { Code = "ELC207", Title = "TECNICAS DE PRESENTACION PARA INGENIERIA" },//94
                new() { Code = "ELC208", Title = "REDES AD HOC" },                            //95
            };
            context.Subjects.AddRange(subjects);

            var prerequisites = new List<SubjectDependency>()
            {
                // INFORMATICA (Antiguo)
                // Semestre 1
                // Semestre 2
                new() { Prerequisite = subjects[0], Postrequisite = subjects[6], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[1], Postrequisite = subjects[7], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[2], Postrequisite = subjects[8], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[3], Postrequisite = subjects[9], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[4], Postrequisite = subjects[10], StudyPlan = studyplans[0] },
                // Semestre 3
                new() { Prerequisite = subjects[6], Postrequisite = subjects[11], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[7], Postrequisite = subjects[12], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[8], Postrequisite = subjects[12], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[8], Postrequisite = subjects[13], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[9], Postrequisite = subjects[13], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[9], Postrequisite = subjects[14], StudyPlan = studyplans[0] },
                // Semestre 4
                new() { Prerequisite = subjects[6], Postrequisite = subjects[18], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[11], Postrequisite = subjects[19], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[12], Postrequisite = subjects[20], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[13], Postrequisite = subjects[21], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[15], Postrequisite = subjects[22], StudyPlan = studyplans[0] },
                // Semestre 5
                new() { Prerequisite = subjects[18], Postrequisite = subjects[24], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[20], Postrequisite = subjects[25], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[20], Postrequisite = subjects[26], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[20], Postrequisite = subjects[27], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[20], Postrequisite = subjects[28], StudyPlan = studyplans[0] },
                // Semestre 6
                new() { Prerequisite = subjects[24], Postrequisite = subjects[34], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[27], Postrequisite = subjects[35], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[26], Postrequisite = subjects[36], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[27], Postrequisite = subjects[37], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[28], Postrequisite = subjects[38], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[27], Postrequisite = subjects[38], StudyPlan = studyplans[0] },
                // Semestre 7
                new() { Prerequisite = subjects[34], Postrequisite = subjects[42], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[25], Postrequisite = subjects[43], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[26], Postrequisite = subjects[43], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[36], Postrequisite = subjects[44], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[36], Postrequisite = subjects[45], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[35], Postrequisite = subjects[46], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[37], Postrequisite = subjects[46], StudyPlan = studyplans[0] },
                // Semestre 8
                new() { Prerequisite = subjects[42], Postrequisite = subjects[50], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[43], Postrequisite = subjects[51], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[46], Postrequisite = subjects[51], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[46], Postrequisite = subjects[52], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[45], Postrequisite = subjects[53], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[46], Postrequisite = subjects[54], StudyPlan = studyplans[0] },
                // Semestre 9 
                new() { Prerequisite = subjects[50], Postrequisite = subjects[59], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[51], Postrequisite = subjects[59], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[52], Postrequisite = subjects[59], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[53], Postrequisite = subjects[59], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[54], Postrequisite = subjects[59], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[51], Postrequisite = subjects[60], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[52], Postrequisite = subjects[60], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[53], Postrequisite = subjects[60], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[54], Postrequisite = subjects[60], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[51], Postrequisite = subjects[61], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[52], Postrequisite = subjects[61], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[53], Postrequisite = subjects[61], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[54], Postrequisite = subjects[61], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[51], Postrequisite = subjects[62], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[52], Postrequisite = subjects[62], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[53], Postrequisite = subjects[62], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[54], Postrequisite = subjects[62], StudyPlan = studyplans[0] },
                // Semestre 10
                new() { Prerequisite = subjects[59], Postrequisite = subjects[66], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[60], Postrequisite = subjects[66], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[61], Postrequisite = subjects[66], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[62], Postrequisite = subjects[66], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[59], Postrequisite = subjects[67], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[60], Postrequisite = subjects[67], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[61], Postrequisite = subjects[67], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[62], Postrequisite = subjects[67], StudyPlan = studyplans[0] },
                // Modalidad de Graduacion
                new() { Prerequisite = subjects[18], Postrequisite = subjects[68], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[20], Postrequisite = subjects[68], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[21], Postrequisite = subjects[68], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[22], Postrequisite = subjects[68], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[34], Postrequisite = subjects[69], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[35], Postrequisite = subjects[69], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[36], Postrequisite = subjects[69], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[37], Postrequisite = subjects[69], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[38], Postrequisite = subjects[69], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[59], Postrequisite = subjects[71], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[60], Postrequisite = subjects[71], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[61], Postrequisite = subjects[71], StudyPlan = studyplans[0] },
                new() { Prerequisite = subjects[62], Postrequisite = subjects[71], StudyPlan = studyplans[0] },
                // SISTEMAS
                // Semestre 1
                // Semestre 2
                new() { Prerequisite = subjects[0], Postrequisite = subjects[6], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[1], Postrequisite = subjects[7], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[2], Postrequisite = subjects[8], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[3], Postrequisite = subjects[9], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[4], Postrequisite = subjects[10], StudyPlan = studyplans[1] },
                // Semestre 3
                new() { Prerequisite = subjects[6], Postrequisite = subjects[11], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[7], Postrequisite = subjects[12], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[8], Postrequisite = subjects[12], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[8], Postrequisite = subjects[13], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[9], Postrequisite = subjects[13], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[9], Postrequisite = subjects[14], StudyPlan = studyplans[1] },
                // Semestre 4
                new() { Prerequisite = subjects[6], Postrequisite = subjects[18], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[11], Postrequisite = subjects[19], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[12], Postrequisite = subjects[20], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[13], Postrequisite = subjects[21], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[15], Postrequisite = subjects[22], StudyPlan = studyplans[1] },
                // Semestre 5
                new() { Prerequisite = subjects[18], Postrequisite = subjects[24], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[20], Postrequisite = subjects[26], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[20], Postrequisite = subjects[27], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[22], Postrequisite = subjects[30], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[22], Postrequisite = subjects[29], StudyPlan = studyplans[1] },
                // Semestre 6
                new() { Prerequisite = subjects[24], Postrequisite = subjects[34], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[26], Postrequisite = subjects[36], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[27], Postrequisite = subjects[37], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[27], Postrequisite = subjects[35], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[29], Postrequisite = subjects[39], StudyPlan = studyplans[1] },
                // Semestre 7
                new() { Prerequisite = subjects[34], Postrequisite = subjects[42], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[36], Postrequisite = subjects[45], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[36], Postrequisite = subjects[44], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[35], Postrequisite = subjects[46], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[37], Postrequisite = subjects[46], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[35], Postrequisite = subjects[47], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[37], Postrequisite = subjects[47], StudyPlan = studyplans[1] },
                // Semestre 8
                new() { Prerequisite = subjects[42], Postrequisite = subjects[50], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[45], Postrequisite = subjects[53], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[46], Postrequisite = subjects[52], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[46], Postrequisite = subjects[54], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[46], Postrequisite = subjects[55], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[39], Postrequisite = subjects[55], StudyPlan = studyplans[1] },
                // Semestre 9 
                new() { Prerequisite = subjects[50], Postrequisite = subjects[59], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[52], Postrequisite = subjects[59], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[53], Postrequisite = subjects[59], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[54], Postrequisite = subjects[59], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[55], Postrequisite = subjects[59], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[52], Postrequisite = subjects[60], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[53], Postrequisite = subjects[60], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[54], Postrequisite = subjects[60], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[55], Postrequisite = subjects[60], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[52], Postrequisite = subjects[61], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[53], Postrequisite = subjects[61], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[54], Postrequisite = subjects[61], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[55], Postrequisite = subjects[61], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[52], Postrequisite = subjects[62], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[53], Postrequisite = subjects[62], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[54], Postrequisite = subjects[62], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[55], Postrequisite = subjects[62], StudyPlan = studyplans[1] },
                // Semestre 10
                new() { Prerequisite = subjects[59], Postrequisite = subjects[66], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[60], Postrequisite = subjects[66], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[61], Postrequisite = subjects[66], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[62], Postrequisite = subjects[66], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[59], Postrequisite = subjects[67], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[60], Postrequisite = subjects[67], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[61], Postrequisite = subjects[67], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[62], Postrequisite = subjects[67], StudyPlan = studyplans[1] },
                // Modalidad de Graduacion
                new() { Prerequisite = subjects[59], Postrequisite = subjects[71], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[60], Postrequisite = subjects[71], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[61], Postrequisite = subjects[71], StudyPlan = studyplans[1] },
                new() { Prerequisite = subjects[62], Postrequisite = subjects[71], StudyPlan = studyplans[1] },
                // REDES
                // Semestre 1
                // Semestre 2
                new() { Prerequisite = subjects[0], Postrequisite = subjects[6], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[1], Postrequisite = subjects[7], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[2], Postrequisite = subjects[8], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[3], Postrequisite = subjects[9], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[4], Postrequisite = subjects[10], StudyPlan = studyplans[2] },
                // Semestre 3
                new() { Prerequisite = subjects[6], Postrequisite = subjects[11], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[7], Postrequisite = subjects[12], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[8], Postrequisite = subjects[12], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[8], Postrequisite = subjects[13], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[9], Postrequisite = subjects[13], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[9], Postrequisite = subjects[17], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[9], Postrequisite = subjects[16], StudyPlan = studyplans[2] },
                // Semestre 4
                new() { Prerequisite = subjects[6], Postrequisite = subjects[18], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[11], Postrequisite = subjects[19], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[12], Postrequisite = subjects[20], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[13], Postrequisite = subjects[21], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[17], Postrequisite = subjects[23], StudyPlan = studyplans[2] },
                // Semestre 5
                new() { Prerequisite = subjects[18], Postrequisite = subjects[24], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[20], Postrequisite = subjects[27], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[23], Postrequisite = subjects[31], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[23], Postrequisite = subjects[32], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[16], Postrequisite = subjects[33], StudyPlan = studyplans[2] },
                // Semestre 6
                new() { Prerequisite = subjects[24], Postrequisite = subjects[34], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[27], Postrequisite = subjects[37], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[31], Postrequisite = subjects[36], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[32], Postrequisite = subjects[40], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[33], Postrequisite = subjects[41], StudyPlan = studyplans[2] },
                // Semestre 7
                new() { Prerequisite = subjects[34], Postrequisite = subjects[42], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[36], Postrequisite = subjects[44], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[36], Postrequisite = subjects[45], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[33], Postrequisite = subjects[45], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[21], Postrequisite = subjects[48], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[40], Postrequisite = subjects[48], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[33], Postrequisite = subjects[49], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[41], Postrequisite = subjects[49], StudyPlan = studyplans[2] },
                // Semestre 8
                new() { Prerequisite = subjects[42], Postrequisite = subjects[50], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[45], Postrequisite = subjects[53], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[45], Postrequisite = subjects[56], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[45], Postrequisite = subjects[57], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[49], Postrequisite = subjects[57], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[49], Postrequisite = subjects[58], StudyPlan = studyplans[2] },
                // Semestre 9 
                new() { Prerequisite = subjects[50], Postrequisite = subjects[59], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[53], Postrequisite = subjects[59], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[56], Postrequisite = subjects[59], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[57], Postrequisite = subjects[59], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[58], Postrequisite = subjects[59], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[53], Postrequisite = subjects[61], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[56], Postrequisite = subjects[61], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[57], Postrequisite = subjects[61], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[58], Postrequisite = subjects[61], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[53], Postrequisite = subjects[63], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[56], Postrequisite = subjects[63], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[57], Postrequisite = subjects[63], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[58], Postrequisite = subjects[63], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[53], Postrequisite = subjects[64], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[56], Postrequisite = subjects[64], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[57], Postrequisite = subjects[64], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[58], Postrequisite = subjects[64], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[58], Postrequisite = subjects[65], StudyPlan = studyplans[2] },
                // Semestre 10
                new() { Prerequisite = subjects[59], Postrequisite = subjects[66], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[61], Postrequisite = subjects[66], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[63], Postrequisite = subjects[66], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[64], Postrequisite = subjects[66], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[65], Postrequisite = subjects[66], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[59], Postrequisite = subjects[67], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[61], Postrequisite = subjects[67], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[63], Postrequisite = subjects[67], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[64], Postrequisite = subjects[67], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[65], Postrequisite = subjects[67], StudyPlan = studyplans[2] },
                // Modalidad de Graduacion
                new() { Prerequisite = subjects[59], Postrequisite = subjects[71], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[61], Postrequisite = subjects[71], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[63], Postrequisite = subjects[71], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[64], Postrequisite = subjects[71], StudyPlan = studyplans[2] },
                new() { Prerequisite = subjects[65], Postrequisite = subjects[71], StudyPlan = studyplans[2] },
            };
            context.SubjectDependencies.AddRange(prerequisites);

            var spsubjects = new List<SpSubject>()
            {
                // INFORMATICA (Antigua)
                // Semestre 1
                new() { StudyPlan = studyplans[0], Subject = subjects[0], Credits = 5, Level = 1, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[1], Credits = 5, Level = 1, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[2], Credits = 5, Level = 1, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[3], Credits = 6, Level = 1, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[4], Credits = 4, Level = 1, Type = 1},
                // Semestre 2
                new() { StudyPlan = studyplans[0], Subject = subjects[6], Credits = 5, Level = 2, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[7], Credits = 5, Level = 2, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[8], Credits = 5, Level = 2, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[9], Credits = 6, Level = 2, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[10], Credits = 4, Level = 2, Type = 1},
                // Semestre 3
                new() { StudyPlan = studyplans[0], Subject = subjects[11], Credits = 5, Level = 3, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[12], Credits = 5, Level = 3, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[13], Credits = 5, Level = 3, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[14], Credits = 6, Level = 3, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[15], Credits = 4, Level = 3, Type = 1},

                new() { StudyPlan = studyplans[0], Subject = subjects[16], Credits = 4, Level = 3, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[17], Credits = 5, Level = 3, Type = 2},
                // Semestre 4
                new() { StudyPlan = studyplans[0], Subject = subjects[18], Credits = 5, Level = 4, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[19], Credits = 5, Level = 4, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[20], Credits = 5, Level = 4, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[21], Credits = 5, Level = 4, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[22], Credits = 4, Level = 4, Type = 1},

                new() { StudyPlan = studyplans[0], Subject = subjects[23], Credits = 4, Level = 4, Type = 2},
                // Semestre 5
                new() { StudyPlan = studyplans[0], Subject = subjects[24], Credits = 5, Level = 5, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[25], Credits = 5, Level = 5, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[26], Credits = 5, Level = 5, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[27], Credits = 5, Level = 5, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[28], Credits = 5, Level = 5, Type = 0},

                new() { StudyPlan = studyplans[0], Subject = subjects[29], Credits = 5, Level = 5, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[30], Credits = 5, Level = 5, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[31], Credits = 5, Level = 5, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[32], Credits = 6, Level = 5, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[33], Credits = 4, Level = 5, Type = 2},

                new() { StudyPlan = studyplans[0], Subject = subjects[72], Credits = 3, Level = 5, Type = 3},
                new() { StudyPlan = studyplans[0], Subject = subjects[73], Credits = 3, Level = 5, Type = 3},
                // Semestre 6
                new() { StudyPlan = studyplans[0], Subject = subjects[34], Credits = 5, Level = 6, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[35], Credits = 5, Level = 6, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[36], Credits = 5, Level = 6, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[37], Credits = 5, Level = 6, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[38], Credits = 5, Level = 6, Type = 0},

                new() { StudyPlan = studyplans[0], Subject = subjects[39], Credits = 5, Level = 6, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[40], Credits = 5, Level = 6, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[41], Credits = 5, Level = 6, Type = 2},

                new() { StudyPlan = studyplans[0], Subject = subjects[74], Credits = 3, Level = 6, Type = 3},
                new() { StudyPlan = studyplans[0], Subject = subjects[75], Credits = 3, Level = 6, Type = 3},
                // Semestre 7
                new() { StudyPlan = studyplans[0], Subject = subjects[42], Credits = 5, Level = 7, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[43], Credits = 5, Level = 7, Type = 0},
                new() { StudyPlan = studyplans[0], Subject = subjects[44], Credits = 5, Level = 7, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[45], Credits = 5, Level = 7, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[46], Credits = 5, Level = 7, Type = 1},

                new() { StudyPlan = studyplans[0], Subject = subjects[47], Credits = 5, Level = 7, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[48], Credits = 5, Level = 7, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[49], Credits = 5, Level = 7, Type = 2},

                new() { StudyPlan = studyplans[0], Subject = subjects[76], Credits = 3, Level = 7, Type = 3},
                new() { StudyPlan = studyplans[0], Subject = subjects[77], Credits = 3, Level = 7, Type = 3},
                // Semestre 8
                new() { StudyPlan = studyplans[0], Subject = subjects[50], Credits = 5, Level = 8, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[51], Credits = 5, Level = 8, Type = 0},
                new() { StudyPlan = studyplans[0], Subject = subjects[52], Credits = 4, Level = 8, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[53], Credits = 5, Level = 8, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[54], Credits = 5, Level = 8, Type = 1},

                new() { StudyPlan = studyplans[0], Subject = subjects[55], Credits = 4, Level = 8, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[56], Credits = 4, Level = 8, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[57], Credits = 5, Level = 8, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[58], Credits = 5, Level = 8, Type = 2},

                new() { StudyPlan = studyplans[0], Subject = subjects[78], Credits = 3, Level = 8, Type = 3},
                new() { StudyPlan = studyplans[0], Subject = subjects[79], Credits = 3, Level = 8, Type = 3},
                // Semestre 9
                new() { StudyPlan = studyplans[0], Subject = subjects[59], Credits = 5, Level = 9, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[60], Credits = 5, Level = 9, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[61], Credits = 5, Level = 9, Type = 1},
                new() { StudyPlan = studyplans[0], Subject = subjects[62], Credits = 4, Level = 9, Type = 1},

                new() { StudyPlan = studyplans[0], Subject = subjects[63], Credits = 4, Level = 9, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[64], Credits = 5, Level = 9, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[65], Credits = 5, Level = 9, Type = 2},
                // Semestre 10 
                new() { StudyPlan = studyplans[0], Subject = subjects[66], Credits = 5, Level = 10, Type = 2},
                new() { StudyPlan = studyplans[0], Subject = subjects[67], Credits = 5, Level = 10, Type = 1},
                // Materias de Titulacion
                new() { StudyPlan = studyplans[0], Subject = subjects[68], Credits = 5, Level = 5, Type = 4},
                new() { StudyPlan = studyplans[0], Subject = subjects[69], Credits = 5, Level = 7, Type = 4},
                new() { StudyPlan = studyplans[0], Subject = subjects[70], Credits = 5, Level = 10, Type = 4},
                new() { StudyPlan = studyplans[0], Subject = subjects[71], Credits = 4, Level = 10, Type = 4},
            };
            context.SpSubjects.AddRange(spsubjects);

            var gestions = new List<Gestion>()
            {
                new() { Year = 2020},
                new() { Year = 2021},
                new() { Year = 2022},
                new() { Year = 2023},
                new() { Year = 2024},
                new() { Year = 2025},
                new() { Year = 2077},
            };
            context.Gestions.AddRange(gestions);

            var periods = new List<Period>()
            {
                new() { Number = 1, Gestion = gestions[0]}, //0. 1er semestre
                new() { Number = 2, Gestion = gestions[0]}, //1. 2do semestre
                new() { Number = 3, Gestion = gestions[0]}, //2. Verano
                new() { Number = 4, Gestion = gestions[0]}, //3. Mesa del 1er Semestre
                new() { Number = 5, Gestion = gestions[0]}, //4. Mesa del 2do Semestre
                new() { Number = 6, Gestion = gestions[0]}, //5. Mesa extraordinaria 1er semestre
                new() { Number = 7, Gestion = gestions[0]}, //6. Mesa extraordianria 2do semestre
                new() { Number = 8, Gestion = gestions[0]}, //7. Mesa extraordinaria verano
                new() { Number = 1, Gestion = gestions[1]}, //8. 1er semestre
                new() { Number = 2, Gestion = gestions[1]}, //9 2do semestre
                new() { Number = 3, Gestion = gestions[1]}, //10 Verano
                new() { Number = 4, Gestion = gestions[1]}, //11 Mesa del 1er Semestre
                new() { Number = 5, Gestion = gestions[1]}, //12 Mesa del 2do Semestre
                new() { Number = 6, Gestion = gestions[1]}, //13 Mesa extraordinaria 1er semestre
                new() { Number = 7, Gestion = gestions[1]}, //14 Mesa extraordianria 2do semestre
                new() { Number = 8, Gestion = gestions[1]}, //15 Mesa extraordinaria verano
                new() { Number = 1, Gestion = gestions[2]}, //16 1er semestre
                new() { Number = 2, Gestion = gestions[2]}, //17 2do semestre
                new() { Number = 3, Gestion = gestions[2]}, //18 Verano
                new() { Number = 4, Gestion = gestions[2]}, //19 Mesa del 1er Semestre
                new() { Number = 5, Gestion = gestions[2]}, //20 Mesa del 2do Semestre
                new() { Number = 6, Gestion = gestions[2]}, //21 Mesa extraordinaria 1er semestre
                new() { Number = 7, Gestion = gestions[2]}, //22 Mesa extraordianria 2do semestre
                new() { Number = 8, Gestion = gestions[2]}, //23 Mesa extraordinaria verano
                new() { Number = 1, Gestion = gestions[3]}, //24 1er semestre
                new() { Number = 2, Gestion = gestions[3]}, //25 2do semestre
                new() { Number = 3, Gestion = gestions[3]}, //26 Verano
                new() { Number = 4, Gestion = gestions[3]}, //27 Mesa del 1er Semestre
                new() { Number = 5, Gestion = gestions[3]}, //28 Mesa del 2do Semestre
                new() { Number = 6, Gestion = gestions[3]}, //29 Mesa extraordinaria 1er semestre
                new() { Number = 7, Gestion = gestions[3]}, //30 Mesa extraordianria 2do semestre
                new() { Number = 8, Gestion = gestions[3]}, //31 Mesa extraordinaria verano
                new() { Number = 1, Gestion = gestions[4]}, //32 1er semestre
                new() { Number = 2, Gestion = gestions[4]}, //33 2do semestre
                new() { Number = 3, Gestion = gestions[4]}, //34 Verano
                new() { Number = 4, Gestion = gestions[4]}, //35 Mesa del 1er Semestre
                new() { Number = 5, Gestion = gestions[4]}, //36 Mesa del 2do Semestre
                new() { Number = 6, Gestion = gestions[4]}, //37 Mesa extraordinaria 1er semestre
                new() { Number = 7, Gestion = gestions[4]}, //38 Mesa extraordianria 2do semestre
                new() { Number = 8, Gestion = gestions[4]}, //39 Mesa extraordinaria verano
                new() { Number = 1, Gestion = gestions[5]}, //40 1er semestre
                new() { Number = 2, Gestion = gestions[5]}, //41 2do semestre
                new() { Number = 3, Gestion = gestions[5]}, //42 Verano
                new() { Number = 4, Gestion = gestions[5]}, //43 Mesa del 1er Semestre
                new() { Number = 5, Gestion = gestions[5]}, //44 Mesa del 2do Semestre
                new() { Number = 6, Gestion = gestions[5]}, //45 Mesa extraordinaria 1er semestre
                new() { Number = 7, Gestion = gestions[5]}, //46 Mesa extraordianria 2do semestre
                new() { Number = 8, Gestion = gestions[5]}, //47 Mesa extraordinaria verano
            };
            context.Periods.AddRange(periods);

            var modules = new List<Module>()
            {
                new() { Number = 236},
                new() { Number = 100},
            };
            context.Modules.AddRange(modules);

            var rooms = new List<Room>()
            {
                new() { Module = modules[0], Number = 11},//0
                new() { Module = modules[0], Number = 12},//1
                new() { Module = modules[0], Number = 13},//2
                new() { Module = modules[0], Number = 14},//3
                new() { Module = modules[0], Number = 15},//4
                new() { Module = modules[0], Number = 16},//5
                new() { Module = modules[0], Number = 17},//6
                new() { Module = modules[0], Number = 21},//7
                new() { Module = modules[0], Number = 22},//8
                new() { Module = modules[0], Number = 23},//9
                new() { Module = modules[0], Number = 24},//10
                new() { Module = modules[0], Number = 25},//11
                new() { Module = modules[0], Number = 26},//12
                new() { Module = modules[0], Number = 27},//13
                new() { Module = modules[0], Number = 31},//14
                new() { Module = modules[0], Number = 32},//15
                new() { Module = modules[0], Number = 33},//16
                new() { Module = modules[0], Number = 34},//17
                new() { Module = modules[0], Number = 35},//18
                new() { Module = modules[0], Number = 36},//19
                new() { Module = modules[0], Number = 37},//20
                new() { Module = modules[0], Number = 38},//21
                new() { Module = modules[0], Number = 40},//22
                new() { Module = modules[0], Number = 41},//23
                new() { Module = modules[0], Number = 42},//24
                new() { Module = modules[0], Number = 43},//25
                new() { Module = modules[0], Number = 44},//26
                new() { Module = modules[0], Number = 45},//27
                new() { Module = modules[0], Number = 46},//28
                new() { Module = modules[0], Number = 51},//28
            };
            context.Rooms.AddRange(rooms);

            var students = new List<Student>()
            {
                new() { FirstName = "Raul", LastName = "Farell Vaca"},
                new() { FirstName = "Santiago", LastName = "Contreras Fuentes" },
                new() { FirstName = "Joaquin", LastName = "Chumacero" },
                new() { FirstName = "Saturnino", LastName = "Mamani" },
                new() { FirstName = "Carlos", LastName = "Coca" },
            };
            context.Students.AddRange(students);

            var teachers = new List<Teacher>()
            {
                new() { FirstName = "", LastName = ""},
                new() { FirstName = "Rufino", LastName = ""},
                new() { FirstName = "Angelica", LastName = "Garzon"},
                new() { FirstName = "Braulio", LastName = "Caceres"},
                new() { FirstName = "Elizabeth", LastName = "Oropeza"},
                new() { FirstName = "", LastName = "Siles"},
                new() { FirstName = "Miguel", LastName = "Peinado"},
                new() { FirstName = "Shirley", LastName = "Perez"},
            };
            context.Teachers.AddRange(teachers);

            var studentsstudyplans = new List<StudentStudyPlan>()
            {
                new() {Student = students[0], StudyPlans = studyplans[0]},
                new() {Student = students[1], StudyPlans = studyplans[0]},
                new() {Student = students[2], StudyPlans = studyplans[0]},
            };
            context.StudentStudyPlans.AddRange(studentsstudyplans);

            var inscriptions = new List<Inscription>()
            {
                new() { Student = students[1], Period = periods[16], Type = 0}, //1-2022 (2*8+1-1)
                new() { Student = students[1], Period = periods[17], Type = 0}, //2-2022  (2*8+2-1)
                new() { Student = students[1], Period = periods[24], Type = 0}, //1-2023  (3*8+1-1)
                new() { Student = students[1], Period = periods[25], Type = 0}, //2-2023
                new() { Student = students[1], Period = periods[32], Type = 0}, //1-2024
                new() { Student = students[1], Period = periods[33], Type = 0}, //2-2024
                new() { Student = students[1], Period = periods[33], Type = 2}, 
                new() { Student = students[1], Period = periods[40], Type = 0},
                new() { Student = students[1], Period = periods[40], Type = 1},
                new() { Student = students[1], Period = periods[40], Type = 2},
                new() { Student = students[1], Period = periods[41], Type = 0},
                new() { Student = students[1], Period = periods[41], Type = 1},
                new() { Student = students[1], Period = periods[41], Type = 2},
                new() { Student = students[0], Period = periods[41], Type = 0},
                new() { Student = students[2], Period = periods[16], Type = 0},
            };
            context.Inscriptions.AddRange(inscriptions);

            var groups = new List<Group>()
            {
                new() { Code = "SA", Mode = "Presencial", Period = periods[40], Subject = subjects[38], Teacher = teachers[0]},
                new() { Code = "SA", Mode = "Presencial", Period = periods[40], Subject = subjects[46], Teacher = teachers[0]},
                new() { Code = "SB", Mode = "Presencial", Period = periods[40], Subject = subjects[44], Teacher = teachers[0]},
                new() { Code = "SA", Mode = "Presencial", Period = periods[40], Subject = subjects[43], Teacher = teachers[0]},
                new() { Code = "SA", Mode = "Presencial", Period = periods[40], Subject = subjects[45], Teacher = teachers[0]},
                new() { Code = "SA", Mode = "Presencial", Period = periods[40], Subject = subjects[50], Teacher = teachers[0]},
                new() { Code = "SA", Mode = "Presencial", Period = periods[41], Subject = subjects[78], Teacher = teachers[7], Quota = 20},
                new() { Code = "SA", Mode = "Presencial", Period = periods[41], Subject = subjects[74], Teacher = teachers[6], Quota = 80},
                new() { Code = "SC", Mode = "Presencial", Period = periods[41], Subject = subjects[53], Teacher = teachers[0]},
                new() { Code = "SB", Mode = "Presencial", Period = periods[41], Subject = subjects[46], Teacher = teachers[0]},
                new() { Code = "Z2", Mode = "Presencial", Period = periods[16], Subject = subjects[3], Teacher = teachers[1]},
                new() { Code = "Z2", Mode = "Presencial", Period = periods[16], Subject = subjects[2], Teacher = teachers[2]},
                new() { Code = "Z2", Mode = "Presencial", Period = periods[16], Subject = subjects[1], Teacher = teachers[3]},
                new() { Code = "Z2", Mode = "Presencial", Period = periods[16], Subject = subjects[4], Teacher = teachers[4]},
                new() { Code = "Z2", Mode = "Presencial", Period = periods[16], Subject = subjects[0], Teacher = teachers[5]},
                new() { Code = "SB", Mode = "Presencial", Period = periods[41], Subject = subjects[78], Teacher = teachers[7], Quota = 20},
            };
            context.Groups.AddRange(groups);

            var timeslots = new List<TimeSlot>()
            {
                new() { Day = "Monday", StartTime = new(11,30), EndTime = new(13,00), Room = rooms[10], Group = groups[0] }, // Compiladores
                new() { Day = "Wednesday", StartTime = new(11,30), EndTime = new(13,00), Room = rooms[27], Group = groups[0]},
                new() { Day = "Friday", StartTime = new(11,30), EndTime = new(13,00), Room = rooms[10], Group = groups[0]},
                new() { Day = "Monday", StartTime = new(19,45), EndTime = new(21,15), Room = rooms[1], Group = groups[4]}, // Redes I
                new() { Day = "Wednesday", StartTime = new(19,45), EndTime = new(21,15), Room = rooms[1], Group = groups[4]},
                new() { Day = "Friday", StartTime = new(19,45), EndTime = new(21,15), Room = rooms[1], Group = groups[4]},
                new() { Day = "Tuesday", StartTime = new(7,00), EndTime = new(9,15), Room = rooms[9], Group = groups[1]}, // SI II
                new() { Day = "Thursday", StartTime = new(7,00), EndTime = new(9,15), Room = rooms[27], Group = groups[1]},
                new() { Day = "Tuesday", StartTime = new(9,15), EndTime = new(11,30), Room = rooms[3], Group = groups[5]},
                new() { Day = "Thursday", StartTime = new(9,15), EndTime = new(11,30), Room = rooms[3], Group = groups[5]},
                new() { Day = "Tuesday", StartTime = new(16,00), EndTime = new(18,15), Room = rooms[28], Group = groups[2]},
                new() { Day = "Thursday", StartTime = new(16,00), EndTime = new(18,15), Room = rooms[23], Group = groups[2]},
                new() { Day = "Tuesday", StartTime = new(18,15), EndTime = new(20,30), Room = rooms[28], Group = groups[3]},
                new() { Day = "Thursday", StartTime = new(18,15), EndTime = new(20,30), Room = rooms[28], Group = groups[3]},
                new() { Day = "Tuesday", StartTime = new(16,00), EndTime = new(17,30), Room = rooms[12], Group = groups[6]},
                new() { Day = "Thursday", StartTime = new(16,00), EndTime = new(17,30), Room = rooms[12], Group = groups[6]},
                new() { Day = "Tuesday", StartTime = new(18,15), EndTime = new(20,30), Room = rooms[17], Group = groups[7]},
                new() { Day = "Thursday", StartTime = new(18,15), EndTime = new(20,30), Room = rooms[25], Group = groups[7]},
                new() { Day = "Tuesday", StartTime = new(17,30), EndTime = new(19,00), Room = rooms[17], Group = groups[15]},
                new() { Day = "Thursday", StartTime = new(17,30), EndTime = new(19,00), Room = rooms[25], Group = groups[15]},
                new() { Day = "Monday", StartTime = new(18,15), EndTime = new(19,45), Room = rooms[11], Group = groups[8]},
                new() { Day = "Wednesday", StartTime = new(18,15), EndTime = new(19,45), Room = rooms[11], Group = groups[8]},
                new() { Day = "Friday", StartTime = new(18,15), EndTime = new(19,45), Room = rooms[11], Group = groups[8]},
                new() { Day = "Monday", StartTime = new(16,45), EndTime = new(18,15), Room = rooms[24], Group = groups[9]},
                new() { Day = "Wednesday", StartTime = new(16,45), EndTime = new(18,15), Room = rooms[24], Group = groups[9]},
                new() { Day = "Friday", StartTime = new(16,45), EndTime = new(18,15), Room = rooms[24], Group = groups[9]},
            };
            context.TimeSlots.AddRange(timeslots);

            var groupinscriptions = new List<GroupInscription>
            {
                new() { Group = groups[0], Inscription = inscriptions[7]},
                new() { Group = groups[2], Inscription = inscriptions[7]},
                new() { Group = groups[3], Inscription = inscriptions[7]},
                new() { Group = groups[4], Inscription = inscriptions[7]},
                new() { Group = groups[5], Inscription = inscriptions[7]},
                new() { Group = groups[1], Inscription = inscriptions[8]},
                new() { Group = groups[1], Inscription = inscriptions[9]},
                new() { Group = groups[2], Inscription = inscriptions[9]},
                new() { Group = groups[6], Inscription = inscriptions[13]},
                new() { Group = groups[7], Inscription = inscriptions[13]},
                new() { Group = groups[6], Inscription = inscriptions[10]},
                new() { Group = groups[7], Inscription = inscriptions[10]},
                new() { Group = groups[9], Inscription = inscriptions[10]},
                new() { Group = groups[8], Inscription = inscriptions[11]},
                new() { Group = groups[9], Inscription = inscriptions[12]},
                new() { Group = groups[10], Inscription = inscriptions[14]},
                new() { Group = groups[11], Inscription = inscriptions[14]},
                new() { Group = groups[12], Inscription = inscriptions[14]},
                new() { Group = groups[13], Inscription = inscriptions[14]},
                new() { Group = groups[14], Inscription = inscriptions[14]},
            };
            context.GroupInscriptions.AddRange(groupinscriptions);

            var studentgroups = new List<StudentGroups>
            {
                new() { Group = groups[0], Student = students[0], Grade = 83, Status = 1},
                new() { Group = groups[3], Student = students[0], Grade = 80, Status = 1},
                new() { Group = groups[4], Student = students[0], Grade = 63, Status = 1},
                new() { Group = groups[5], Student = students[0], Grade = 70, Status = 1},
                new() { Group = groups[1], Student = students[0], Status = 2},
                new() { Group = groups[2], Student = students[0], Status = 2},
                new() { Group = groups[6], Student = students[1]},
                new() { Group = groups[7], Student = students[1]},
                new() { Group = groups[6], Student = students[0]},
                new() { Group = groups[7], Student = students[0]},
                new() { Group = groups[8], Student = students[0]},
                new() { Group = groups[9], Student = students[0]},
                new() { Group = groups[10], Student = students[2], Grade = 57, Status = 1},
                new() { Group = groups[11], Student = students[2], Grade = 95, Status = 1},
                new() { Group = groups[12], Student = students[2], Grade = 51, Status = 1},
                new() { Group = groups[13], Student = students[2], Grade = 82, Status = 1},
                new() { Group = groups[14], Student = students[2], Grade = 66, Status = 1},
            };
            context.StudentGroups.AddRange(studentgroups);

            context.SaveChanges();
        }
    }
}