using System.Collections;
using System.Collections.Generic;

namespace RecruitmentTask
{
    public class User
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public decimal Commission { get; set; } = 0;

        public User Parent { get; set; }
        public IList<User> Childs { get; set; }
    }
}