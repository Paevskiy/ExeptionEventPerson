using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading.Tasks;

namespace ExeptionAttempt
{
    #region Events for Name and Age
    // По большому счету, код переписан из нашего исходного файла, с некоторыми упрощениями
    // ниже объявляю события на изменения в Возрасте и Имени, каждому своё, можно ли как то упростить
    public class ChangeNameEventArg : EventArgs
    {
        public string OldName { get; private set; }
        public string NewName { get; private set; }

        public ChangeNameEventArg(string oldName, string newName)
        {
            OldName = oldName;
            NewName = newName;
        }
    }
    public class ChangeAgeEventArg : EventArgs// что такое EventArgs??? почему идет после ":", и какие варианты замены есть?
    {
        public int OldAge { get; private set; }
        public int NewAge { get; private set; }

        public ChangeAgeEventArg(int oldAge, int newAge)
        {
            OldAge = oldAge;
            NewAge = newAge;
        }
    }
    #endregion

    #region ClassPerson
    /// <summary>
    /// НАконец добрался до Класса персона, протестировал работу автоматически свойств GET / SET, так и созданные в ручную
    /// </summary>
    class Person
    {
        public event EventHandler<ChangeNameEventArg> OnChangeName;
        public event EventHandler<ChangeAgeEventArg> OnChangeAge;

        private string name;
        private int age;

        public string Name
        {
            get { return this.name; }
            set
            {
                OnChangeName?.Invoke(this, new ChangeNameEventArg(Name, value));
                name = value;
            }


        }
        public int Age
        {
            get
            {
                return this.age;
            }
            set
            {
                OnChangeAge?.Invoke(this, new ChangeAgeEventArg(Age, value));
                age = value;
                if (value < 1 || value >= 100)
                {
                    
                    throw new ExceptionAge();
                }
                else age = value;

            }
        }
    }
    #endregion

    #region BodyOFProgram
    class Program
        {
            static void Main(string[] args)
            {
                Person Human = new Person();
                try
                {
                // присваиваем значение вновь созданному объекту класса Person
                Human.Name = "Andrey";
                Human.Age = 30;

                Console.WriteLine("\nHis current name - {0}, His current age - {1}\n", Human.Name, Human.Age);

                //Изменяем значение класса Person и отрабатываем события
                Human.OnChangeName += OnChangedName;
                Human.Name = "Nikolay";
                Human.OnChangeAge += OnChangedAge;
                Human.Age = 31;

                }

                 catch (ExceptionAge m)
                {
                    Console.WriteLine("{0}", m.Message);
                }
                finally
                {
                    Console.WriteLine("His new name - {0}, His real age - {1}\n", Human.Name, Human.Age);
                    Console.WriteLine("Done");
                    Console.ReadKey();
                }

            }
        /// <summary>
        /// ниже две строчки интересно что они задаю, парам мы проходили но надо повторить.
        /// далее по тексту программы идут текст выводимый при условии наступления событий,
        /// изменение имени
        /// инменение возраста.
        /// </summary>
        /// <param name="senderName"></param>
        /// <param name="e"></param>
        public static void OnChangedName(object senderName, ChangeNameEventArg e)
        {
            Console.WriteLine(new string('=', 79));
            Console.WriteLine("name was changed from {0} to {1}", e.OldName, e.NewName);
            Console.WriteLine(new string('=', 79));
            
        }
        public static void OnChangedAge(object senderAge, ChangeAgeEventArg ageEvent)
        {
            Console.WriteLine("age was changed from {0} to {1}", ageEvent.OldAge, ageEvent.NewAge);
            Console.WriteLine(new string('=', 79));
            Console.WriteLine("");
            
        }

    }
    #endregion

    #region ExceptionAGe
    /// <summary>
    /// блок для вывода собственного текста исключения
    /// </summary>
    class ExceptionAge : Exception
        {
            public override string Message
            {
                get { return "Wrong age"; }
            }

        }
    #endregion
}
